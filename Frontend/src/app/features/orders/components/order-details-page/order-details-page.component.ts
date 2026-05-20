import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { RouterModule, ActivatedRoute, Router } from "@angular/router";
import { OrderDetailsDto } from "../../../../core/models/orders/order-details.dto";
import { OrdersService } from "../../../../data-access/orders/orders.service";
import { PageLayoutComponent } from "../../../../layout/app-shell/page-layout/page-layout/page-layout.component";
import { OrderItemsComponent } from "./tabs/order-items/order-items.component";
import { OrderRoutesComponent } from "./tabs/order-routes/order-routes.component";
import { OrderEquipmentComponent } from "./tabs/order-equipment/order-equipment.component";
import { AuthFacade } from "../../../../core/auth/auth.facade";
import { EditOrderModalComponent } from "./edit-order-modal/edit-order-modal.component";
import { OrderCostsComponent } from "./tabs/order-costs/order-costs.component";
import { OrderNotesComponent } from "./tabs/order-notes/order-notes.component";
import { OrderDocumentsComponent } from "./tabs/order-documents/order-documents.component";
import { OrderExternalIdsComponent } from "./tabs/order-external-ids/order-external-ids.component";
import { enumToOptions } from "../../../../core/utils/enum-options";
import { OrderStatus } from "../../../../core/enums/orders/order-status.enum";
import { OrderPhase } from "../../../../core/enums/orders/order-phase.enum";
import { OrderDirection } from "../../../../core/enums/orders/order-direction.enum";
import { OrderType } from "../../../../core/enums/orders/order-type.enum";
import { ToastrService } from "ngx-toastr";
import { HttpErrorResponse } from "@angular/common/http";
import { CreateLoadFromOrderDto } from "../../../../core/models/orders/create-load-from-order.dto";
import { CreateLoadFromOrderModalComponent } from "./create-load-from-order-modal/create-load-from-order-modal.component";


type TabKey = 'overview' | 'items' | 'routes' | 'equipment' | 'costs' | 'references' | 'notes' | 'documents';

@Component({
  selector: 'app-order-details-page',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    PageLayoutComponent,
    OrderItemsComponent,
    OrderRoutesComponent,
    OrderEquipmentComponent,
    EditOrderModalComponent,
    OrderCostsComponent,
    OrderNotesComponent,
    OrderDocumentsComponent,
    OrderExternalIdsComponent,
    CreateLoadFromOrderModalComponent
  ],
  templateUrl: './order-details-page.component.html',
  styleUrl: './order-details-page.component.css'
})
export class OrderDetailsPageComponent implements OnInit {

  orderId!: string;
  order?: OrderDetailsDto;

  loading = true;
  actionLoading = false;
  tab: TabKey = 'overview';
  private readonly visitedTabs = new Set<TabKey>(['overview']);
  canUpdate = false;
  canCreateFromOrder = false;
  showEditModal = false;
  showCreateLoadModal = false;
  private readonly statusLookup = new Map(enumToOptions(OrderStatus).map(x => [x.value, x.label]));
  private readonly phaseLookup = new Map(enumToOptions(OrderPhase).map(x => [x.value, x.label]));
  private readonly directionLookup = new Map(enumToOptions(OrderDirection).map(x => [x.value, x.label]));
  private readonly typeLookup = new Map(enumToOptions(OrderType).map(x => [x.value, x.label]));

  constructor(
    private route: ActivatedRoute,
    private ordersService: OrdersService,
    public auth: AuthFacade,
    private toastr: ToastrService,
    private router: Router
  ) { }

  ngOnInit() {
    this.orderId = this.route.snapshot.paramMap.get('id')!;
    this.canUpdate = this.auth.hasPermission('Load_Update');
    this.canCreateFromOrder = this.auth.hasPermission('Load_CreateFromOrder');
    this.load();
  }

