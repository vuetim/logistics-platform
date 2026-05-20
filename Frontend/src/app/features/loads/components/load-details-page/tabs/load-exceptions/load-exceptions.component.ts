import { CommonModule } from "@angular/common";
import { Component, Input, OnInit } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ToastrService } from "ngx-toastr";
import { LoadExceptionStatus } from "../../../../../../core/enums/loads/load-exception-status.enum";
import { LoadExceptionDto } from "../../../../../../core/models/loads/load-details.dto";
import { LoadsService } from "../../../../../../data-access/loads/loads.service";
import { AuthFacade } from "../../../../../../core/auth/auth.facade";
import { Permission } from "../../../../../../core/auth/permissions/permission.enum";

@Component({
  selector: 'app-load-exceptions',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './load-exceptions.component.html',
  styleUrl: '../load-tab-shared.css'
})
export class LoadExceptionsComponent implements OnInit {
  @Input({ required: true }) loadId!: string;

  readonly Status = LoadExceptionStatus;
  exceptions: LoadExceptionDto[] = [];
  form = this.emptyForm();

  readonly statusOptions = [
    { label: 'Open', value: LoadExceptionStatus.Open },
    { label: 'Valid', value: LoadExceptionStatus.Valid },
    { label: 'In review', value: LoadExceptionStatus.InReview },
    { label: 'Resolved', value: LoadExceptionStatus.Resolved },
    { label: 'Rejected', value: LoadExceptionStatus.Rejected },
    { label: 'Cancelled', value: LoadExceptionStatus.Cancelled }
  ];

  constructor(
    private loadsService: LoadsService,
    private toastr: ToastrService,
    private auth: AuthFacade
  ) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.loadsService.getExceptions(this.loadId).subscribe({
      next: res => (this.exceptions = this.sortExceptions(res)),
      error: err => this.toastr.error(this.errorMessage(err), "Failed to load exceptions")
    });
  }

  get openExceptions() {
    return this.exceptions.filter(x => !this.isClosed(x.status));
  }

  get closedExceptions() {
    return this.exceptions.filter(x => this.isClosed(x.status));
  }

  create() {
    if (!this.canCreate() || !this.form.exceptionValue.trim()) return;
    this.loadsService.createException(this.loadId, {
      ...this.form,
      occurredAt: new Date().toISOString()
    }).subscribe({
      next: () => {
        this.toastr.success("Exception added");
        this.form = this.emptyForm();
        this.load();
      },
      error: err => this.toastr.error(this.errorMessage(err), "Failed to add exception")
    });
  }

  updateStatus(item: LoadExceptionDto, status: LoadExceptionStatus) {
    if (!this.canUpdate()) return;
    this.loadsService.updateException(this.loadId, item.id, { status }).subscribe({
      next: () => {
        this.toastr.success("Exception updated");
        this.load();
      },
      error: err => this.toastr.error(this.errorMessage(err), "Failed to update exception")
    });
  }

  statusLabel(value: number | string) {
    if (typeof value === 'string') return this.humanize(value);
    return this.humanize(LoadExceptionStatus[value] ?? String(value));
  }

  canCreate() {
    return this.auth.hasRole('Admin') || this.auth.hasPermission(Permission.LoadException_Create);
  }

  canUpdate() {
    return this.auth.hasRole('Admin') || this.auth.hasPermission(Permission.LoadException_Update);
  }

  private sortExceptions(items: LoadExceptionDto[]) {
    return [...items].sort((a, b) => {
      const closedDiff = Number(this.isClosed(a.status)) - Number(this.isClosed(b.status));
      if (closedDiff !== 0) return closedDiff;
      return new Date(b.occurredAt).getTime() - new Date(a.occurredAt).getTime();
    });
  }

  private isClosed(value: number | string) {
    const key = typeof value === 'string'
      ? value.replace(/\s+/g, '').toLowerCase()
      : String(LoadExceptionStatus[value] ?? value).replace(/\s+/g, '').toLowerCase();
    return ['resolved', 'rejected', 'cancelled'].includes(key);
  }

  private emptyForm() {
    return {
      exceptionKey: 'shipment-exception',
      exceptionValue: '',
      reasonKey: '',
      reasonValue: '',
      ediReasonCode: '',
      responsiblePartyKey: '',
      responsiblePartyValue: '',
      status: LoadExceptionStatus.Open,
      description: '',
      affectedItemName: '',
      affectedItemReference: '',
      quantity: null as number | null,
      unit: ''
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
}
