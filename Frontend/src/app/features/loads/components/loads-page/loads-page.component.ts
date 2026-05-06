import { KeyValuePipe, NgFor, NgIf } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { GenericListPage } from "../../../../shared/list/generic-list-page";
import { LoadsQueryParameters } from "../../../../core/models/loads/loads-query-parameters.model";
import { LoadListItem } from "../../../../core/models/loads/load-list-item.model";
import { LoadsService } from "../../../../data-access/loads/loads.service";
import { TableAction, TableColumn } from "../../../../shared/UI/entity-table/entity-table.models";
import { PageLayoutComponent } from "../../../../layout/app-shell/page-layout/page-layout/page-layout.component";
import { UiCardComponent } from "../../../../shared/UI/ui-card/ui-card.component";
import { UiButtonComponent } from "../../../../shared/UI/ui-button/ui-button.component";
import { EntityTableComponent } from "../../../../shared/UI/entity-table/entity-table.component";
import { PaginationComponent } from "../../../../shared/UI/pagination/pagination.component";
import { LoadStatus } from "../../../../core/enums/loads/load-status.enum";
import { ModeType } from "../../../../core/enums/loads/mode-type.enum";

@Component({
  selector: 'app-loads-page',
  standalone: true,
  imports: [NgIf, NgFor, FormsModule, KeyValuePipe, PageLayoutComponent, UiCardComponent, UiButtonComponent, EntityTableComponent, PaginationComponent],
  templateUrl: './loads-page.component.html',
  styleUrl: './loads-page.component.css'
})
export class LoadsPageComponent extends GenericListPage<LoadsQueryParameters> implements OnInit {
  loads: LoadListItem[] = [];
  filterModel = {
    search: '',
    status: '',
    mode: '',
    pickupFrom: '',
    pickupTo: '',
    deliveryFrom: '',
    deliveryTo: ''
  };

  readonly statusOptions = Object.keys(LoadStatus)
    .filter(k => !isNaN(Number((LoadStatus as any)[k])))
    .map(k => ({ label: this.humanize(k), value: String((LoadStatus as any)[k]) }));

  readonly modeOptions = Object.keys(ModeType)
    .filter(k => !isNaN(Number((ModeType as any)[k])))
    .map(k => ({ label: k, value: String((ModeType as any)[k]) }));

  columns: TableColumn<LoadListItem>[] = [
    { key: 'loadNumber', label: 'Load #', sortable: true },
    { key: 'customerName', label: 'Customer', sortable: true },
    { key: 'carrierName', label: 'Carrier', formatter: l => l.carrierName || '-' },
    { key: 'modeType', label: 'Mode', formatter: l => this.modeLabel(l.modeType) },
    {
      key: 'status',
      label: 'Status',
      sortable: true,
      formatter: l => this.statusLabel(l.status),
      classFn: l => this.statusClass(l.status)
    },
    {
      key: 'pickupDate',
      label: 'Pickup',
      sortable: true,
      formatter: l => l.pickupDate ? new Date(l.pickupDate).toLocaleString() : '-'
    },
    {
      key: 'deliveryDate',
      label: 'Delivery',
      sortable: true,
      formatter: l => l.deliveryDate ? new Date(l.deliveryDate).toLocaleString() : '-'
    },
    { key: 'totalBillable', label: 'Total Billable', formatter: l => this.money(l.totalBillable) },
    { key: 'totalPayable', label: 'Total Payable', formatter: l => this.money(l.totalPayable) },
    { key: 'margin', label: 'Margin', formatter: l => this.money(l.margin) },
    { key: 'hasEquipment', label: 'Equipment', formatter: l => l.hasEquipment ? 'Yes' : 'No' }
  ];

  actions: TableAction<LoadListItem>[] = [
    {
      label: 'View',
      variant: 'ghost',
      routerLink: l => `/loads/${l.id}`
    }
  ];

  constructor(private loadsService: LoadsService) {
    super();
  }

  ngOnInit(): void {
    this.filtersOpen = false;
    this.reload();
  }

  toggleFilters() {
    this.filtersOpen = !this.filtersOpen;
  }

  applyFilters() {
    const next: Record<string, any> = {};
    const search = this.filterModel.search.trim();
    if (search) next['search'] = search;
    if (this.filterModel.status !== '') next['status'] = Number(this.filterModel.status);
    if (this.filterModel.mode !== '') next['mode'] = Number(this.filterModel.mode);
    if (this.filterModel.pickupFrom) next['pickupFrom'] = this.filterModel.pickupFrom;
    if (this.filterModel.pickupTo) next['pickupTo'] = this.filterModel.pickupTo;
    if (this.filterModel.deliveryFrom) next['deliveryFrom'] = this.filterModel.deliveryFrom;
    if (this.filterModel.deliveryTo) next['deliveryTo'] = this.filterModel.deliveryTo;

    this.activeFilters = next;
    this.page = 1;
    this.reload();
  }

  clearFilters() {
    this.filterModel = {
      search: '',
      status: '',
      mode: '',
      pickupFrom: '',
      pickupTo: '',
      deliveryFrom: '',
      deliveryTo: ''
    };
    this.activeFilters = {};
    this.page = 1;
    this.reload();
  }

  removeFilterChip(key: string) {
    delete this.activeFilters[key];
    if (key in this.filterModel) {
      (this.filterModel as any)[key] = '';
    }
    this.page = 1;
    this.reload();
  }

  formatFilterValue(key: string, value: any): string {
    if (key === 'status') return this.statusLabel(Number(value));
    if (key === 'mode') return this.modeLabel(Number(value));
    return String(value);
  }

  protected loadData(query: LoadsQueryParameters) {
    this.loadsService.getPaged(query).subscribe(res => {
      this.loads = res.items;
      this.totalCount = res.total;
      this.page = res.page;
      this.pageSize = res.pageSize;
    });
  }

  statusLabel(value: number | string) {
    if (typeof value === 'string') {
      const numeric = (LoadStatus as any)[value];
      return typeof numeric === 'number' ? this.humanize(value) : this.humanize(String(value));
    }

    return this.humanize(LoadStatus[value] ?? String(value));
  }

  modeLabel(value: number | string) {
    if (typeof value === 'string') {
      const numeric = (ModeType as any)[value];
      return typeof numeric === 'number' ? value : String(value);
    }

    return ModeType[value] ?? String(value);
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
    if (key === 'planned') return 'badge-indigo';
    if (key === 'tendered') return 'badge-violet';
    if (key === 'accepted') return 'badge-emerald';
    if (key === 'dispatched') return 'badge-blue';
    if (key === 'enroutetopickup') return 'badge-cyan';
    if (key === 'atpickup') return 'badge-amber';
    if (key === 'loaded') return 'badge-lime';
    if (key === 'intransit') return 'badge-orange';
    if (key === 'enroutetodelivery') return 'badge-rose';
    if (key === 'atdelivery') return 'badge-teal';
    if (key === 'delivered') return 'badge-green';
    if (key === 'completed') return 'badge-success';
    if (key === 'cancelled') return 'badge-danger';
    if (key === 'rejected') return 'badge-pink';
    return 'badge-muted';
  }

  private statusKey(value: number | string) {
    if (typeof value === 'string') return value.replace(/\s+/g, '').toLowerCase();
    return String(LoadStatus[value] ?? value).replace(/\s+/g, '').toLowerCase();
  }

  private humanize(value: unknown) {
    return String(value).replace(/([A-Z])/g, ' $1').trim();
  }
}
