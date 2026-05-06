import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, OnChanges, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ToastrService } from "ngx-toastr";
import { ChargeType } from "../../../../../../core/enums/loads/charge-type.enum";
import { LoadCostDto, LoadCostSummaryDto } from "../../../../../../core/models/loads/load-details.dto";
import { LoadsService } from "../../../../../../data-access/loads/loads.service";

@Component({
  selector: 'app-load-costs',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './load-costs.component.html',
  styleUrl: '../load-tab-shared.css'
})
export class LoadCostsComponent implements OnChanges {
  @Input({ required: true }) loadId!: string;
  @Input() summary?: LoadCostSummaryDto | null;
  @Output() changed = new EventEmitter<void>();
  cost?: LoadCostDto;
  loading = false;
  saving = false;

  readonly chargeTypes = Object.keys(ChargeType)
    .filter(k => !isNaN(Number((ChargeType as any)[k])))
    .map(k => ({ label: this.humanize(k), value: (ChargeType as any)[k] as number }));

  constructor(private loadsService: LoadsService, private toastr: ToastrService) {}

  ngOnChanges() {
    if (!this.loadId) return;
    this.loading = true;
    this.loadsService.getCosts(this.loadId).subscribe({
      next: cost => {
        this.cost = {
          ...cost,
          lineItems: (cost.lineItems || []).map(line => ({
            ...line,
            type: this.normalizeType(line.type),
            qty: line.qty ?? 1,
            price: line.price ?? line.amount ?? 0,
            amount: line.amount ?? (line.qty ?? 1) * (line.price ?? 0)
          }))
        };
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  money(value?: number | null) {
    return Number(value ?? 0).toLocaleString(undefined, {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    });
  }

  get billCustomerSubtotal() {
    return this.cost?.lineItems
      ?.filter(line => line.isCustomer)
      .reduce((sum, line) => sum + Number(line.amount || 0), 0) ?? 0;
  }

  get payCarrierSubtotal() {
    return this.cost?.lineItems
      ?.filter(line => line.isCarrier)
      .reduce((sum, line) => sum + Number(line.amount || 0), 0) ?? 0;
  }

  addLine(isCarrier = true) {
    if (!this.cost) {
      this.cost = { notes: null, totalAmount: 0, lineItems: [] };
    }

    this.cost.lineItems.push({
      type: ChargeType.Linehaul,
      qty: 1,
      price: 0,
      amount: 0,
      isCustomer: !isCarrier,
      isCarrier,
      notes: ''
    });
  }

  removeLine(index: number) {
    this.cost?.lineItems.splice(index, 1);
  }

  recalc(line: any) {
    line.amount = Number(line.qty || 0) * Number(line.price || 0);
  }

  save() {
    if (!this.cost) return;
    this.saving = true;
    const dto = {
      notes: this.cost.notes || null,
      lineItems: this.cost.lineItems.map(line => ({
        id: line.id || null,
        type: this.normalizeType(line.type),
        qty: Number(line.qty || 0),
        price: Number(line.price || 0),
        amount: Number(line.amount || 0),
        isCustomer: !!line.isCustomer,
        isCarrier: !!line.isCarrier,
        notes: line.notes || null
      }))
    };

    this.loadsService.updateCosts(this.loadId, dto).subscribe({
      next: () => {
        this.saving = false;
        this.toastr.success("Load costs updated");
        this.changed.emit();
        this.ngOnChanges();
      },
      error: err => {
        this.saving = false;
        this.toastr.error(this.errorMessage(err), "Failed to update costs");
      }
    });
  }

  typeLabel(value: number | string) {
    if (typeof value === 'string') {
      const numeric = (ChargeType as any)[value];
      return typeof numeric === 'number' ? this.humanize(value) : value;
    }
    return this.humanize(ChargeType[value] ?? String(value));
  }

  private normalizeType(value: unknown): ChargeType {
    if (typeof value === 'number') {
      return Object.values(ChargeType).includes(value as ChargeType)
        ? (value as ChargeType)
        : ChargeType.Other;
    }

    if (typeof value === 'string') {
      const trimmed = value.trim();
      const numeric = Number(trimmed);
      if (!Number.isNaN(numeric) && Object.values(ChargeType).includes(numeric as ChargeType)) {
        return numeric as ChargeType;
      }

      const fromName = (ChargeType as Record<string, unknown>)[trimmed];
      if (typeof fromName === 'number' && Object.values(ChargeType).includes(fromName as ChargeType)) {
        return fromName as ChargeType;
      }
    }

    return ChargeType.Other;
  }

  private humanize(value: unknown) {
    return String(value).replace(/([A-Z])/g, ' $1').trim();
  }

  private errorMessage(err: any) {
    if (!err?.error) return "Unexpected server error.";
    if (typeof err.error === 'string') return err.error;
    return err.error.message || err.error.title || "Unexpected server error.";
  }
}
