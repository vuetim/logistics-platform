import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ToastrService } from "ngx-toastr";
import { StopType } from "../../../../../../core/enums/orders/stop-type.enum";
import { StopStatus } from "../../../../../../core/enums/loads/stop-status.enum";
import { AppointmentType } from "../../../../../../core/enums/loads/appointment-type.enum";
import { LoadStopDetailsDto } from "../../../../../../core/models/loads/load-details.dto";
import { LoadsService } from "../../../../../../data-access/loads/loads.service";

@Component({
  selector: 'app-load-stops',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './load-stops.component.html',
  styleUrl: '../load-tab-shared.css'
})
export class LoadStopsComponent {
  @Input({ required: true }) loadId!: string;
  @Input() stops: LoadStopDetailsDto[] = [];
  @Output() changed = new EventEmitter<void>();
  editing = false;
  editingId?: string;
  form = this.emptyForm();

  readonly stopTypeOptions = [
    { label: 'Pickup', value: StopType.Pickup },
    { label: 'Delivery', value: StopType.Delivery },
    { label: 'Transload', value: StopType.Transload },
    { label: 'Storage', value: StopType.Storage }
  ];

  readonly appointmentOptions = [
    { label: 'Appointment', value: AppointmentType.Appointment },
    { label: 'FCFS', value: AppointmentType.FCFS }
  ];

  constructor(private loadsService: LoadsService, private toastr: ToastrService) {}

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

  canMarkEnroute(stop: LoadStopDetailsDto) {
    return this.statusKey(stop.status) === 'pending';
  }

  canMarkArrive(stop: LoadStopDetailsDto) {
    return this.statusKey(stop.status) === 'enroute';
  }

  canMarkLoaded(stop: LoadStopDetailsDto) {
    return this.stopTypeKey(stop.stopType) === 'pickup' && this.statusKey(stop.status) === 'arrived';
  }

  canMarkUnloaded(stop: LoadStopDetailsDto) {
    return this.stopTypeKey(stop.stopType) === 'delivery' && this.statusKey(stop.status) === 'arrived';
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
    this.editing = true;
    this.editingId = undefined;
    this.form = this.emptyForm();
    this.form.sequence = this.nextSequence();
  }

  openEdit(stop: LoadStopDetailsDto) {
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
      appointmentNumber: stop.appointmentNumber || '',
      stopReference: stop.stopReference || '',
      notes: stop.notes || ''
    };
  }

  cancelEdit() {
    this.editing = false;
    this.editingId = undefined;
    this.form = this.emptyForm();
  }

  saveStop() {
    const payload = {
      ...this.form,
      plannedArrivalFrom: this.form.plannedArrivalFrom || null,
      plannedArrivalTo: this.form.plannedArrivalTo || null,
      plannedDepartureFrom: this.form.plannedDepartureFrom || null,
      plannedDepartureTo: this.form.plannedDepartureTo || null,
      flexMinutes: this.form.flexMinutes ?? null,
      appointmentNumber: this.form.appointmentNumber || null,
      stopReference: this.form.stopReference || '',
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
    if (!confirm("Delete this stop?")) return;
    this.loadsService.deleteStop(this.loadId, stop.id).subscribe({
      next: () => {
        this.toastr.success("Stop deleted");
        this.changed.emit();
      },
      error: err => this.toastr.error(this.errorMessage(err), "Failed to delete stop")
    });
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
      appointmentNumber: '',
      stopReference: '',
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
}
