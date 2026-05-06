import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { AuthFacade } from "../../../../../../core/auth/auth.facade";
import { ChargeType } from "../../../../../../core/enums/orders/charge-type.enum";
import { OrderCostLineItemDto } from "../../../../../../core/models/orders/order-costs/order-cost-line-item.model";
import { OrderCostDto } from "../../../../../../core/models/orders/order-costs/order-cost.model";
import { UpdateOrderCostDto } from "../../../../../../core/models/orders/order-costs/update-order-cost.dto";
import { enumToOptions } from "../../../../../../core/utils/enum-options";
import { OrderCostsService } from "../../../../../../data-access/orders/order-costs/order-costs.service";
import { OrderExternalIdsService } from "../../../../../../data-access/orders/order-external-ids/order-external-ids.service";
import { OrdersService } from "../../../../../../data-access/orders/orders.service";
import { UiButtonComponent } from "../../../../../../shared/UI/ui-button/ui-button.component";
import { ToastrService } from "ngx-toastr";
import { HttpErrorResponse } from "@angular/common/http";

@Component({
  selector: 'app-order-costs',
  standalone: true,
  imports: [CommonModule, FormsModule, UiButtonComponent],
  templateUrl: './order-costs.component.html',
  styleUrl: '../order-tab-shared.css'
})
export class OrderCostsComponent implements OnInit {
  @Input({ required: true }) parentId!: string;
  @Output() changed = new EventEmitter<void>();

  canView = false;
  canUpdate = false;
  loading = false;
  chargeTypeOptions = enumToOptions(ChargeType);
  billToOptions: string[] = [];

  cost: OrderCostDto = {
    billTo: '',
    notes: '',
    taxRate: 0,
    baseFreight: 0,
    accessorials: 0,
    quotedTotal: 0,
    subTotal: 0,
    totalTax: 0,
    totalAmount: 0,
    totalBillable: 0,
    totalNonBillable: 0,
    lineItems: []
  };

  constructor(
    private service: OrderCostsService,
    private ordersService: OrdersService,
    private externalIdsService: OrderExternalIdsService,
    private auth: AuthFacade,
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.canView = this.auth.hasPermission('Load_View');
    this.canUpdate = this.auth.hasPermission('Load_Update');
    this.loadBillToOptions();
    this.load();
  }

  private loadBillToOptions() {
    if (!this.parentId) return;

    this.ordersService.getDetails(this.parentId).subscribe({
      next: order => {
        const options = new Set<string>();
        if (order.customerName?.trim()) {
          options.add(order.customerName.trim());
        }
        if (order.preferredCarrierName?.trim()) {
          options.add(order.preferredCarrierName.trim());
        }

        this.externalIdsService.getByOrder(this.parentId).subscribe({
          next: ids => {
            ids
              .map(x => x.relatedParty?.trim())
              .filter((x): x is string => !!x)
              .forEach(x => options.add(x));

            this.billToOptions = Array.from(options);
            if (!this.cost.billTo && this.billToOptions.length) {
              this.cost.billTo = this.billToOptions[0];
            }
          },
          error: () => {
            this.billToOptions = Array.from(options);
            if (!this.cost.billTo && this.billToOptions.length) {
              this.cost.billTo = this.billToOptions[0];
            }
          }
        });
      },
      error: () => {
        this.billToOptions = [];
      }
    });
  }

  load() {
    if (!this.canView || !this.parentId) return;

    this.loading = true;
    this.service.get(this.parentId).subscribe({
      next: res => {
        this.cost = {
          ...res,
          lineItems: res.lineItems.map(li => ({
            ...li,
            type: this.normalizeType(li.type),
            amount: (li.qty ?? 0) * (li.price ?? 0)
          }))
        };
        if (!this.cost.billTo && this.billToOptions.length) {
          this.cost.billTo = this.billToOptions[0];
        }
        this.recalculate();
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  addLine() {
    if (!this.canUpdate) return;

      this.cost.lineItems.push({
        id: null,
      type: ChargeType.Linehaul,
      qty: 1,
      price: 0,
      amount: 0,
      isCustomer: true,
      isCarrier: false,
      notes: ''
    });
    this.recalculate();
  }

  removeLine(index: number) {
    if (!this.canUpdate) return;
    this.cost.lineItems.splice(index, 1);
    this.recalculate();
  }

  recalculate() {
    let totalBillable = 0;
    let totalNonBillable = 0;
    let baseFreight = 0;

    this.cost.lineItems.forEach(li => {
      li.amount = (li.qty ?? 0) * (li.price ?? 0);
      if (li.isCustomer) {
        totalBillable += li.amount;
        if (this.normalizeType(li.type) === ChargeType.Linehaul) {
          baseFreight += li.amount;
        }
      } else {
        totalNonBillable += li.amount;
      }
    });

    const normalizedTaxRate = this.normalizeTaxRate(this.cost.taxRate);
    const totalTax = Number((totalBillable * normalizedTaxRate / 100).toFixed(2));
    this.cost.taxRate = normalizedTaxRate;
    this.cost.baseFreight = baseFreight;
    this.cost.accessorials = totalBillable - baseFreight;
    this.cost.quotedTotal = totalBillable + totalTax;
    this.cost.subTotal = totalBillable;
    this.cost.totalTax = totalTax;
    this.cost.totalBillable = totalBillable;
    this.cost.totalNonBillable = totalNonBillable;
    this.cost.totalAmount = totalBillable + totalNonBillable + totalTax;
  }

  save() {
    if (!this.canUpdate) return;

    const dto: UpdateOrderCostDto = {
      billTo: this.cost.billTo ?? '',
      notes: this.cost.notes ?? '',
      taxRate: this.normalizeTaxRate(this.cost.taxRate),
      lineItems: this.cost.lineItems.map((li: OrderCostLineItemDto) => ({
        id: li.id ?? null,
        type: this.normalizeType(li.type),
        qty: Number(li.qty ?? 0),
        price: Number(li.price ?? 0),
        amount: Number(li.amount ?? 0),
        isCustomer: !!li.isCustomer,
        isCarrier: false,
        notes: li.notes ?? ''
      }))
    };

    this.loading = true;
    this.service.update(this.parentId, dto).subscribe({
      next: () => {
        this.changed.emit();
        this.load();
      },
      error: (err: HttpErrorResponse) => {
        const message = this.extractError(err);
        this.toastr.error(message, "Failed to save costs");
        this.loading = false;
      }
    });
  }

  private extractError(err: HttpErrorResponse): string {
    if (!err.error) return "Unexpected server error.";
    if (typeof err.error === "string") return err.error;
    return err.error?.message || err.error?.title || "Unexpected server error.";
  }

  private normalizeType(value: unknown): ChargeType {
    if (typeof value === "number") {
      return Object.values(ChargeType).includes(value as ChargeType)
        ? (value as ChargeType)
        : ChargeType.Linehaul;
    }

    if (typeof value === "string") {
      const trimmed = value.trim();
      const numeric = Number(trimmed);
      if (!Number.isNaN(numeric) && Object.values(ChargeType).includes(numeric as ChargeType)) {
        return numeric as ChargeType;
      }

      const fromName = (ChargeType as Record<string, unknown>)[trimmed];
      if (typeof fromName === "number" && Object.values(ChargeType).includes(fromName as ChargeType)) {
        return fromName as ChargeType;
      }
    }

    return ChargeType.Linehaul;
  }

  private normalizeTaxRate(value: number | null | undefined): number {
    const n = Number(value ?? 0);
    if (!Number.isFinite(n) || n < 0) return 0;
    if (n > 100) return 100;
    return Number(n.toFixed(4));
  }
}
