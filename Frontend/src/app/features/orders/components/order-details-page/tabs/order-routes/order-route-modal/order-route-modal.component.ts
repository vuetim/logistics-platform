import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { StopType } from "../../../../../../../core/enums/orders/stop-type.enum";
import { AppointmentType } from "../../../../../../../core/enums/loads/appointment-type.enum";
import { enumToOptions } from "../../../../../../../core/utils/enum-options";
import { CreateOrderRouteDto } from "../../../../../../../core/models/orders/order-routes/create-order-route.dto";
import { OrderRouteDto } from "../../../../../../../core/models/orders/order-routes/order-route.model";
import { UpdateOrderRouteDto } from "../../../../../../../core/models/orders/order-routes/update-order-route.dto";
import { OrderRoutesService } from "../../../../../../../data-access/orders/order-routes/order-routes.service";
import { GeocodingResult, GeocodingService } from "../../../../../../../data-access/geocoding/geocoding.service";

@Component({
  selector: 'app-order-route-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './order-route-modal.component.html',
  styleUrl: './order-route-modal.component.css'
})
export class OrderRouteModalComponent {
  @Input({ required: true }) orderId!: string;
  @Input() editing?: OrderRouteDto;
  @Output() close = new EventEmitter<boolean>();

  loading = false;
  geocoding = false;
  geocodeQuery = '';
  geocodeResults: GeocodingResult[] = [];
  geocodeMessage = '';
  stopTypeOptions = enumToOptions(StopType);
  appointmentTypeOptions = enumToOptions(AppointmentType);

  model: CreateOrderRouteDto = {
    sequence: 1,
    stopType: StopType.Pickup,
    locationName: '',
    addressLine1: '',
    addressLine2: '',
    city: '',
    state: '',
    postalCode: '',
    country: '',
    latitude: null,
    longitude: null,
    plannedArrivalFrom: null,
    plannedArrivalTo: null,
    appointmentType: AppointmentType.Appointment,
    flexMinutes: null,
    timeZone: 'America/Chicago',
    hasTime: true,
    copyToLoad: true,
    stopReference: '',
    appointmentNumber: '',
    poNumbers: '',
    notes: ''
  };

  constructor(
    private service: OrderRoutesService,
    private geocodingService: GeocodingService
  ) { }

  ngOnInit() {
    if (!this.editing) return;

    this.model = {
      sequence: this.editing.sequence,
      stopType: this.normalizeStopType(this.editing.stopType),
      locationName: this.editing.locationName,
      addressLine1: this.editing.addressLine1 ?? '',
      addressLine2: this.editing.addressLine2 ?? '',
      city: this.editing.city,
      state: this.editing.state,
      postalCode: this.editing.postalCode ?? '',
      country: this.editing.country,
      latitude: this.editing.latitude ?? null,
      longitude: this.editing.longitude ?? null,
      plannedArrivalFrom: this.toInputDate(this.editing.plannedArrivalFrom),
      plannedArrivalTo: this.toInputDate(this.editing.plannedArrivalTo),
      appointmentType: this.editing.appointmentType ?? AppointmentType.Appointment,
      flexMinutes: this.editing.flexMinutes ?? null,
      timeZone: this.editing.timeZone ?? 'America/Chicago',
      hasTime: this.editing.hasTime,
      copyToLoad: this.editing.copyToLoad,
      stopReference: this.editing.stopReference ?? '',
      appointmentNumber: this.editing.appointmentNumber ?? '',
      poNumbers: this.editing.poNumbers ?? '',
      notes: this.editing.notes ?? ''
    };
  }

  save() {
    if (!this.model.locationName || !this.model.city || !this.model.country) return;

    this.loading = true;

    if (this.editing) {
      const dto: UpdateOrderRouteDto = {
        ...this.model,
        plannedArrivalFrom: this.toApiDate(this.model.plannedArrivalFrom),
        plannedArrivalTo: this.toApiDate(this.model.plannedArrivalTo)
      };

      this.service.update(this.orderId, this.editing.id, dto).subscribe({
        next: () => this.close.emit(true),
        error: () => this.loading = false
      });

      return;
    }

    const createDto: CreateOrderRouteDto = {
      ...this.model,
      plannedArrivalFrom: this.toApiDate(this.model.plannedArrivalFrom),
      plannedArrivalTo: this.toApiDate(this.model.plannedArrivalTo)
    };

    this.service.create(this.orderId, createDto).subscribe({
      next: () => this.close.emit(true),
      error: () => this.loading = false
    });
  }

  cancel() {
    this.close.emit(false);
  }

  searchLocation() {
    const query = this.geocodeQuery.trim()
      || [this.model.locationName, this.model.addressLine1, this.model.city, this.model.state, this.model.postalCode, this.model.country]
        .filter(Boolean)
        .join(', ');

    if (!query.trim()) {
      this.geocodeMessage = 'Enter a location or address to search.';
      return;
    }

    this.geocoding = true;
    this.geocodeMessage = '';
    this.geocodingService.search(query).subscribe({
      next: results => {
        this.geocoding = false;
        this.geocodeResults = results;
        this.geocodeMessage = results.length ? '' : 'No geocoding results found.';
      },
      error: () => {
        this.geocoding = false;
        this.geocodeMessage = 'Geocoding failed. You can enter coordinates manually.';
      }
    });
  }

  useGeocode(result: GeocodingResult) {
    this.model.latitude = result.latitude;
    this.model.longitude = result.longitude;
    this.model.locationName = this.model.locationName || result.label.split(',')[0] || '';
    this.model.addressLine1 = result.addressLine1 || this.model.addressLine1;
    this.model.city = result.city || this.model.city;
    this.model.state = result.state || this.model.state;
    this.model.postalCode = result.postalCode || this.model.postalCode;
    this.model.country = result.country || this.model.country || 'United States';
    this.geocodeQuery = result.label;
    this.geocodeResults = [];
  }

  private toInputDate(value?: string | null) {
    if (!value) return null;
    return value.slice(0, 16);
  }

  private toApiDate(value?: string | null) {
    return value?.trim() ? new Date(value).toISOString() : null;
  }

  private normalizeStopType(value: number | string | null | undefined): StopType {
    if (typeof value === 'number') return value as StopType;

    if (typeof value === 'string') {
      const numeric = Number(value);
      if (!Number.isNaN(numeric)) return numeric as StopType;

      const fromEnum = (StopType as Record<string, unknown>)[value];
      if (typeof fromEnum === 'number') return fromEnum as StopType;
    }

    return StopType.Pickup;
  }
}
