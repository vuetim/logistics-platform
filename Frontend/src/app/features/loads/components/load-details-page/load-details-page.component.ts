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
import { OrderDirection } from "../../../../core/enums/orders/order-direction.enum";
import { OrderType } from "../../../../core/enums/orders/order-type.enum";
import { LoadStopsComponent } from "./tabs/load-stops/load-stops.component";
import { LoadItemsComponent } from "./tabs/load-items/load-items.component";
import { LoadEquipmentComponent } from "./tabs/load-equipment/load-equipment.component";
import { LoadCostsComponent } from "./tabs/load-costs/load-costs.component";
import { LoadNotesComponent } from "./tabs/load-notes/load-notes.component";
import { LoadDocumentsComponent } from "./tabs/load-documents/load-documents.component";
import { LoadActivityComponent } from "./tabs/load-activity/load-activity.component";
import { LoadDispatcherPanelComponent } from "./load-dispatcher-panel/load-dispatcher-panel.component";
import { LoadBillingComponent } from "./tabs/load-billing/load-billing.component";

type TabKey = 'overview' | 'stops' | 'items' | 'equipment' | 'costs' | 'billing' | 'notes' | 'documents' | 'activity';

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
    LoadDispatcherPanelComponent,
    LoadBillingComponent
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
    private toastr: ToastrService
  ) {}

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
    return ['overview', 'stops', 'items', 'equipment', 'costs', 'billing', 'notes', 'documents', 'activity'].includes(value);
  }

  isTabVisited(t: TabKey) {
    return this.visitedTabs.has(t);
  }

  statusLabel(value: number | string) {
    if (typeof value === 'string') {
      const numeric = (LoadStatus as any)[value];
      return typeof numeric === 'number' ? this.humanize(value) : this.humanize(String(value));
    }

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

  canMarkAccepted() {
    const status = this.statusKey(this.load?.execution.status);
    return !!this.load?.execution.carrierId && status !== 'accepted' && status !== 'dispatched' && status !== 'intransit' && status !== 'delivered' && status !== 'completed';
  }

  canDispatch() {
    return this.statusKey(this.load?.execution.status) === 'accepted' && !!this.load?.execution.carrierId;
  }

  canComplete() {
    return this.statusKey(this.load?.execution.status) === 'delivered';
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
}
