import { Injectable } from "@angular/core";
import { FinancialsApi } from "./financials.api";

@Injectable({ providedIn: 'root' })
export class FinancialsService {
  constructor(private api: FinancialsApi) {}

  getInvoices() {
    return this.api.getInvoices();
  }

  getSettlements() {
    return this.api.getSettlements();
  }

  recordInvoicePayment(invoiceId: string, amountPaid: number, paymentReference?: string | null, paidAt?: string | null) {
    return this.api.recordInvoicePayment(invoiceId, {
      amountPaid,
      paidAt: paidAt ? new Date(paidAt).toISOString() : new Date().toISOString(),
      paymentReference: paymentReference || null
    });
  }

  updateInvoiceStatus(invoiceId: string, status: number) {
    return this.api.updateInvoiceStatus(invoiceId, status);
  }

  recordSettlementPayment(settlementId: string, amountPaid: number, paymentReference?: string | null, paidAt?: string | null) {
    return this.api.recordSettlementPayment(settlementId, {
      amountPaid,
      paidAt: paidAt ? new Date(paidAt).toISOString() : new Date().toISOString(),
      paymentReference: paymentReference || null
    });
  }

  updateSettlementStatus(settlementId: string, status: number) {
    return this.api.updateSettlementStatus(settlementId, status);
  }
}
