-- Deletes operational order/load/billing data while keeping master data such as
-- carriers, customers, users, roles, permissions, and addresses.
-- Run only on a local/dev database after backup.

BEGIN TRANSACTION;

IF OBJECT_ID(N'[CarrierStopPerformances]', N'U') IS NOT NULL DELETE FROM [CarrierStopPerformances];
IF OBJECT_ID(N'[DelayResponsibilities]', N'U') IS NOT NULL DELETE FROM [DelayResponsibilities];
IF OBJECT_ID(N'[LoadDelayResponsibilities]', N'U') IS NOT NULL DELETE FROM [LoadDelayResponsibilities];
IF OBJECT_ID(N'[LoadAlerts]', N'U') IS NOT NULL DELETE FROM [LoadAlerts];

IF OBJECT_ID(N'[CustomerInvoiceLineItems]', N'U') IS NOT NULL DELETE FROM [CustomerInvoiceLineItems];
IF OBJECT_ID(N'[CustomerInvoices]', N'U') IS NOT NULL DELETE FROM [CustomerInvoices];
IF OBJECT_ID(N'[CarrierSettlementLineItems]', N'U') IS NOT NULL DELETE FROM [CarrierSettlementLineItems];
IF OBJECT_ID(N'[CarrierSettlements]', N'U') IS NOT NULL DELETE FROM [CarrierSettlements];

IF OBJECT_ID(N'[LoadCostLineItems]', N'U') IS NOT NULL DELETE FROM [LoadCostLineItems];
IF OBJECT_ID(N'[LoadCosts]', N'U') IS NOT NULL DELETE FROM [LoadCosts];
IF OBJECT_ID(N'[LoadDocuments]', N'U') IS NOT NULL DELETE FROM [LoadDocuments];
IF OBJECT_ID(N'[LoadNotes]', N'U') IS NOT NULL DELETE FROM [LoadNotes];
IF OBJECT_ID(N'[LoadEquipment]', N'U') IS NOT NULL DELETE FROM [LoadEquipment];
IF OBJECT_ID(N'[LoadItems]', N'U') IS NOT NULL DELETE FROM [LoadItems];
IF OBJECT_ID(N'[LoadCarrierAssignments]', N'U') IS NOT NULL DELETE FROM [LoadCarrierAssignments];
IF OBJECT_ID(N'[LoadStops]', N'U') IS NOT NULL DELETE FROM [LoadStops];
IF OBJECT_ID(N'[LoadOrders]', N'U') IS NOT NULL DELETE FROM [LoadOrders];
IF OBJECT_ID(N'[Loads]', N'U') IS NOT NULL DELETE FROM [Loads];

IF OBJECT_ID(N'[OrderCostLineItems]', N'U') IS NOT NULL DELETE FROM [OrderCostLineItems];
IF OBJECT_ID(N'[OrderCosts]', N'U') IS NOT NULL DELETE FROM [OrderCosts];
IF OBJECT_ID(N'[OrderDocuments]', N'U') IS NOT NULL DELETE FROM [OrderDocuments];
IF OBJECT_ID(N'[OrderExternalIds]', N'U') IS NOT NULL DELETE FROM [OrderExternalIds];
IF OBJECT_ID(N'[OrderNotes]', N'U') IS NOT NULL DELETE FROM [OrderNotes];
IF OBJECT_ID(N'[OrderEquipmentRequirements]', N'U') IS NOT NULL DELETE FROM [OrderEquipmentRequirements];
IF OBJECT_ID(N'[OrderItems]', N'U') IS NOT NULL DELETE FROM [OrderItems];
IF OBJECT_ID(N'[OrderRoutes]', N'U') IS NOT NULL DELETE FROM [OrderRoutes];
IF OBJECT_ID(N'[Orders]', N'U') IS NOT NULL DELETE FROM [Orders];

COMMIT TRANSACTION;
