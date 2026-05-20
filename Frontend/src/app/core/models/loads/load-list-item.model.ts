export interface LoadListItem {
  id: string;
  loadNumber: string;
  customerName: string;
  carrierName?: string | null;
  origin: string;
  destination: string;
  status: number;
  modeType: number;
  pickupDate?: string | null;
  deliveryDate?: string | null;
  pickupStops: number;
  deliveryStops: number;
  customerRate?: number | null;
  carrierRate?: number | null;
  totalBillable: number;
  totalPayable: number;
  margin?: number | null;
  hasEquipment: boolean;
}
