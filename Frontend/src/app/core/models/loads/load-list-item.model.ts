export interface LoadListItem {
  id: string;
  loadNumber: string;
  customerName: string;
  carrierName?: string | null;
  status: number;
  modeType: number;
  pickupDate?: string | null;
  deliveryDate?: string | null;
  customerRate?: number | null;
  carrierRate?: number | null;
  totalBillable: number;
  totalPayable: number;
  margin?: number | null;
  hasEquipment: boolean;
}
