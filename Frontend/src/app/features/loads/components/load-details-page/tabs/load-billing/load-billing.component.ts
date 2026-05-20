import { CommonModule } from "@angular/common";
import { Component, Input, OnInit } from "@angular/core";
import { AuthFacade } from "../../../../../../core/auth/auth.facade";
import { Permission } from "../../../../../../core/auth/permissions/permission.enum";
import { CarrierSettlementDto, CustomerInvoiceDto } from "../../../../../../core/models/loads/load-details.dto";
import { FinancialsService } from "../../../../../../data-access/financials/financials.service";
import { LoadsService } from "../../../../../../data-access/loads/loads.service";

@Component({
  selector: 'app-load-billing',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './load-billing.component.html',
  styleUrl: '../load-tab-shared.css'
})
export class LoadBillingComponent implements OnInit {
  @Input({ required: true }) loadId!: string;
  invoice?: CustomerInvoiceDto;
  settlement?: CarrierSettlementDto;
  invoiceError = '';
  settlementError = '';

  constructor(
    private loadsService: LoadsService,
    private financialsService: FinancialsService,
    private auth: AuthFacade
  ) {}

  ngOnInit() {
    this.loadFinancials();
  }

  loadFinancials() {
    this.loadsService.getInvoice(this.loadId).subscribe({
      next: invoice => this.invoice = invoice,
      error: err => this.invoiceError = this.errorMessage(err)
    });

    this.loadsService.getSettlement(this.loadId).subscribe({
      next: settlement => this.settlement = settlement,
      error: err => this.settlementError = this.errorMessage(err)
    });
  }

  invoicePdfUrl() {
    return this.invoice ? this.loadsService.invoicePdfUrl(this.loadId, this.invoice.id) : '';
  }

  settlementPdfUrl() {
    return this.settlement ? this.loadsService.settlementPdfUrl(this.loadId, this.settlement.id) : '';
  }

  downloadInvoicePdf() {
    if (!this.invoice) return;

    this.loadsService.downloadInvoicePdf(this.loadId, this.invoice.id).subscribe({
      next: blob => this.downloadBlob(blob, `invoice-${this.invoice?.invoiceNumber ?? this.invoice?.id}.pdf`),
      error: err => this.invoiceError = this.errorMessage(err)
    });
  }

  downloadSettlementPdf() {
    if (!this.settlement) return;

    this.loadsService.downloadSettlementPdf(this.loadId, this.settlement.id).subscribe({
      next: blob => this.downloadBlob(blob, `settlement-${this.settlement?.settlementNumber ?? this.settlement?.id}.pdf`),
      error: err => this.settlementError = this.errorMessage(err)
    });
  }

  markInvoiceSent() {
    if (!this.invoice) return;
    this.financialsService.updateInvoiceStatus(this.invoice.id, 1).subscribe({
      next: () => this.loadFinancials(),
      error: err => this.invoiceError = this.errorMessage(err)
    });
  }

  cancelInvoice() {
    if (!this.invoice) return;
    this.financialsService.updateInvoiceStatus(this.invoice.id, 4).subscribe({
      next: () => this.loadFinancials(),
      error: err => this.invoiceError = this.errorMessage(err)
    });
  }

  recordInvoicePayment() {
    if (!this.invoice) return;
    const balance = Number(this.invoice.balanceDue ?? this.invoice.totalAmount ?? 0);
    this.financialsService.recordInvoicePayment(this.invoice.id, balance).subscribe({
      next: () => this.loadFinancials(),
      error: err => this.invoiceError = this.errorMessage(err)
    });
  }

  markSettlementSent() {
    if (!this.settlement) return;
    this.financialsService.updateSettlementStatus(this.settlement.id, 1).subscribe({
      next: () => this.loadFinancials(),
      error: err => this.settlementError = this.errorMessage(err)
    });
  }

  cancelSettlement() {
    if (!this.settlement) return;
    this.financialsService.updateSettlementStatus(this.settlement.id, 4).subscribe({
      next: () => this.loadFinancials(),
      error: err => this.settlementError = this.errorMessage(err)
    });
  }

  recordSettlementPayment() {
    if (!this.settlement) return;
    const balance = Number(this.settlement.balanceDue ?? this.settlement.totalAmount ?? 0);
    this.financialsService.recordSettlementPayment(this.settlement.id, balance).subscribe({
      next: () => this.loadFinancials(),
      error: err => this.settlementError = this.errorMessage(err)
    });
  }

  canMarkSent(status: number | string) {
    return this.canUpdateFinancialStatus('invoice') && this.statusValue(status) === 0;
  }

  canCancel(status: number | string, kind: 'invoice' | 'settlement' = 'invoice') {
    if (!this.canUpdateFinancialStatus(kind)) return false;
    const value = this.statusValue(status);
    return value !== 2 && value !== 4;
  }

  canRecordPayment(status: number | string, balance?: number | null, kind: 'invoice' | 'settlement' = 'invoice') {
    if (!this.canRecordFinancialPayment(kind)) return false;
    const value = this.statusValue(status);
    return value !== 2 && value !== 4 && Number(balance ?? 0) > 0;
  }

  canMarkInvoiceSent(status: number | string) {
    return this.canUpdateFinancialStatus('invoice') && this.statusValue(status) === 0;
  }

  canMarkSettlementSent(status: number | string) {
    return this.canUpdateFinancialStatus('settlement') && this.statusValue(status) === 0;
  }

  money(value?: number | null) {
    return Number(value ?? 0).toLocaleString(undefined, {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    });
  }

  private errorMessage(err: any) {
    if (!err?.error) return "Unavailable";
    if (typeof err.error === 'string') return err.error;
    return err.error.message || err.error.title || "Unavailable";
  }

  private downloadBlob(blob: Blob, fileName: string) {
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = fileName;
    link.click();
    URL.revokeObjectURL(url);
  }

  private statusValue(status: number | string) {
    if (typeof status === 'number') return status;
    const numeric = Number(status);
    if (!Number.isNaN(numeric)) return numeric;
    const normalized = status.replace(/\s+/g, '').toLowerCase();
    if (normalized === 'draft') return 0;
    if (normalized === 'sent') return 1;
    if (normalized === 'paid') return 2;
    if (normalized === 'overdue' || normalized === 'disputed') return 3;
    if (normalized === 'canceled' || normalized === 'cancelled') return 4;
    return -1;
  }

  private canUpdateFinancialStatus(kind: 'invoice' | 'settlement') {
    if (this.auth.hasRole('Admin')) return true;
    return kind === 'invoice'
      ? this.auth.hasPermission(Permission.Financial_Invoice_UpdateStatus)
      : this.auth.hasPermission(Permission.Financial_Settlement_UpdateStatus);
  }

  private canRecordFinancialPayment(kind: 'invoice' | 'settlement') {
    if (this.auth.hasRole('Admin')) return true;
    return kind === 'invoice'
      ? this.auth.hasPermission(Permission.Financial_Invoice_RecordPayment)
      : this.auth.hasPermission(Permission.Financial_Settlement_RecordPayment);
  }
}
