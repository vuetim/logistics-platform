import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderListItem } from '../../../../core/models/orders/order-list-item.model';
import { OrdersService } from '../../../../data-access/orders/orders.service';
import { GenericListPage } from '../../../../shared/list/generic-list-page';
import { TableAction, TableColumn } from '../../../../shared/UI/entity-table/entity-table.models';
import { UiCardComponent } from '../../../../shared/UI/ui-card/ui-card.component';
import { UiButtonComponent } from '../../../../shared/UI/ui-button/ui-button.component';
import { EntityTableComponent } from '../../../../shared/UI/entity-table/entity-table.component';
import { PaginationComponent } from '../../../../shared/UI/pagination/pagination.component';
import { PageLayoutComponent } from '../../../../layout/app-shell/page-layout/page-layout/page-layout.component';
import { OrdersQueryParameters } from '../../../../core/models/orders/orders-query-parameters.dto';
import { KeyValuePipe, NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthFacade } from '../../../../core/auth/auth.facade';
import { OrderCreateModalComponent } from '../order-create-modal/order-create-modal.component';
import { OrderDirection } from '../../../../core/enums/orders/order-direction.enum';
import { OrderStatus } from '../../../../core/enums/orders/order-status.enum';
import { OrderPhase } from '../../../../core/enums/orders/order-phase.enum';
import { TablePreferencesService } from '../../../../data-access/ui/table-preferences.service';
import { CarriersService } from '../../../../data-access/carriers/carriers.service';
import { CarrierListItem } from '../../../../core/models/carriers/carrier-list-item.model';

