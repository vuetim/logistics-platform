import { CommonModule } from "@angular/common";
import { Component, Input, OnInit } from "@angular/core";
import { CarrierSettlementDto, CustomerInvoiceDto } from "../../../../../../core/models/loads/load-details.dto";
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

  constructor(private loadsService: LoadsService) {}

  ngOnInit() {
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
}
