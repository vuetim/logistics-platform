import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { RouterLink } from "@angular/router";
import { forkJoin, Observable } from "rxjs";
import { FinancialsService } from "../../data-access/financials/financials.service";
import { CarrierSettlementDto, CustomerInvoiceDto } from "../../core/models/loads/load-details.dto";

@Component({
  selector: 'app-financials-page',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './financials-page.component.html',
  styleUrl: './financials-page.component.css'
})
export class FinancialsPageComponent implements OnInit {
  invoices: CustomerInvoiceDto[] = [];
  settlements: CarrierSettlementDto[] = [];
  tab: 'invoices' | 'settlements' = 'invoices';
  loading = false;
  error = '';
  paymentModal?: {
    kind: 'invoice' | 'settlement';
    id: string;
    number: string;
    total: number;
    balance: number;
    amountPaid: number;
    paidAt: string;
    paymentReference: string;
  };

  constructor(private financialsService: FinancialsService) { }

  ngOnInit() {
    this.load();
  }

  load() {
    this.loading = true;
    forkJoin({
      invoices: this.financialsService.getInvoices(),
      settlements: this.financialsService.getSettlements()
    }).subscribe({
      next: result => {
        this.invoices = result.invoices || [];
        this.settlements = result.settlements || [];
        this.loading = false;
      },
      error: err => {
        this.error = this.errorMessage(err);
        this.loading = false;
      }
    });
  }

  money(value?: number | null) {
    return Number(value ?? 0).toLocaleString(undefined, {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    });
  }

  totalInvoices() {
    return this.invoices.reduce((sum, invoice) => sum + Number(invoice.totalAmount ?? 0), 0);
  }

  totalSettlements() {
    return this.settlements.reduce((sum, settlement) => sum + Number(settlement.totalAmount ?? 0), 0);
  }

  margin() {
    return this.totalInvoices() - this.totalSettlements();
  }

  recordInvoicePayment(invoice: CustomerInvoiceDto) {
    this.paymentModal = {
      kind: 'invoice',
      id: invoice.id,
      number: invoice.invoiceNumber,
      total: Number(invoice.totalAmount ?? 0),
      balance: Number(invoice.balanceDue ?? invoice.totalAmount ?? 0),
      amountPaid: Number(invoice.balanceDue ?? invoice.totalAmount ?? 0),
      paidAt: this.todayInputValue(),
      paymentReference: invoice.paymentReference || ''
    };
  }

  recordSettlementPayment(settlement: CarrierSettlementDto) {
    this.paymentModal = {
      kind: 'settlement',
      id: settlement.id,
      number: settlement.settlementNumber,
      total: Number(settlement.totalAmount ?? 0),
      balance: Number(settlement.balanceDue ?? settlement.totalAmount ?? 0),
      amountPaid: Number(settlement.balanceDue ?? settlement.totalAmount ?? 0),
      paidAt: this.todayInputValue(),
      paymentReference: settlement.paymentReference || ''
    };
  }

  closePaymentModal() {
    this.paymentModal = undefined;
  }

  savePayment() {
    if (!this.paymentModal) return;

    const amount = Number(this.paymentModal.amountPaid ?? 0);
    if (!Number.isFinite(amount) || amount < 0) {
      this.error = 'Payment amount must be zero or greater.';
      return;
    }

    const request: Observable<any> =
      this.paymentModal.kind === 'invoice'
        ? this.financialsService.recordInvoicePayment(
          this.paymentModal.id,
          amount,
          this.paymentModal.paymentReference,
          this.paymentModal.paidAt
        )
        : this.financialsService.recordSettlementPayment(
          this.paymentModal.id,
          amount,
          this.paymentModal.paymentReference,
          this.paymentModal.paidAt
        );

    request.subscribe({
      next: () => {
        this.paymentModal = undefined;
        this.load();
      },
      error: err => {
        this.error = this.errorMessage(err);
        this.loading = false;
      }
    });
  }

  label(value: unknown) {
    return String(value).replace(/([A-Z])/g, ' $1').trim();
  }

  invoiceStatusLabel(invoice: CustomerInvoiceDto) {
    if (this.isOverdue(invoice.dueDate, invoice.status)) return 'Overdue';
    return this.financialStatusLabel(invoice.status, ['Draft', 'Sent', 'Paid', 'Overdue', 'Canceled']);
  }

  settlementStatusLabel(settlement: CarrierSettlementDto) {
    return this.financialStatusLabel(settlement.status, ['Draft', 'Sent', 'Paid', 'Disputed', 'Canceled']);
  }

  invoiceStatusClass(invoice: CustomerInvoiceDto) {
    return this.statusClass(this.invoiceStatusLabel(invoice));
  }

  settlementStatusClass(settlement: CarrierSettlementDto) {
    return this.statusClass(this.settlementStatusLabel(settlement));
  }

  private financialStatusLabel(value: unknown, labels: string[]) {
    if (typeof value === 'number') return labels[value] ?? String(value);
    const numeric = Number(value);
    if (!Number.isNaN(numeric) && String(value).trim() !== '') return labels[numeric] ?? String(value);
    return this.label(value);
  }

  private isOverdue(dueDate?: string | null, status?: number | string) {
    const statusLabel = this.financialStatusLabel(status, ['Draft', 'Sent', 'Paid', 'Overdue', 'Canceled'])
      .replace(/\s+/g, '')
      .toLowerCase();
    if (!dueDate || statusLabel === 'paid' || statusLabel === 'canceled') return false;
    const due = new Date(dueDate);
    if (Number.isNaN(due.getTime())) return false;
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    due.setHours(0, 0, 0, 0);
    return due < today;
  }

  private statusClass(value: unknown) {
    const key = String(value).replace(/\s+/g, '').toLowerCase();
    if (key === 'draft') return 'badge-slate';
    if (key === 'sent') return 'badge-blue';
    if (key === 'paid') return 'badge-success';
    if (key === 'disputed') return 'badge-rose';
    if (key === 'void' || key === 'voided' || key === 'cancelled' || key === 'canceled') return 'badge-danger';
    if (key === 'overdue') return 'badge-orange';
    return 'badge-indigo';
  }

  private todayInputValue() {
    return new Date().toISOString().slice(0, 10);
  }

  private errorMessage(err: any) {
    if (!err?.error) return "Financials unavailable";
    if (typeof err.error === 'string') return err.error;
    return err.error.message || err.error.title || "Financials unavailable";
  }
}
