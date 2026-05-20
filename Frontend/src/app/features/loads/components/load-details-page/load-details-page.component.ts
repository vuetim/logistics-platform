import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ActivatedRoute, RouterModule } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { PageLayoutComponent } from "../../../../layout/app-shell/page-layout/page-layout/page-layout.component";
import { LoadDetailsDto } from "../../../../core/models/loads/load-details.dto";
import { LoadsService } from "../../../../data-access/loads/loads.service";
import { LoadStatus } from "../../../../core/enums/loads/load-status.enum";
import { ModeType } from "../../../../core/enums/loads/mode-type.enum";
import { EquipmentType } from "../../../../core/enums/loads/equipment-type.enum";
import { OrderDirection } from "../../../../core/enums/orders/order-direction.enum";
import { OrderType } from "../../../../core/enums/orders/order-type.enum";
import { StopType } from "../../../../core/enums/orders/stop-type.enum";
import { AppointmentType } from "../../../../core/enums/loads/appointment-type.enum";
import { LoadStopsComponent } from "./tabs/load-stops/load-stops.component";
import { LoadItemsComponent } from "./tabs/load-items/load-items.component";
import { LoadEquipmentComponent } from "./tabs/load-equipment/load-equipment.component";
import { LoadCostsComponent } from "./tabs/load-costs/load-costs.component";
import { LoadNotesComponent } from "./tabs/load-notes/load-notes.component";
import { LoadDocumentsComponent } from "./tabs/load-documents/load-documents.component";
import { LoadActivityComponent } from "./tabs/load-activity/load-activity.component";
import { LoadBillingComponent } from "./tabs/load-billing/load-billing.component";
import { LoadRouteMapComponent } from "./load-route-map/load-route-map.component";
import { AuthFacade } from "../../../../core/auth/auth.facade";
import { Permission } from "../../../../core/auth/permissions/permission.enum";
import { LoadTendersComponent } from "./tabs/load-tenders/load-tenders.component";
import { LoadExceptionsComponent } from "./tabs/load-exceptions/load-exceptions.component";
import { LoadOperationalPanelComponent } from "./load-operational-panel/load-operational-panel.component";

type TabKey = 'overview' | 'stops' | 'items' | 'equipment' | 'tenders' | 'costs' | 'billing' | 'exceptions' | 'notes' | 'documents' | 'activity';

@Component({
  selector: 'app-load-details-page',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    PageLayoutComponent,
    LoadStopsComponent,
    LoadItemsComponent,
    LoadEquipmentComponent,
    LoadCostsComponent,
    LoadNotesComponent,
    LoadDocumentsComponent,
    LoadActivityComponent,
    LoadOperationalPanelComponent,
    LoadBillingComponent,
    LoadRouteMapComponent,
    LoadTendersComponent,
    LoadExceptionsComponent
  ],
  templateUrl: './load-details-page.component.html',
  styleUrl: './load-details-page.component.css'
})
export class LoadDetailsPageComponent implements OnInit {
  loadId!: string;
  load?: LoadDetailsDto;
  loading = true;
  actionLoading = false;
  dispatching = false;
  dispatchForm = {
    driverName: '',
    driverPhone: '',
    driverEmail: '',
    truckNumber: '',
    trailerNumber: ''
  };
  tab: TabKey = 'overview';
  private readonly visitedTabs = new Set<TabKey>(['overview']);

  constructor(
    private route: ActivatedRoute,
    private loadsService: LoadsService,
    private toastr: ToastrService,
    private auth: AuthFacade
  ) { }

  ngOnInit() {
    this.loadId = this.route.snapshot.paramMap.get('id')!;
    const requestedTab = this.route.snapshot.queryParamMap.get('tab') as TabKey | null;
    if (requestedTab && this.isValidTab(requestedTab)) {
      this.setTab(requestedTab);
    }
    this.reload();
  }

  reload() {
    this.loading = true;
    this.loadsService.getDetails(this.loadId).subscribe({
      next: res => {
        this.load = res;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.toastr.error("Failed to load load details");
      }
    });
  }

  setTab(t: TabKey) {
    this.tab = t;
    this.visitedTabs.add(t);
  }

  private isValidTab(value: string): value is TabKey {
    return ['overview', 'stops', 'items', 'equipment', 'tenders', 'costs', 'billing', 'exceptions', 'notes', 'documents', 'activity'].includes(value);
  }

