import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ToastrService } from "ngx-toastr";
import { StopType } from "../../../../../../core/enums/orders/stop-type.enum";
import { StopStatus } from "../../../../../../core/enums/loads/stop-status.enum";
import { AppointmentType } from "../../../../../../core/enums/loads/appointment-type.enum";
import { AppointmentStatus } from "../../../../../../core/enums/loads/appointment-status.enum";
import { LoadStatus } from "../../../../../../core/enums/loads/load-status.enum";
import { LoadStopDetailsDto, LoadStopServiceDto } from "../../../../../../core/models/loads/load-details.dto";
import { LoadsService } from "../../../../../../data-access/loads/loads.service";
import { AuthFacade } from "../../../../../../core/auth/auth.facade";
import { Permission } from "../../../../../../core/auth/permissions/permission.enum";

@Component({
  selector: 'app-load-stops',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './load-stops.component.html',
  styleUrl: '../load-tab-shared.css'
})
export class LoadStopsComponent {
  @Input({ required: true }) loadId!: string;
  @Input() loadStatus?: number | string;
  @Input() stops: LoadStopDetailsDto[] = [];
  @Output() changed = new EventEmitter<void>();
  editing = false;
  editingId?: string;
  expandedStopId?: string;
  servicesByStop: Record<string, LoadStopServiceDto[]> = {};
  serviceForm = this.emptyServiceForm();
  form = this.emptyForm();

  readonly stopTypeOptions = [
    { label: 'Pickup', value: StopType.Pickup },
    { label: 'Delivery', value: StopType.Delivery },
    { label: 'Transload', value: StopType.Transload },
    { label: 'Storage', value: StopType.Storage }
  ];

  readonly stopServiceOptions = [
    { label: 'Pickup appointment', key: '21323' },
    { label: 'Delivery appointment', key: '21407' },
    { label: 'Lumper', key: '21320' },
    { label: 'Detention', key: '21308' },
    { label: 'Liftgate', key: '21303' },
    { label: 'Driver assist', key: '21312' },
    { label: 'Limited access', key: '21318' },
    { label: 'After hours', key: '21307' },
    { label: 'Inside pickup', key: '21302' },
    { label: 'Inside delivery', key: '21402' },
    { label: 'Refrigerated', key: '21807' },
    { label: 'Other', key: 'service.other' }
  ];

  readonly appointmentOptions = [
    { label: 'Appointment', value: AppointmentType.Appointment },
    { label: 'FCFS', value: AppointmentType.FCFS }
  ];

  readonly appointmentStatusOptions = [
    { label: 'Pending', value: AppointmentStatus.Pending },
    { label: 'Requested', value: AppointmentStatus.Requested },
    { label: 'Confirmed', value: AppointmentStatus.Confirmed },
    { label: 'Rescheduled', value: AppointmentStatus.Rescheduled },
    { label: 'Cancelled', value: AppointmentStatus.Cancelled },
    { label: 'Missed', value: AppointmentStatus.Missed }
  ];

  constructor(
    private loadsService: LoadsService,
    private toastr: ToastrService,
    private auth: AuthFacade
  ) {}

  stopTypeLabel(value: number | string) {
    if (typeof value === 'string') return value;
    return StopType[value] ?? String(value);
  }

  statusLabel(value: number | string) {
    if (typeof value === 'string') return this.humanize(value);
    return this.humanize(StopStatus[value] ?? String(value));
  }

  appointmentLabel(value: number | string) {
    if (typeof value === 'string') return value;
    return AppointmentType[value] ?? String(value);
  }

  appointmentStatusLabel(value?: number | string | null) {
    if (value == null) return 'Pending';
    if (typeof value === 'string') return this.humanize(value);
    return this.humanize(AppointmentStatus[value] ?? String(value));
  }

  canMarkEnroute(stop: LoadStopDetailsDto) {
    return !this.isCompletedLoad() && this.canUpdateExecution() && this.statusKey(stop.status) === 'pending';
  }

  canMarkArrive(stop: LoadStopDetailsDto) {
    return !this.isCompletedLoad() && this.canUpdateExecution() && this.statusKey(stop.status) === 'enroute';
  }

  canMarkLoaded(stop: LoadStopDetailsDto) {
    return !this.isCompletedLoad() && this.canUpdateExecution() && this.stopTypeKey(stop.stopType) === 'pickup' && this.statusKey(stop.status) === 'arrived';
  }

  canMarkUnloaded(stop: LoadStopDetailsDto) {
    return !this.isCompletedLoad() && this.canUpdateExecution() && this.stopTypeKey(stop.stopType) === 'delivery' && this.statusKey(stop.status) === 'arrived';
  }