@Component({
  selector: 'app-orders-page',
  standalone: true,
  imports: [NgIf, UiCardComponent, UiButtonComponent, EntityTableComponent, PaginationComponent, PageLayoutComponent, OrderCreateModalComponent, NgFor, FormsModule, KeyValuePipe],
  templateUrl: './orders-page.component.html',
  styleUrl: './orders-page.component.css'
})
export class OrdersPageComponent
  extends GenericListPage<OrdersQueryParameters>
  implements OnInit {

  orders: OrderListItem[] = [];
  creatingOrder = false;
  canCreate = false;
  showColumnMenu = false;
  private tablePreferencesAvailable = true;
  private readonly tableKey = 'orders';
  private readonly columnStorageKey = 'orders.table.columns.v1';
  private readonly mandatoryColumnKeys = new Set<string>(['orderNumber']);
  private columnVisibility = new Map<string, boolean>();
  private columnOrder: string[] = [];
  private snapshotColumnVisibility = new Map<string, boolean>();
  private snapshotColumnOrder: string[] = [];
  filterModel = {
    search: '',
    status: '',
    phase: '',
    direction: '',
    preferredCarrierId: '',
    fromDate: '',
    toDate: ''
  };
  carriers: CarrierListItem[] = [];
  readonly statusOptions = [
    { label: 'Draft', value: String(OrderStatus.Draft) },
    { label: 'Submitted', value: String(OrderStatus.Submitted) },
    { label: 'Confirmed', value: String(OrderStatus.Confirmed) },
    { label: 'Scheduled', value: String(OrderStatus.Scheduled) },
    { label: 'Dispatched', value: String(OrderStatus.Dispatched) },
    { label: 'At Pickup', value: String(OrderStatus.AtPickup) },
    { label: 'Picked Up', value: String(OrderStatus.PickedUp) },
    { label: 'In Transit', value: String(OrderStatus.InTransit) },
    { label: 'At Delivery', value: String(OrderStatus.AtDelivery) },
    { label: 'Delivered', value: String(OrderStatus.Delivered) },
    { label: 'Ready For Billing', value: String(OrderStatus.ReadyForBilling) },
    { label: 'Billed', value: String(OrderStatus.Billed) },
    { label: 'Completed', value: String(OrderStatus.Completed) },
    { label: 'Cancelled', value: String(OrderStatus.Cancelled) }
  ];
  readonly phaseOptions = [
    { label: 'Open', value: String(OrderPhase.Open) },
    { label: 'Plan', value: String(OrderPhase.Plan) },
    { label: 'Ship', value: String(OrderPhase.Ship) },
    { label: 'Bill', value: String(OrderPhase.Bill) },
    { label: 'Complete', value: String(OrderPhase.Complete) },
    { label: 'Cancelled', value: String(OrderPhase.Cancelled) }
  ];
  readonly directionOptions = [
    { label: 'Inbound', value: String(OrderDirection.Inbound) },
    { label: 'Outbound', value: String(OrderDirection.Outbound) },
    { label: 'Transfer', value: String(OrderDirection.Transfer) }
  ];

  private readonly allColumns: TableColumn<OrderListItem>[] = [
    { key: 'orderNumber', label: 'Order #', sortable: true },
    { key: 'customerName', label: 'Customer', sortable: true },
    {
      key: 'preferredCarrierName',
      label: 'Preferred Carrier',
      formatter: o => o.preferredCarrierName || '-'
    },
    {
      key: 'direction',
      label: 'Direction',
      sortable: true,
      formatter: o => this.directionLabel(o.direction),
      classFn: o => o.direction === OrderDirection.Inbound ? 'badge-blue' : 'badge-purple'
    },
    {
      key: 'originDestination',
      label: 'Origin -> Destination',
      formatter: o => `${o.origin || '-'} -> ${o.destination || '-'}`
    },
    {
      key: 'quotedTotal',
      label: 'Quoted Total',
      sortable: true,
      formatter: o => this.money(o.quotedTotal)
    },
    {
      key: 'baseFreight',
      label: 'Base Freight',
      sortable: true,
      formatter: o => this.money(o.baseFreight)
    },
    {
      key: 'accessorials',
      label: 'Accessorials',
      sortable: true,
      formatter: o => this.money(o.accessorials)
    },
    { key: 'commodity', label: 'Commodity', formatter: o => o.commodity || '-' },
    { key: 'primaryPONumber', label: 'PO', formatter: o => o.primaryPONumber || '-' },
    { key: 'primaryBolNumber', label: 'BOL', formatter: o => o.primaryBolNumber || '-' },
    { key: 'primaryProNumber', label: 'PRO', formatter: o => o.primaryProNumber || '-' },
    { key: 'totalWeight', label: 'Weight', sortable: true, formatter: o => o.totalWeight != null ? String(o.totalWeight) : '-' },
    { key: 'totalPallets', label: 'Pallets', sortable: true, formatter: o => o.totalPallets != null ? String(o.totalPallets) : '-' },
    { key: 'totalVolume', label: 'Volume', sortable: true, formatter: o => o.totalVolume != null ? String(o.totalVolume) : '-' },
    {
      key: 'load',
      label: 'Load',
      formatter: o => o.hasActiveLoad ? (o.activeLoadNumber || 'Linked') : 'No load',
      routerLink: o => o.activeLoadId ? `/loads/${o.activeLoadId}` : null,
      classFn: o => o.hasActiveLoad ? 'badge-success' : 'badge-muted'
    },
    {
      key: 'status',
      label: 'Status',
      sortable: true,
      formatter: o => this.statusLabel(o.status),
      classFn: o => this.statusClass(o.status)
    },
    {
      key: 'phase',
      label: 'Phase',
      sortable: true,
      formatter: o => this.phaseLabel(o.phase),
      classFn: o => this.phaseClass(o.phase)
    },
    {
      key: 'plannedWindow',
      label: 'Planned Window',
      formatter: o =>
        `${o.plannedPickupDate ? new Date(o.plannedPickupDate).toLocaleDateString() : '-'} -> ${o.plannedDeliveryDate ? new Date(o.plannedDeliveryDate).toLocaleDateString() : '-'}`
    },
    {
      key: 'orderWindow',
      label: 'Order Window',
      formatter: o =>
        `${new Date(o.startDate).toLocaleDateString()} -> ${new Date(o.endDate).toLocaleDateString()}`
    },
    {
      key: 'updatedAt',
      label: 'Last Updated',
      sortable: true,
      formatter: o =>
        o.updatedAt
          ? new Date(o.updatedAt).toLocaleString()
          : new Date(o.createdAt).toLocaleString()
    }
  ];

  get columns(): TableColumn<OrderListItem>[] {
    if (!this.columnOrder.length) return this.allColumns;

    const byKey = new Map(this.allColumns.map(c => [String(c.key), c]));
    return this.columnOrder
      .map(key => byKey.get(key))
      .filter((c): c is TableColumn<OrderListItem> => !!c)
      .filter(c => this.columnVisibility.get(String(c.key)) !== false);
  }

  get columnControls() {
    const byKey = new Map(this.allColumns.map(c => [String(c.key), c]));
    return this.columnOrder
      .map((key, index) => {
        const column = byKey.get(key);
        if (!column) return null;
        return {
          key,
          label: column.label,
          visible: this.columnVisibility.get(key) !== false,
          locked: this.mandatoryColumnKeys.has(key),
          index
        };
      })
      .filter((x): x is { key: string; label: string; visible: boolean; locked: boolean; index: number } => !!x);
  }

  actions: TableAction<OrderListItem>[] = [
    {
      label: 'View',
      variant: 'ghost',
      routerLink: o => `/orders/${o.id}`
    }
  ];

  constructor(
    private ordersService: OrdersService,
    private tablePreferencesService: TablePreferencesService,
    private carriersService: CarriersService,
    private auth: AuthFacade,
    private route: ActivatedRoute,
    private router: Router
  ) {
    super();
  }

  ngOnInit(): void {
    this.canCreate = this.auth.hasPermission('Load_Create');
    this.filtersOpen = false;
    this.carriersService.getAll().subscribe({
      next: carriers => this.carriers = carriers,
      error: () => this.carriers = []
    });
    this.initColumns();
    this.loadColumnsFromBackend();

    this.route.url.subscribe(() => {
      const routePath = this.route.snapshot.routeConfig?.path;
      this.creatingOrder = routePath === 'create';
    });

    this.reload();
  }

  toggleFilters() {
    this.filtersOpen = !this.filtersOpen;
  }

  applyFilters() {
    const next: Record<string, any> = {};
    const search = this.filterModel.search?.trim();
    if (search) next['search'] = search;
    if (this.filterModel.status !== '') next['status'] = Number(this.filterModel.status);
    if (this.filterModel.phase !== '') next['phase'] = Number(this.filterModel.phase);
    if (this.filterModel.direction !== '') next['direction'] = Number(this.filterModel.direction);
    if (this.filterModel.preferredCarrierId !== '') next['preferredCarrierId'] = this.filterModel.preferredCarrierId;
    if (this.filterModel.fromDate) next['fromDate'] = this.filterModel.fromDate;
    if (this.filterModel.toDate) next['toDate'] = this.filterModel.toDate;

    this.activeFilters = next;
    this.page = 1;
    this.reload();
  }

  clearFilters() {
    this.filterModel = {
      search: '',
      status: '',
      phase: '',
      direction: '',
      preferredCarrierId: '',
      fromDate: '',
      toDate: ''
    };
    this.activeFilters = {};
    this.page = 1;
    this.reload();
  }

  removeFilterChip(key: string) {
    delete this.activeFilters[key];
    switch (key) {
      case 'search': this.filterModel.search = ''; break;
      case 'status': this.filterModel.status = ''; break;
      case 'phase': this.filterModel.phase = ''; break;
      case 'direction': this.filterModel.direction = ''; break;
      case 'preferredCarrierId': this.filterModel.preferredCarrierId = ''; break;
      case 'fromDate': this.filterModel.fromDate = ''; break;
      case 'toDate': this.filterModel.toDate = ''; break;
    }
    this.page = 1;
    this.reload();
  }

  formatFilterValue(key: string, value: any): string {
    if (key === 'status') return this.statusLabel(Number(value));
    if (key === 'phase') return this.phaseLabel(Number(value));
    if (key === 'direction') return this.directionLabel(Number(value));
    if (key === 'preferredCarrierId') return this.carriers.find(c => c.id === value)?.name || String(value);
    return String(value);
  }

  toggleColumnMenu() {
    if (!this.showColumnMenu) {
      this.snapshotColumnOrder = [...this.columnOrder];
      this.snapshotColumnVisibility = new Map(this.columnVisibility);
      this.mandatoryColumnKeys.forEach(key => this.snapshotColumnVisibility.set(key, true));
      this.showColumnMenu = true;
      return;
    }

    this.cancelColumnConfig();
  }

  setColumnVisible(key: string, visible: boolean) {
    if (this.mandatoryColumnKeys.has(key)) return;
    this.columnVisibility.set(key, visible);
  }

  isColumnVisible(key: string) {
    return this.columnVisibility.get(key) !== false;
  }

  onColumnCheckboxClick(key: string, event: MouseEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.setColumnVisible(key, !this.isColumnVisible(key));
  }

  moveColumn(key: string, direction: -1 | 1) {
    const index = this.columnOrder.indexOf(key);
    if (index < 0) return;
    const newIndex = index + direction;
    if (newIndex < 0 || newIndex >= this.columnOrder.length) return;
    const [item] = this.columnOrder.splice(index, 1);
    this.columnOrder.splice(newIndex, 0, item);
  }

  saveColumnConfig() {
    this.mandatoryColumnKeys.forEach(key => this.columnVisibility.set(key, true));
    this.persistColumns();
    this.closeColumnMenu();
  }

  cancelColumnConfig() {
    if (this.snapshotColumnOrder.length) {
      this.columnOrder = [...this.snapshotColumnOrder];
    }
    if (this.snapshotColumnVisibility.size) {
      this.columnVisibility = new Map(this.snapshotColumnVisibility);
    }
    this.mandatoryColumnKeys.forEach(key => this.columnVisibility.set(key, true));
    this.closeColumnMenu();
  }

  private closeColumnMenu() {
    this.showColumnMenu = false;
    this.snapshotColumnOrder = [];
    this.snapshotColumnVisibility = new Map();
  }

  openCreateOrder() {
    this.router.navigate(['create'], { relativeTo: this.route });
  }

  onCreateOrderClose(created: boolean) {
    this.creatingOrder = false;
    this.router.navigate(['../'], { relativeTo: this.route });

    if (created) {
      this.reload();
    }
  }

  protected loadData(query: OrdersQueryParameters) {
    this.ordersService.getPaged(query).subscribe(res => {
      this.orders = res.items;
      this.totalCount = res.total;
      this.page = res.page;
      this.pageSize = res.pageSize;
    });
  }

  private statusLabel(value: number) {
    const labels: Record<number, string> = {
      [OrderStatus.Draft]: 'Draft',
      [OrderStatus.Submitted]: 'Submitted',
      [OrderStatus.Confirmed]: 'Confirmed',
      [OrderStatus.Scheduled]: 'Scheduled',
      [OrderStatus.Dispatched]: 'Dispatched',
      [OrderStatus.AtPickup]: 'At Pickup',
      [OrderStatus.PickedUp]: 'Picked Up',
      [OrderStatus.InTransit]: 'In Transit',
      [OrderStatus.AtDelivery]: 'At Delivery',
      [OrderStatus.Delivered]: 'Delivered',
      [OrderStatus.ReadyForBilling]: 'Ready For Billing',
      [OrderStatus.Billed]: 'Billed',
      [OrderStatus.Completed]: 'Completed',
      [OrderStatus.Cancelled]: 'Cancelled'
    };
    return labels[value] ?? String(value);
  }

  private phaseLabel(value: number) {
    const labels: Record<number, string> = {
      [OrderPhase.Open]: 'Open',
      [OrderPhase.Plan]: 'Plan',
      [OrderPhase.Ship]: 'Ship',
      [OrderPhase.Bill]: 'Bill',
      [OrderPhase.Complete]: 'Complete',
      [OrderPhase.Cancelled]: 'Cancelled'
    };
    return labels[value] ?? String(value);
  }

  private directionLabel(value: number) {
    const labels: Record<number, string> = {
      [OrderDirection.Inbound]: 'Inbound',
      [OrderDirection.Outbound]: 'Outbound',
      [OrderDirection.Transfer]: 'Transfer'
    };
    return labels[value] ?? String(value);
  }

  private money(value?: number | null) {
    return Number(value ?? 0).toLocaleString(undefined, {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    });
  }

  private statusClass(status: number | string) {
    const key = this.statusKey(status);
    if (key === 'draft') return 'badge-slate';
    if (key === 'submitted') return 'badge-indigo';
    if (key === 'confirmed') return 'badge-violet';
    if (key === 'scheduled') return 'badge-cyan';
    if (key === 'dispatched') return 'badge-blue';
    if (key === 'atpickup') return 'badge-amber';
    if (key === 'pickedup') return 'badge-lime';
    if (key === 'intransit') return 'badge-orange';
    if (key === 'atdelivery') return 'badge-teal';
    if (key === 'delivered') return 'badge-green';
    if (key === 'readyforbilling') return 'badge-yellow';
    if (key === 'billed') return 'badge-emerald';
    if (key === 'completed') return 'badge-success';
    if (key === 'cancelled') return 'badge-danger';
    return 'badge-muted';
  }

  private phaseClass(phase: number | string) {
    const key = this.phaseKey(phase);
    if (key === 'cancelled') return 'badge-danger';
    if (key === 'complete') return 'badge-success';
    if (key === 'bill') return 'badge-warning';
    if (key === 'ship') return 'badge-blue';
    if (key === 'plan') return 'badge-purple';
    if (key === 'open') return 'badge-muted';
    return 'badge-muted';
  }

  private statusKey(value: number | string) {
    if (typeof value === 'string') return value.replace(/\s+/g, '').toLowerCase();
    return String(OrderStatus[value] ?? value).replace(/\s+/g, '').toLowerCase();
  }

  private phaseKey(value: number | string) {
    if (typeof value === 'string') return value.replace(/\s+/g, '').toLowerCase();
    return String(OrderPhase[value] ?? value).replace(/\s+/g, '').toLowerCase();
  }

  private initColumns() {
    const defaultOrder = this.allColumns.map(c => String(c.key));
    this.columnOrder = defaultOrder;
    defaultOrder.forEach(k => this.columnVisibility.set(k, true));

    const raw = localStorage.getItem(this.columnStorageKey);
    if (!raw) return;

    try {
      const parsed = JSON.parse(raw) as {
        order?: string[];
        visibility?: Record<string, boolean>;
      };

      if (Array.isArray(parsed.order) && parsed.order.length) {
        const allowed = new Set(defaultOrder);
        const cleaned = parsed.order.filter(k => allowed.has(k));
        const missing = defaultOrder.filter(k => !cleaned.includes(k));
        this.columnOrder = [...cleaned, ...missing];
      }

      if (parsed.visibility && typeof parsed.visibility === 'object') {
        Object.entries(parsed.visibility).forEach(([key, value]) => {
          if (defaultOrder.includes(key)) {
            this.columnVisibility.set(key, !!value);
          }
        });
      }
    } catch {
      this.columnOrder = defaultOrder;
      defaultOrder.forEach(k => this.columnVisibility.set(k, true));
    }

    this.mandatoryColumnKeys.forEach(key => this.columnVisibility.set(key, true));
  }

  private persistColumns() {
    const visibility: Record<string, boolean> = {};
    this.columnOrder.forEach(key => {
      visibility[key] = this.columnVisibility.get(key) !== false;
    });

    const payload = JSON.stringify({
      order: this.columnOrder,
      visibility
    });

    localStorage.setItem(this.columnStorageKey, payload);

    if (!this.tablePreferencesAvailable) return;

    this.tablePreferencesService.save(this.tableKey, { jsonConfig: payload }).subscribe({
      error: () => {
        this.tablePreferencesAvailable = false;
      }
    });
  }

  private loadColumnsFromBackend() {
    if (!this.tablePreferencesAvailable) return;

    this.tablePreferencesService.get(this.tableKey).subscribe({
      next: pref => {
        if (!pref?.jsonConfig) return;
        this.applyColumnConfig(pref.jsonConfig);
      },
      error: () => {
        this.tablePreferencesAvailable = false;
      }
    });
  }

  private applyColumnConfig(raw: string) {
    const defaultOrder = this.allColumns.map(c => String(c.key));
    try {
      const parsed = JSON.parse(raw) as {
        order?: string[];
        visibility?: Record<string, boolean>;
      };

      if (Array.isArray(parsed.order) && parsed.order.length) {
        const allowed = new Set(defaultOrder);
        const cleaned = parsed.order.filter(k => allowed.has(k));
        const missing = defaultOrder.filter(k => !cleaned.includes(k));
        this.columnOrder = [...cleaned, ...missing];
      }

      if (parsed.visibility && typeof parsed.visibility === 'object') {
        Object.entries(parsed.visibility).forEach(([key, value]) => {
          if (defaultOrder.includes(key)) {
            this.columnVisibility.set(key, !!value);
          }
        });
      }

      this.mandatoryColumnKeys.forEach(key => this.columnVisibility.set(key, true));
    } catch {
      // ignore malformed config
    }
  }
}
