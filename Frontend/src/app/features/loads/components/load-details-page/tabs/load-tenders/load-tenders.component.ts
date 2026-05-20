import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ToastrService } from "ngx-toastr";
import { AssignmentStatus } from "../../../../../../core/enums/loads/assignment-status.enum";
import { LoadCarrierAssignmentDto } from "../../../../../../core/models/loads/load-details.dto";
import { CarrierListItem } from "../../../../../../core/models/carriers/carrier-list-item.model";
import { CarriersService } from "../../../../../../data-access/carriers/carriers.service";
import { LoadsService } from "../../../../../../data-access/loads/loads.service";
import { AuthFacade } from "../../../../../../core/auth/auth.facade";
import { Permission } from "../../../../../../core/auth/permissions/permission.enum";

@Component({
  selector: 'app-load-tenders',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './load-tenders.component.html',
  styleUrl: '../load-tab-shared.css'
})
export class LoadTendersComponent implements OnInit {
  @Input({ required: true }) loadId!: string;
  @Output() changed = new EventEmitter<void>();

  assignments: LoadCarrierAssignmentDto[] = [];
  carriers: CarrierListItem[] = [];
  loading = false;
  form = this.emptyForm();

  constructor(
    private loadsService: LoadsService,
    private carriersService: CarriersService,
    private toastr: ToastrService,
    private auth: AuthFacade
  ) {}

  ngOnInit() {
    this.load();
    this.carriersService.getAll().subscribe({
      next: res => (this.carriers = res),
      error: () => this.toastr.error("Failed to load carriers")
    });
  }

  load() {
    this.loading = true;
    this.loadsService.getCarrierAssignments(this.loadId).subscribe({
      next: res => {
        this.assignments = res;
        this.loading = false;
      },
      error: err => {
        this.loading = false;
        this.toastr.error(this.errorMessage(err), "Failed to load tenders");
      }
    });
  }

  tender() {
    if (!this.canCreate() || !this.form.carrierId) return;
    const payload = {
      carrierId: this.form.carrierId,
      offeredRate: this.form.offeredRate ?? null,
      currency: this.form.currency || 'USD',
      rateConfirmationNumber: this.form.rateConfirmationNumber || null,
      tenderMethod: this.form.tenderMethod || 'Manual',
      tenderNotes: this.form.tenderNotes || null,
      tenderExpiresAt: this.form.tenderExpiresAt || null,
      emailTo: this.form.emailTo || null
    };

    this.loadsService.tenderCarrier(this.loadId, payload).subscribe({
      next: () => {
        this.toastr.success("Carrier tender created");
        this.form = this.emptyForm();
        this.load();
        this.changed.emit();
      },
      error: err => this.toastr.error(this.errorMessage(err), "Failed to tender carrier")
    });
  }

  accept(item: LoadCarrierAssignmentDto) {
    if (!this.canAccept(item)) return;
    this.loadsService.acceptCarrierAssignment(this.loadId, item.id).subscribe({
      next: () => {
        this.toastr.success("Carrier marked covered");
        this.load();
        this.changed.emit();
      },
      error: err => this.toastr.error(this.errorMessage(err), "Failed to accept tender")
    });
  }

  reject(item: LoadCarrierAssignmentDto) {
    if (!this.canReject(item)) return;
    this.loadsService.rejectCarrierAssignment(this.loadId, item.id).subscribe({
      next: () => {
        this.toastr.success("Carrier tender rejected");
        this.load();
        this.changed.emit();
      },
      error: err => this.toastr.error(this.errorMessage(err), "Failed to reject tender")
    });
  }

  canAccept(item: LoadCarrierAssignmentDto) {
    return this.hasAny(Permission.CarrierOffer_Accept, Permission.Load_Tender) && this.statusKey(item.status) === 'tendered';
  }

  canReject(item: LoadCarrierAssignmentDto) {
    return this.hasAny(Permission.CarrierOffer_Reject, Permission.Load_Tender) && this.statusKey(item.status) === 'tendered';
  }

  canCreate() {
    return this.hasAny(Permission.CarrierOffer_Create, Permission.Load_Tender);
  }

  statusLabel(value: number | string) {
    if (typeof value === 'string') return this.humanize(value);
    return this.humanize(AssignmentStatus[value] ?? String(value));
  }

  money(value?: number | null) {
    return Number(value ?? 0).toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  private statusKey(value: number | string) {
    if (typeof value === 'string') return value.replace(/\s+/g, '').toLowerCase();
    return String(AssignmentStatus[value] ?? value).replace(/\s+/g, '').toLowerCase();
  }

  private emptyForm() {
    return {
      carrierId: '',
      offeredRate: null as number | null,
      currency: 'USD',
      rateConfirmationNumber: '',
      tenderMethod: 'Manual',
      tenderNotes: '',
      tenderExpiresAt: '',
      emailTo: ''
    };
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
