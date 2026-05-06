export interface LoadsQueryParameters {
  page: number;
  pageSize: number;
  search?: string;
  sortBy?: string;
  sortDirection?: string;
  status?: number;
  customerId?: string;
  carrierId?: string;
  mode?: number;
  pickupFrom?: string;
  pickupTo?: string;
  deliveryFrom?: string;
  deliveryTo?: string;
}
