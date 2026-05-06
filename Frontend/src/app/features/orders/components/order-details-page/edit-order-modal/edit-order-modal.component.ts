import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { OrderDetailsDto } from "../../../../../core/models/orders/order-details.dto";
import { UpdateOrderDto } from "../../../../../core/models/orders/update-order.dto";
import { OrdersService } from "../../../../../data-access/orders/orders.service";
import { CarrierListItem } from "../../../../../core/models/carriers/carrier-list-item.model";
import { CarriersService } from "../../../../../data-access/carriers/carriers.service";

@Component({
  selector: 'app-edit-order-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './edit-order-modal.component.html',
  styleUrl: './edit-order-modal.component.css'
})
export class EditOrderModalComponent {
  @Input({ required: true }) order!: OrderDetailsDto;
  @Output() close = new EventEmitter<boolean>();

  loading = false;
  carriers: CarrierListItem[] = [];

  private readonly orderTypeMap: Record<number, string> = {
    1: 'Transportation',
    2: 'Warehouse',
    3: 'Storage',
    4: 'CustomerOrder'
  };

  private readonly directionMap: Record<number, string> = {
    1: 'Inbound',
    2: 'Outbound',
    3: 'Transfer'
  };

  model: Omit<UpdateOrderDto, 'startDate' | 'endDate' | 'plannedPickup' | 'plannedDelivery'> = {
    orderType: 'Transportation',
    direction: 'Inbound',
    dispatchNotes: '',
    deliveryNotes: '',
    preferredCarrierId: null,
    primaryPONumber: '',
    primaryBolNumber: '',
    primaryProNumber: '',
    commodity: '',
    totalWeight: null,
    totalPallets: null,
    totalVolume: null,
    customerRate: null
  };
  startDateInput: string | null = null;
  endDateInput: string | null = null;
  plannedPickupInput: string | null = null;
  plannedDeliveryInput: string | null = null;

  constructor(
    private ordersService: OrdersService,
    private carriersService: CarriersService
  ) { }

  ngOnInit() {
    this.carriersService.getAll().subscribe({
      next: carriers => this.carriers = carriers,
      error: () => this.carriers = []
    });

    this.model = {
      orderType: this.orderTypeMap[this.order.orderType] ?? 'Transportation',
      direction: this.directionMap[this.order.direction] ?? 'Inbound',
      dispatchNotes: this.order.dispatchNotes ?? '',
      deliveryNotes: this.order.deliveryNotes ?? '',
      preferredCarrierId: this.order.preferredCarrierId ?? null,
      primaryPONumber: this.order.primaryPONumber ?? '',
      primaryBolNumber: this.order.primaryBolNumber ?? '',
      primaryProNumber: this.order.primaryProNumber ?? '',
      commodity: this.order.commodity ?? '',
      totalWeight: this.order.totalWeight ?? null,
      totalPallets: this.order.totalPallets ?? null,
      totalVolume: this.order.totalVolume ?? null,
      customerRate: this.order.customerRate ?? null
    };
    this.startDateInput = this.toInputDate(this.order.startDate?.date);
    this.endDateInput = this.toInputDate(this.order.endDate?.date);
    this.plannedPickupInput = this.toInputDate(this.order.plannedPickup?.date);
    this.plannedDeliveryInput = this.toInputDate(this.order.plannedDelivery?.date);
  }

  save() {
    if (!this.startDateInput || !this.endDateInput) return;

    this.loading = true;
    const timezone = Intl.DateTimeFormat().resolvedOptions().timeZone || 'UTC';
    const startDateIso = this.toApiDate(this.startDateInput);
    const endDateIso = this.toApiDate(this.endDateInput);
    const pickupIso = this.toApiDate(this.plannedPickupInput);
    const deliveryIso = this.toApiDate(this.plannedDeliveryInput);

    const dto: UpdateOrderDto = {
      ...this.model,
      startDate: startDateIso ? { date: startDateIso, timezone, hasTime: true } : null,
      endDate: endDateIso ? { date: endDateIso, timezone, hasTime: true } : null,
      startDateType: { key: '33091', value: 'On a specific date' },
      endDateType: { key: '33091', value: 'On a specific date' },
      plannedPickup: pickupIso ? { date: pickupIso, timezone, hasTime: true } : null,
      plannedDelivery: deliveryIso ? { date: deliveryIso, timezone, hasTime: true } : null
    };

    this.ordersService.update(this.order.id, dto).subscribe({
      next: () => this.close.emit(true),
      error: () => this.loading = false
    });
  }

  cancel() {
    this.close.emit(false);
  }

  private toInputDate(value?: string | null) {
    if (!value) return null;
    return value.slice(0, 16);
  }

  private toApiDate(value?: string | null) {
    return value?.trim() ? new Date(value).toISOString() : null;
  }
}