  mark(stopId: string, action: 'enroute' | 'arrive' | 'loaded' | 'unloaded') {
    this.loadsService.markStop(stopId, action).subscribe({
      next: () => {
        this.toastr.success("Stop updated");
        this.changed.emit();
      },
      error: err => this.toastr.error(this.errorMessage(err), "Failed to update stop")
    });
  }

  openCreate() {
    if (!this.canEditStops()) return;
    this.editing = true;
    this.editingId = undefined;
    this.form = this.emptyForm();
    this.form.sequence = this.nextSequence();
  }

  openEdit(stop: LoadStopDetailsDto) {
    if (!this.canEditStops()) return;
    this.editing = true;
    this.editingId = stop.id;
    this.form = {
      stopType: this.toStopTypeValue(stop.stopType),
      sequence: stop.sequence,
      locationName: stop.locationName || '',
      addressLine1: stop.addressLine1 || '',
      addressLine2: stop.addressLine2 || '',
      city: stop.city || '',
      state: stop.state || '',
      postalCode: stop.postalCode || '',
      country: stop.country || '',
      plannedArrivalFrom: this.toInputDate(stop.plannedArrivalFrom),
      plannedArrivalTo: this.toInputDate(stop.plannedArrivalTo),
      plannedDepartureFrom: '',
      plannedDepartureTo: '',
      appointmentType: this.toAppointmentValue(stop.appointmentType),
      flexMinutes: stop.flexMinutes ?? null,
      timeZone: stop.timeZone || 'America/Chicago',
      appointmentStatus: this.toAppointmentStatusValue(stop.appointmentStatus),
      appointmentConfirmed: !!stop.appointmentConfirmed,
      appointmentConfirmationNumber: stop.appointmentConfirmationNumber || '',
      appointmentNumber: stop.appointmentNumber || '',
      stopReference: stop.stopReference || '',
      poNumbers: stop.poNumbers || '',
      notes: stop.notes || ''
    };
  }

  cancelEdit() {
    this.editing = false;
    this.editingId = undefined;
    this.form = this.emptyForm();
  }

  saveStop() {
    if (!this.canEditStops()) return;
    const payload = {
      ...this.form,
      plannedArrivalFrom: this.form.plannedArrivalFrom || null,
      plannedArrivalTo: this.form.plannedArrivalTo || null,
      plannedDepartureFrom: this.form.plannedDepartureFrom || null,
      plannedDepartureTo: this.form.plannedDepartureTo || null,
      flexMinutes: this.form.flexMinutes ?? null,
      timeZone: this.form.timeZone || 'UTC',
      appointmentStatus: this.form.appointmentStatus,
      appointmentConfirmed: this.form.appointmentConfirmed,
      appointmentConfirmationNumber: this.form.appointmentConfirmationNumber || null,
      appointmentNumber: this.form.appointmentNumber || null,
      stopReference: this.form.stopReference || '',
      poNumbers: this.form.poNumbers || null,
      notes: this.form.notes || null
    };

    const request = this.editingId
      ? this.loadsService.updateStop(this.loadId, this.editingId, payload)
      : this.loadsService.createStop(this.loadId, payload);

    request.subscribe({
      next: () => {
        this.toastr.success(this.editingId ? "Stop updated" : "Stop added");
        this.cancelEdit();
        this.changed.emit();
      },
      error: err => this.toastr.error(this.errorMessage(err), "Failed to save stop")
    });
  }

  deleteStop(stop: LoadStopDetailsDto) {
    if (!this.canEditStops()) return;
    if (!confirm("Delete this stop?")) return;
    this.loadsService.deleteStop(this.loadId, stop.id).subscribe({
      next: () => {
        this.toastr.success("Stop deleted");
        this.changed.emit();
      },
      error: err => this.toastr.error(this.errorMessage(err), "Failed to delete stop")
    });
  }

  toggleServices(stop: LoadStopDetailsDto) {
    this.expandedStopId = this.expandedStopId === stop.id ? undefined : stop.id;
    if (this.expandedStopId && !this.servicesByStop[stop.id]) {
      this.loadServices(stop.id);
    }
  }

  loadServices(stopId: string) {
    this.loadsService.getStopServices(stopId).subscribe({
      next: res => (this.servicesByStop[stopId] = res),
      error: err => this.toastr.error(this.errorMessage(err), "Failed to load stop services")
    });
  }