  load() {
    this.loading = true;

    this.ordersService.getDetails(this.orderId).subscribe({
      next: res => {
        this.order = res;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  setTab(t: TabKey) {
    this.tab = t;
    this.visitedTabs.add(t);
  }

  isTabVisited(t: TabKey): boolean {
    return this.visitedTabs.has(t);
  }

  onChildChanged() {
    this.load();
  }

  openEditOrder() {
    if (!this.canUpdate) return;
    this.showEditModal = true;
  }

  canSubmit(order: OrderDetailsDto): boolean {
    return this.statusKey(order.status) === 'draft';
  }

  canCreateLoad(order: OrderDetailsDto): boolean {
    if (!this.canCreateFromOrder) return false;
    if (order.hasActiveLoad) return false;
    const key = this.statusKey(order.status);
    return key === 'submitted' || key === 'confirmed' || key === 'scheduled';
  }

  canCancel(order: OrderDetailsDto): boolean {
    if (!this.canUpdate) return false;
    const key = this.statusKey(order.status);
    return key !== 'completed' && key !== 'cancelled';
  }

  submitOrder() {
    if (!this.order || !this.canSubmit(this.order)) return;

    this.actionLoading = true;
    this.ordersService.submit(this.order.id).subscribe({
      next: () => {
        this.toastr.success("Order submitted");
        this.load();
        this.actionLoading = false;
      },
      error: () => {
        this.actionLoading = false;
        this.toastr.error("Failed to submit order");
      }
    });
  }

  createLoadFromOrder() {
    if (!this.order || !this.canCreateLoad(this.order)) return;
    this.showCreateLoadModal = true;
  }

  onCreateLoadClose(dto: CreateLoadFromOrderDto | null) {
    this.showCreateLoadModal = false;
    if (!dto || !this.order || !this.canCreateLoad(this.order)) return;

    this.actionLoading = true;
    this.ordersService.createLoadFromOrder(dto).subscribe({
      next: res => {
        this.actionLoading = false;
        this.toastr.success(`Load created: ${res.loadId}`);
        this.router.navigate(['/loads', res.loadId]);
      },
      error: () => {
        this.actionLoading = false;
        this.toastr.error("Failed to create load from order");
      }
    });
  }

  cancelOrder() {
    if (!this.order || !this.canCancel(this.order)) return;
    if (!confirm('Cancel this order?')) return;

    this.actionLoading = true;
    this.ordersService.cancel(this.order.id).subscribe({
      next: () => {
        this.actionLoading = false;
        this.toastr.success("Order cancelled");
        this.load();
      },
      error: (err: HttpErrorResponse) => {
        this.actionLoading = false;
        this.toastr.error(this.extractErrorMessage(err), "Failed to cancel order");
      }
    });
  }

  onEditClose(saved: boolean) {
    this.showEditModal = false;
    if (saved) this.load();
  }

  statusLabel(value: number) {
    return this.statusLookup.get(value) ?? value;
  }

  statusClass(value: number | string) {
    const key = this.statusKey(value);
    if (key === 'draft') return 'status-slate';
    if (key === 'submitted') return 'status-indigo';
    if (key === 'confirmed') return 'status-violet';
    if (key === 'scheduled') return 'status-cyan';
    if (key === 'dispatched') return 'status-blue';
    if (key === 'atpickup') return 'status-amber';
    if (key === 'pickedup') return 'status-lime';
    if (key === 'intransit') return 'status-orange';
    if (key === 'atdelivery') return 'status-teal';
    if (key === 'delivered') return 'status-green';
    if (key === 'readyforbilling') return 'status-yellow';
    if (key === 'billed') return 'status-emerald';
    if (key === 'completed') return 'status-success';
    if (key === 'cancelled') return 'status-danger';
    return 'status-muted';
  }

  phaseLabel(value: number) {
    return this.phaseLookup.get(value) ?? value;
  }

  phaseClass(value: number | string) {
    const key = typeof value === 'string'
      ? value.replace(/\s+/g, '').toLowerCase()
      : String(OrderPhase[value] ?? value).replace(/\s+/g, '').toLowerCase();
    if (key === 'cancelled') return 'status-danger';
    if (key === 'complete') return 'status-success';
    if (key === 'ship') return 'status-blue';
    if (key === 'bill') return 'status-warning';
    if (key === 'open') return 'status-muted';
    return 'status-purple';
  }

  directionLabel(value: number) {
    return this.directionLookup.get(value) ?? value;
  }

  typeLabel(value: number) {
    return this.typeLookup.get(value) ?? value;
  }

  private statusKey(status: unknown): string {
    if (typeof status === 'string') return status.trim().toLowerCase();
    if (typeof status === 'number') {
      const enumName = OrderStatus[status];
      return typeof enumName === 'string'
        ? enumName.toLowerCase()
        : String(status).trim().toLowerCase();
    }
    return '';
  }

  private extractErrorMessage(err: HttpErrorResponse): string {
    if (!err.error) return "Unexpected server error.";
    if (typeof err.error === "string") return err.error;
    return err.error?.message || err.error?.title || "Unexpected server error.";
  }
}
