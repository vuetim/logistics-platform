export interface CreateLoadFromOrderDto {
  orderId: string;
  carrierId?: string | null;
  plannedPickupDate?: string | null;
  plannedDeliveryDate?: string | null;
  carrierRate?: number | null;
  rateConfirmationNumber?: string | null;
  splitOrder: boolean;
}
