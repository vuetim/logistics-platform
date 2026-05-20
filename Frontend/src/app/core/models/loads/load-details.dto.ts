import { OrderDirection } from "../../enums/orders/order-direction.enum";
import { OrderType } from "../../enums/orders/order-type.enum";
import { StopType } from "../../enums/orders/stop-type.enum";

export interface LoadDetailsDto {
  execution: LoadExecutionDetailsDto;
  orderSnapshot?: LoadOrderSnapshotDto | null;
  items: LoadItemDto[];
  summary?: LoadSummaryDto | null;
  costSummary?: LoadCostSummaryDto | null;
  equipment: LoadEquipmentDto[];
  hasEquipment: boolean;
}

export interface LoadExecutionDetailsDto {
  id: string;
  loadNumber: string;
  status: number;
  mode: number;
  customerId: string;
  customerName: string;
  carrierId?: string | null;
  carrierName?: string | null;
  origin: string;
  destination: string;
  plannedPickupDate?: string | null;
  plannedDeliveryDate?: string | null;
  actualPickupDate?: string | null;
  actualDeliveryDate?: string | null;
  customerRate?: number | null;
  carrierRate?: number | null;
  margin?: number | null;
  accessorials?: number | null;
  bolNumber?: string | null;
  proNumber?: string | null;
  rateConfirmationNumber?: string | null;
  trackingNumber?: string | null;
  distanceMiles?: number | null;
  durationMinutes?: number | null;
  encodedPolyline?: string | null;
  lastKnownLatitude?: number | null;
  lastKnownLongitude?: number | null;
  lastKnownLocationAt?: string | null;
  trackingProvider?: string | null;
  trackingExternalId?: string | null;
  driverName?: string | null;
  driverPhone?: string | null;
  driverEmail?: string | null;
  truckNumber?: string | null;
  trailerNumber?: string | null;
  carrierSCAC?: string | null;
  podReceivedAt?: string | null;
  podUploadedBy?: string | null;
  stops: LoadStopDetailsDto[];
  items: LoadItemDto[];
}

export interface LoadOrderSnapshotDto {
  orderId: string;
  orderNumber: string;
  primaryPONumber?: string | null;
  primaryBolNumber?: string | null;
  primaryProNumber?: string | null;
  commodity?: string | null;
  orderType: OrderType;
  direction: OrderDirection;
  plannedPickupDate?: string | null;
  plannedDeliveryDate?: string | null;
  routes: LoadOrderRouteSnapshotDto[];
}

export interface LoadOrderRouteSnapshotDto {
  id: string;
  sequence: number;
  stopType: StopType;
  locationName: string;
  city: string;
  state: string;
  country: string;
  plannedArrivalFrom?: string | null;
  plannedArrivalTo?: string | null;
  hasTime: boolean;
  copyToLoad: boolean;
  timeZone?: string | null;
  poNumbers?: string | null;
  notes?: string | null;
  isActive: boolean;
}

export interface LoadStopDetailsDto {
  id: string;
  sequence: number;
  stopType: StopType;
  status: number;
  locationName: string;
  addressLine1: string;
  addressLine2?: string | null;
  city: string;
  state: string;
  postalCode: string;
  country: string;
  latitude?: number | null;
  longitude?: number | null;
  plannedArrivalFrom?: string | null;
  plannedArrivalTo?: string | null;
  appointmentType: number;
  flexMinutes?: number | null;
  timeZone?: string | null;
  appointmentStatus?: number | string | null;
  appointmentConfirmed?: boolean | null;
  appointmentConfirmationNumber?: string | null;
  appointmentNumber?: string | null;
  stopReference?: string | null;
  poNumbers?: string | null;
  revisedArrivalFrom?: string | null;
  revisedArrivalTo?: string | null;
  actualArrival?: string | null;
  actualDeparture?: string | null;
  isOnTime?: boolean | null;
  minutesLate?: number | null;
  notes?: string | null;
}

export interface LoadItemDto {
  id: string;
  name: string;
  customerReference?: string | null;
  quantity: number;
  quantityUnit: string;
  handlingQuantity?: number | null;
  handlingUnit?: string | null;
  unitNetWeight?: number | null;
  unitGrossWeight?: number | null;
  weightUnit?: string | null;
  length?: number | null;
  width?: number | null;
  height?: number | null;
  dimensionUnit?: string | null;
  isHazmat: boolean;
  freightClass?: string | null;
  hazardClass?: string | null;
  identificationNumber?: string | null;
  volumeUnit?: string | null;
  volume?: number | null;
  minTemperature?: number | null;
  maxTemperature?: number | null;
  temperatureUnit?: string | null;
  declaredValue?: number | null;
  currency?: string | null;
  stackable?: boolean | null;
  notes?: string | null;
}

export interface LoadEquipmentDto {
  id: string;
  quantity?: number | null;
  equipmentType: number;
  length?: number | null;
  weight?: number | null;
  weightUnit: number;
  minTemp?: number | null;
  maxTemp?: number | null;
  tempUnit: number;
}

