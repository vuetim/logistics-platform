using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities;

public class Order : BaseEntity
{
    // =========================
    // Core
    // =========================

    public string OrderNumber { get; private set; } = string.Empty;

    public Guid CustomerId { get; private set; }
    public Customer Customer { get; set; } = null!; // EF needs set

    public Guid? PreferredCarrierId { get; private set; }
    public Carrier? PreferredCarrier { get; set; }

    public OrderType OrderType { get; private set; }
    public OrderDirection Direction { get; private set; }

    public OrderStatus Status { get; private set; } = OrderStatus.Draft;
    public OrderPhase Phase { get; private set; } = OrderPhase.Open;

    public Guid CreatedByUserId { get; private set; }
    public User CreatedByUser { get; set; } = null!;

    // =========================
    // Planning
    // =========================

    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public DateTime? PlannedPickupDate { get; private set; }
    public DateTime? PlannedDeliveryDate { get; private set; }

    // =========================
    // Business
    // =========================

    public string? PrimaryPONumber { get; private set; }
    public string? PrimaryBolNumber { get; private set; }
    public string? PrimaryProNumber { get; private set; }

    public string? Commodity { get; private set; }
    public decimal? TotalWeight { get; private set; }
    public int? TotalPallets { get; private set; }
    public decimal? TotalVolume { get; private set; }

    public string? DispatchNotes { get; private set; }
    public string? DeliveryNotes { get; private set; }

    public decimal? CustomerRate { get; private set; }

    // =========================
    // Child aggregates (EF friendly)
    // =========================

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    public ICollection<OrderEquipmentRequirement> EquipmentRequirements { get; set; }
        = new List<OrderEquipmentRequirement>();

    public OrderCost? Cost { get; set; }

    public ICollection<OrderExternalId> ExternalIds { get; set; }
        = new List<OrderExternalId>();

    public ICollection<OrderNote> Notes { get; set; } = new List<OrderNote>();

    public ICollection<OrderDocument> Documents { get; set; } = new List<OrderDocument>();

    public ICollection<OrderRoute> OrderRoutes { get; set; } = new List<OrderRoute>();

    public ICollection<LoadOrder> Loads { get; set; } = new List<LoadOrder>();

    // =========================
    // EF constructor
    // =========================

    private Order() { }

    // =========================
    // Factory
    // =========================

    public static Order Create(
        string number,
        Guid customerId,
        Guid createdByUserId,
        OrderType type,
        OrderDirection direction,
        DateTime start,
        DateTime end,
        Guid? preferredCarrierId = null)
    {
        if (start > end)
            throw new InvalidOperationException("Start date cannot be after End date.");

        return new Order
        {
            OrderNumber = number,
            CustomerId = customerId,
            CreatedByUserId = createdByUserId,
            PreferredCarrierId = preferredCarrierId,
            OrderType = type,
            Direction = direction,
            StartDate = start,
            EndDate = end
        };
    }

    // =====================================================
    // Domain behavior (business rules)
    // =====================================================

    public void UpdatePlanning(DateTime? pickup, DateTime? delivery)
    {
        EnsureEditable();
        PlannedPickupDate = pickup;
        PlannedDeliveryDate = delivery;
    }

    public void UpdateCore(
        OrderType? type,
        OrderDirection? direction,
        DateTime? start,
        DateTime? end,
        Guid? preferredCarrierId)
    {
        EnsureEditable();

        if (type.HasValue)
            OrderType = type.Value;

        if (direction.HasValue)
            Direction = direction.Value;

        var finalStart = start ?? StartDate;
        var finalEnd = end ?? EndDate;

        if (finalStart > finalEnd)
            throw new InvalidOperationException("Start date cannot be after End date.");

        StartDate = finalStart;
        EndDate = finalEnd;

        PreferredCarrierId = preferredCarrierId;
    }

    public void UpdateNotes(string? dispatch, string? delivery)
    {
        EnsureEditable();
        DispatchNotes = dispatch;
        DeliveryNotes = delivery;
    }

    public void SetCustomerRate(decimal rate)
    {
        CustomerRate = rate;
    }
    public void SetBusinessDetails(string? poPo, string? bolNumber, string? proNumber,
    string? commodity, decimal? weight, int? pallets, decimal? volume)
    {
        EnsureEditable();
        PrimaryPONumber = poPo;
        PrimaryBolNumber = bolNumber;
        PrimaryProNumber = proNumber;
        Commodity = commodity;
        TotalWeight = weight;
        TotalPallets = pallets;
        TotalVolume = volume;
    }
    public void ChangeStatus(OrderStatus newStatus)
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Completed order cannot change status.");