  isTabVisited(t: TabKey) {
    return this.visitedTabs.has(t);
  }

  statusLabel(value: number | string) {
    if (typeof value === 'string') {
      const numeric = (LoadStatus as any)[value];
      if (value.toLowerCase() === 'accepted') return 'Covered';
      return typeof numeric === 'number' ? this.humanize(value) : this.humanize(String(value));
    }

    if (value === LoadStatus.Accepted) return 'Covered';
    return this.humanize(LoadStatus[value] ?? String(value));
  }

  statusClass(value: number | string) {
    const key = this.statusKey(value);
    if (key === 'draft') return 'status-slate';
    if (key === 'planned') return 'status-indigo';
    if (key === 'tendered') return 'status-violet';
    if (key === 'accepted') return 'status-emerald';
    if (key === 'dispatched') return 'status-blue';
    if (key === 'enroutetopickup') return 'status-cyan';
    if (key === 'atpickup') return 'status-amber';
    if (key === 'loaded') return 'status-lime';
    if (key === 'intransit') return 'status-orange';
    if (key === 'enroutetodelivery') return 'status-rose';
    if (key === 'atdelivery') return 'status-teal';
    if (key === 'delivered') return 'status-green';
    if (key === 'completed') return 'status-success';
    if (key === 'cancelled') return 'status-danger';
    if (key === 'rejected') return 'status-pink';
    return 'status-muted';
  }

  modeLabel(value: number | string) {
    if (typeof value === 'string') {
      const numeric = (ModeType as any)[value];
      return typeof numeric === 'number' ? value : String(value);
    }

    return ModeType[value] ?? String(value);
  }

  directionLabel(value: number | string) {
    if (typeof value === 'string') return this.humanize(value);
    return this.humanize(OrderDirection[value] ?? String(value));
  }

  orderTypeLabel(value: number | string) {
    if (typeof value === 'string') return this.humanize(value);
    return this.humanize(OrderType[value] ?? String(value));
  }

  money(value?: number | null) {
    return Number(value ?? 0).toLocaleString(undefined, {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    });
  }

  equipmentText(load: LoadDetailsDto) {
    if (!load.equipment?.length) return '-';
    return load.equipment
      .map(e => `${e.quantity || 1} ${this.humanize(EquipmentType[e.equipmentType] ?? e.equipmentType)}`)
      .join(', ');
  }

  stopTypeLabel(value: number | string) {
    if (typeof value === 'string') return this.humanize(value);
    return this.humanize(StopType[value] ?? String(value));
  }

  appointmentLabel(value: number | string) {
    if (typeof value === 'string') return this.humanize(value);
    return this.humanize(AppointmentType[value] ?? String(value));
  }

  cityState(stop: { city?: string | null; state?: string | null; country?: string | null }) {
    return [stop.city, stop.state || stop.country].filter(Boolean).join(', ') || '-';
  }

  stopWindow(stop: { plannedArrivalFrom?: string | null; plannedArrivalTo?: string | null }) {
    const from = stop.plannedArrivalFrom ? new Date(stop.plannedArrivalFrom) : null;
    const to = stop.plannedArrivalTo ? new Date(stop.plannedArrivalTo) : null;
    const fromText = from && !Number.isNaN(from.getTime()) ? from.toLocaleString() : '-';
    const toText = to && !Number.isNaN(to.getTime()) ? to.toLocaleString() : '';
    return toText ? `${fromText} - ${toText}` : fromText;
  }

  stopClass(stop: { stopType: number | string }) {
    const key = typeof stop.stopType === 'string' ? stop.stopType.toLowerCase() : StopType[stop.stopType]?.toLowerCase();
    return {
      pickup: key === 'pickup',
      delivery: key === 'delivery'
    };
  }

  overviewStops(load: LoadDetailsDto) {
    const stops = [...(load.execution.stops || [])];
    const typeRank = (stop: { stopType: number | string }) => {
      const key = typeof stop.stopType === 'string' ? stop.stopType.toLowerCase() : StopType[stop.stopType]?.toLowerCase();
      if (key === 'pickup') return 0;
      if (key === 'delivery') return 2;
      return 1;
    };

    return stops.sort((a, b) => {
      const rankDiff = typeRank(a) - typeRank(b);
      return rankDiff !== 0 ? rankDiff : a.sequence - b.sequence;
    });
  }