export interface LoadSummaryDto {
  totalWeight: number;
  totalVolume: number;
  totalPallets: number;
  totalItems: number;
  totalStops: number;
  pickupStops: number;
  deliveryStops: number;
  pickupLocations: string[];
  deliveryLocations: string[];
}

export interface LoadCostSummaryDto {
  customerRate: number;
  carrierRate: number;
  margin: number;
  totalBillable: number;
  totalPayable: number;
}

export interface LoadCostDto {
  notes?: string | null;
  totalAmount: number;
  lineItems: LoadCostLineItemDto[];
}

export interface LoadCostLineItemDto {
  id?: string;
  type: number | string;
  qty: number;
  price: number;
  amount: number;
  isCustomer: boolean;
  isCarrier: boolean;
  notes?: string | null;
}

export interface LoadCarrierAssignmentDto {
  id: string;
  loadId: string;
  carrierId: string;
  carrierName: string;
  offeredRate?: number | null;
  currency?: string | null;
  rateConfirmationNumber?: string | null;
  status: number | string;
  tenderMethod?: string | null;
  tenderNotes?: string | null;
  tenderExpiresAt?: string | null;
  acceptedByName?: string | null;
  acceptedByEmail?: string | null;
  rejectedReason?: string | null;
  tenderedAt: string;
  acceptedAt?: string | null;
  rejectedAt?: string | null;
  isActive: boolean;
}

export interface OpenCarrierOfferDto {
  assignmentId: string;
  loadId: string;
  loadNumber: string;
  loadStatus: string;
  customerName: string;
  origin: string;
  destination: string;
  carrierId: string;
  carrierName: string;
  offeredRate?: number | null;
  currency?: string | null;
  rateConfirmationNumber?: string | null;
  tenderMethod?: string | null;
  tenderNotes?: string | null;
  tenderedAt: string;
  tenderExpiresAt?: string | null;
}

export interface LoadExceptionDto {
  id: string;
  loadId: string;
  loadStopId?: string | null;
  orderId?: string | null;
  exceptionKey: string;
  exceptionValue: string;
  reasonKey?: string | null;
  reasonValue?: string | null;
  ediReasonCode?: string | null;
  responsiblePartyKey?: string | null;
  responsiblePartyValue?: string | null;
  status: number | string;
  description?: string | null;
  resolutionNotes?: string | null;
  affectedItemName?: string | null;
  affectedItemReference?: string | null;
  quantity?: number | null;
  unit?: string | null;
  occurredAt: string;
  resolvedAt?: string | null;
}

export interface LoadStopServiceDto {
  id: string;
  loadStopId: string;
  serviceKey: string;
  serviceValue: string;
  notes?: string | null;
  isPickupService: boolean;
  isDeliveryService: boolean;
}

export interface LoadNoteDto {
  id: string;
  message?: string | null;
  body?: string | null;
  text?: string | null;
  note?: string | null;
  isInternal: boolean;
  createdAt?: string | null;
  createdByName?: string | null;
}

export interface LoadDocumentDto {
  id: string;
  documentType: number;
  fileUrl: string;
  isInternal: boolean;
  uploadedAt?: string | null;
  uploadedByName?: string | null;
}

export interface LoadActivityDto {
  id?: string;
  action?: string | null;
  field?: string | null;
  oldValue?: string | null;
  newValue?: string | null;
  details?: string | null;
  performedBy?: string | null;
  createdAt: string;
  activityType?: string | number | null;
  description?: string | null;
  message?: string | null;
  userName?: string | null;
}

export interface CustomerInvoiceDto {
  id: string;
  loadId: string;
  customerId: string;
  invoiceNumber: string;
  invoiceDate: string;
  dueDate?: string | null;
  invoiceType: number | string;
  status: number | string;
  totalAmount: number;
  amountPaid?: number;
  balanceDue?: number;
  paidAt?: string | null;
  paymentReference?: string | null;
  notes?: string | null;
  lineItems: InvoiceLineItemDto[];
  pdfUrl?: string | null;
}

export interface InvoiceLineItemDto {
  description?: string | null;
  type?: number | string;
  qty?: number;
  price?: number;
  amount: number;
  notes?: string | null;
}

export interface CarrierSettlementDto {
  id: string;
  loadId: string;
  carrierId: string;
  settlementNumber: string;
  settlementDate: string;
  dueDate?: string | null;
  status: number | string;
  totalAmount: number;
  amountPaid?: number;
  balanceDue?: number;
  paidAt?: string | null;
  paymentReference?: string | null;
  notes?: string | null;
  lineItems: CarrierSettlementLineItemDto[];
  pdfUrl?: string | null;
}

export interface CarrierSettlementLineItemDto {
  description?: string | null;
  type?: number | string;
  qty?: number;
  price?: number;
  amount: number;
  notes?: string | null;
}
