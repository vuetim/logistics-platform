import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { StopType } from "../../../../../../../core/enums/orders/stop-type.enum";
import { enumToOptions } from "../../../../../../../core/utils/enum-options";
import { CreateOrderRouteDto } from "../../../../../../../core/models/orders/order-routes/create-order-route.dto";
import { OrderRouteDto } from "../../../../../../../core/models/orders/order-routes/order-route.model";
import { UpdateOrderRouteDto } from "../../../../../../../core/models/orders/order-routes/update-order-route.dto";
import { OrderRoutesService } from "../../../../../../../data-access/orders/order-routes/order-routes.service";

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
  stopTypeOptions = enumToOptions(StopType);

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
    plannedArrivalFrom: null,
    plannedArrivalTo: null,
    hasTime: true,
    copyToLoad: true,
    stopReference: '',
    appointmentNumber: '',
    notes: ''
  };

  constructor(private service: OrderRoutesService) { }

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
      plannedArrivalFrom: this.toInputDate(this.editing.plannedArrivalFrom),
      plannedArrivalTo: this.toInputDate(this.editing.plannedArrivalTo),
      hasTime: this.editing.hasTime,
      copyToLoad: this.editing.copyToLoad,
      stopReference: this.editing.stopReference ?? '',
      appointmentNumber: this.editing.appointmentNumber ?? '',
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