  marginPercent(load: LoadDetailsDto) {
    const billable = Number(load.costSummary?.totalBillable ?? 0);
    const margin = Number(load.costSummary?.margin ?? load.execution.margin ?? 0);
    if (!billable) return '';
    return `(${((margin / billable) * 100).toFixed(1)}%)`;
  }

  canViewTracking() {
    return this.auth.hasRole('Admin') || this.auth.hasPermission(Permission.Load_Tracking_View);
  }

  canViewCarrierOffers() {
    return this.hasAny(Permission.CarrierOffer_View, Permission.CarrierOffer_View_All, Permission.Load_Tender);
  }

  canViewExceptions() {
    return this.hasAny(Permission.LoadException_View);
  }

  canViewCosts() {
    return this.hasAny(Permission.LoadCost_View);
  }

  canViewFinancials() {
    return this.hasAny(Permission.Financial_View);
  }

  canMarkAccepted() {
    const status = this.statusKey(this.load?.execution.status);
    return this.hasAny(Permission.Load_ChangeStatus) &&
      !!this.load?.execution.carrierId &&
      status !== 'accepted' &&
      status !== 'dispatched' &&
      status !== 'intransit' &&
      status !== 'delivered' &&
      status !== 'completed';
  }

  canDispatch() {
    return this.hasAny(Permission.Load_Dispatch) &&
      this.statusKey(this.load?.execution.status) === 'accepted' &&
      !!this.load?.execution.carrierId;
  }

  canComplete() {
    return this.hasAny(Permission.Load_ChangeStatus) &&
      this.statusKey(this.load?.execution.status) === 'delivered';
  }

  markAccepted() {
    if (!this.load || !this.canMarkAccepted()) return;
    this.changeStatus(LoadStatus.Accepted, "Load marked accepted");
  }

  completeLoad() {
    if (!this.load || !this.canComplete()) return;
    if (!confirm("Complete this load and generate financial documents?")) return;
    this.changeStatus(LoadStatus.Completed, "Load completed");
  }

  openDispatch() {
    if (!this.load || !this.canDispatch()) return;
    this.dispatchForm = {
      driverName: this.load.execution.driverName || '',
      driverPhone: this.load.execution.driverPhone || '',
      driverEmail: this.load.execution.driverEmail || '',
      truckNumber: this.load.execution.truckNumber || '',
      trailerNumber: this.load.execution.trailerNumber || ''
    };
    this.dispatching = true;
  }

  dispatchLoad() {
    if (!this.load || !this.dispatchForm.driverName.trim() || !this.dispatchForm.truckNumber.trim()) return;
    this.actionLoading = true;
    this.loadsService.dispatch(this.load.execution.id, this.dispatchForm).subscribe({
      next: () => {
        this.actionLoading = false;
        this.dispatching = false;
        this.toastr.success("Load dispatched");
        this.reload();
      },
      error: err => {
        this.actionLoading = false;
        this.toastr.error(this.errorMessage(err), "Failed to dispatch load");
      }
    });
  }

  private changeStatus(status: LoadStatus, success: string) {
    if (!this.load) return;
    this.actionLoading = true;
    this.loadsService.changeStatus(this.load.execution.id, status).subscribe({
      next: () => {
        this.actionLoading = false;
        this.toastr.success(success);
        this.reload();
      },
      error: err => {
        this.actionLoading = false;
        this.toastr.error(this.errorMessage(err), "Failed to change status");
      }
    });
  }

  private statusKey(value: number | string | undefined) {
    if (value == null) return '';
    if (typeof value === 'string') return value.replace(/\s+/g, '').toLowerCase();
    return String(LoadStatus[value] ?? value).replace(/\s+/g, '').toLowerCase();
  }

  private errorMessage(err: any) {
    if (!err?.error) return "Unexpected server error.";
    if (typeof err.error === 'string') return err.error;
    return err.error.message || err.error.title || "Unexpected server error.";
  }

  private humanize(value: unknown) {
    return String(value).replace(/([A-Z])/g, ' $1').trim();
  }

  private hasAny(...permissions: Permission[]) {
    return this.auth.hasRole('Admin') || permissions.some(p => this.auth.hasPermission(p));
  }
}
