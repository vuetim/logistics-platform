import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../../core/config/endpoints";
import { CarrierSettlementDto, CustomerInvoiceDto } from "../../core/models/loads/load-details.dto";

@Injectable({ providedIn: 'root' })
export class FinancialsApi {
  constructor(private http: HttpClient) {}

  getInvoices() {
    return this.http.get<CustomerInvoiceDto[]>(API_ENDPOINTS.financialInvoices);
  }

  getSettlements() {
    return this.http.get<CarrierSettlementDto[]>(API_ENDPOINTS.financialSettlements);
  }

  recordInvoicePayment(invoiceId: string, dto: { amountPaid: number; paidAt?: string | null; paymentReference?: string | null }) {
    return this.http.patch<CustomerInvoiceDto>(`${API_ENDPOINTS.financialInvoices}/${invoiceId}/payment`, dto);
  }

  updateInvoiceStatus(invoiceId: string, status: number) {
    return this.http.patch<void>(`${API_ENDPOINTS.financialInvoices}/${invoiceId}/status`, { status });
  }

  recordSettlementPayment(settlementId: string, dto: { amountPaid: number; paidAt?: string | null; paymentReference?: string | null }) {
    return this.http.patch<CarrierSettlementDto>(`${API_ENDPOINTS.financialSettlements}/${settlementId}/payment`, dto);
  }

  updateSettlementStatus(settlementId: string, status: number) {
    return this.http.patch<void>(`${API_ENDPOINTS.financialSettlements}/${settlementId}/status`, { status });
  }
}
