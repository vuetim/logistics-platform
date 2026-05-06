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

  recordSettlementPayment(settlementId: string, amountPaid: number, paymentReference?: string | null, paidAt?: string | null) {
    return this.api.recordSettlementPayment(settlementId, {
      amountPaid,
      paidAt: paidAt ? new Date(paidAt).toISOString() : new Date().toISOString(),
      paymentReference: paymentReference || null
    });
  }
}