  addService(stop: LoadStopDetailsDto) {
    if (!this.canAddServices() || !this.serviceForm.serviceValue.trim()) return;
    const option = this.stopServiceOptions.find(x => x.label === this.serviceForm.serviceValue);
    const payload = {
      serviceKey: option?.key || 'service.other',
      serviceValue: this.serviceForm.serviceValue.trim(),
      notes: this.serviceForm.notes || null,
      isPickupService: this.stopTypeKey(stop.stopType) === 'pickup',
      isDeliveryService: this.stopTypeKey(stop.stopType) === 'delivery'
    };

    this.loadsService.createStopService(stop.id, payload).subscribe({
      next: () => {
        this.toastr.success("Stop service added");
        this.serviceForm = this.emptyServiceForm();
        this.loadServices(stop.id);
      },
      error: err => this.toastr.error(this.errorMessage(err), "Failed to add stop service")
    });
  }

  deleteService(stopId: string, serviceId: string) {
    if (!this.canDeleteServices()) return;
    this.loadsService.deleteStopService(stopId, serviceId).subscribe({
      next: () => {
        this.toastr.success("Stop service removed");
        this.loadServices(stopId);
      },
      error: err => this.toastr.error(this.errorMessage(err), "Failed to remove stop service")
    });
  }

  isCompletedLoad() {
    if (this.loadStatus == null) return false;
    if (typeof this.loadStatus === 'string') return this.loadStatus.replace(/\s+/g, '').toLowerCase() === 'completed';
    return LoadStatus[this.loadStatus]?.toLowerCase() === 'completed';
  }

  canEditStops() {
    if (!this.hasAny(Permission.Load_Update)) return false;
    return !this.isCompletedLoad() || this.hasAny(Permission.Load_CompletedCorrection);
  }

  canViewServices() {
    return this.hasAny(Permission.LoadStopService_View);
  }

  canAddServices() {
    if (!this.hasAny(Permission.LoadStopService_Create)) return false;
    return !this.isCompletedLoad() || this.hasAny(Permission.Load_CompletedCorrection);
  }

  canDeleteServices() {
    if (!this.hasAny(Permission.LoadStopService_Delete)) return false;
    return !this.isCompletedLoad() || this.hasAny(Permission.Load_CompletedCorrection);
  }

  hasStopActions(stop: LoadStopDetailsDto) {
    return this.canMarkEnroute(stop) || this.canMarkArrive(stop) || this.canMarkLoaded(stop) || this.canMarkUnloaded(stop);
  }

  private statusKey(value: number | string) {
    if (typeof value === 'string') return value.replace(/\s+/g, '').toLowerCase();
    return String(StopStatus[value] ?? value).replace(/\s+/g, '').toLowerCase();
  }

  private stopTypeKey(value: number | string) {
    if (typeof value === 'string') return value.replace(/\s+/g, '').toLowerCase();
    return String(StopType[value] ?? value).replace(/\s+/g, '').toLowerCase();
  }

  private emptyForm() {
    return {
      stopType: StopType.Delivery,
      sequence: 1,
      locationName: '',
      addressLine1: '',
      addressLine2: '',
      city: '',
      state: '',
      postalCode: '',
      country: '',
      plannedArrivalFrom: '',
      plannedArrivalTo: '',
      plannedDepartureFrom: '',
      plannedDepartureTo: '',
      appointmentType: AppointmentType.Appointment,
      flexMinutes: null as number | null,
      timeZone: 'America/Chicago',
      appointmentStatus: AppointmentStatus.Pending,
      appointmentConfirmed: false,
      appointmentConfirmationNumber: '',
      appointmentNumber: '',
      stopReference: '',
      poNumbers: '',
      notes: ''
    };
  }

  private emptyServiceForm() {
    return {
      serviceValue: '',
      notes: ''
    };
  }

  private nextSequence() {
    return this.stops.length ? Math.max(...this.stops.map(s => s.sequence)) + 1 : 1;
  }

  private toStopTypeValue(value: number | string) {
    if (typeof value === 'number') return value;
    return (StopType as any)[value] ?? StopType.Delivery;
  }

  private toAppointmentValue(value: number | string) {
    if (typeof value === 'number') return value;
    return (AppointmentType as any)[value] ?? AppointmentType.Appointment;
  }

  private toAppointmentStatusValue(value?: number | string | null) {
    if (typeof value === 'number') return value;
    if (!value) return AppointmentStatus.Pending;
    return (AppointmentStatus as any)[value] ?? AppointmentStatus.Pending;
  }

  private toInputDate(value?: string | null) {
    if (!value) return '';
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return '';
    const pad = (n: number) => String(n).padStart(2, '0');
    return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}`;
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

  private canUpdateExecution() {
    return this.hasAny(Permission.Load_Tracking_Update, Permission.Load_ChangeStatus);
  }
}