        if (!IsManualTransitionAllowed(Status, newStatus))
            throw new InvalidOperationException($"Invalid status transition: {Status} -> {newStatus}");

        ApplyStatus(newStatus);
    }

    public bool TrySyncStatusFromExecution(OrderStatus newStatus)
    {
        if (Status == OrderStatus.Completed || Status == OrderStatus.Cancelled)
            return false;

        if (newStatus == OrderStatus.Cancelled)
        {
            ApplyStatus(OrderStatus.Cancelled);
            return true;
        }

        var currentRank = GetWorkflowRank(Status);
        var nextRank = GetWorkflowRank(newStatus);
        if (nextRank < 0 || currentRank < 0)
            return false;

        if (nextRank <= currentRank)
            return false;

        ApplyStatus(newStatus);
        return true;
    }

    private void EnsureEditable()
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Completed order cannot be edited.");
    }

    private void ApplyStatus(OrderStatus newStatus)
    {
        Status = newStatus;
        Phase = ResolvePhase(newStatus);
    }

    private static OrderPhase ResolvePhase(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Draft or OrderStatus.Submitted => OrderPhase.Open,
            OrderStatus.Confirmed or OrderStatus.Scheduled => OrderPhase.Plan,
            OrderStatus.Dispatched or
            OrderStatus.AtPickup or
            OrderStatus.PickedUp or
            OrderStatus.InTransit or
            OrderStatus.AtDelivery or
            OrderStatus.Delivered => OrderPhase.Ship,
            OrderStatus.ReadyForBilling or OrderStatus.Billed => OrderPhase.Bill,
            OrderStatus.Completed => OrderPhase.Complete,
            OrderStatus.Cancelled => OrderPhase.Cancelled,
            _ => PhaseFallback(status)
        };
    }

    private static OrderPhase PhaseFallback(OrderStatus status)
    {
        return status == OrderStatus.Cancelled
            ? OrderPhase.Cancelled
            : OrderPhase.Open;
    }

    private static bool IsManualTransitionAllowed(OrderStatus from, OrderStatus to)
    {
        if (from == to)
            return true;

        return from switch
        {
            OrderStatus.Draft => to is OrderStatus.Submitted or OrderStatus.Cancelled,
            OrderStatus.Submitted => to is OrderStatus.Confirmed or OrderStatus.Scheduled or OrderStatus.Cancelled,
            OrderStatus.Confirmed => to is OrderStatus.Scheduled or OrderStatus.Cancelled,
            OrderStatus.Scheduled => to is OrderStatus.Dispatched or OrderStatus.Cancelled,
            OrderStatus.Dispatched => to is OrderStatus.AtPickup or OrderStatus.InTransit or OrderStatus.Cancelled,
            OrderStatus.AtPickup => to is OrderStatus.PickedUp or OrderStatus.InTransit or OrderStatus.Cancelled,
            OrderStatus.PickedUp => to is OrderStatus.InTransit or OrderStatus.AtDelivery or OrderStatus.Cancelled,
            OrderStatus.InTransit => to is OrderStatus.AtDelivery or OrderStatus.Delivered or OrderStatus.Cancelled,
            OrderStatus.AtDelivery => to is OrderStatus.Delivered or OrderStatus.Cancelled,
            OrderStatus.Delivered => to is OrderStatus.ReadyForBilling or OrderStatus.Completed or OrderStatus.Cancelled,
            OrderStatus.ReadyForBilling => to is OrderStatus.Billed or OrderStatus.Completed or OrderStatus.Cancelled,
            OrderStatus.Billed => to is OrderStatus.Completed or OrderStatus.Cancelled,
            OrderStatus.Completed or OrderStatus.Cancelled => false,
            _ => false
        };
    }

    private static int GetWorkflowRank(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Draft => 1,
            OrderStatus.Submitted => 2,
            OrderStatus.Confirmed => 3,
            OrderStatus.Scheduled => 4,
            OrderStatus.Dispatched => 5,
            OrderStatus.AtPickup => 6,
            OrderStatus.PickedUp => 7,
            OrderStatus.InTransit => 8,
            OrderStatus.AtDelivery => 9,
            OrderStatus.Delivered => 10,
            OrderStatus.ReadyForBilling => 11,
            OrderStatus.Billed => 12,
            OrderStatus.Completed => 13,
            _ => -1
        };
    }
}
