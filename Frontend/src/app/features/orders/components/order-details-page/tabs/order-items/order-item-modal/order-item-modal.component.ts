import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { CreateOrderItemDto } from "../../../../../../../core/models/orders/order-items/create-order-item.dto";
import { OrderItemDto } from "../../../../../../../core/models/orders/order-items/order-item.model";
import { UpdateOrderItemDto } from "../../../../../../../core/models/orders/order-items/update-order-items.dto";
import { OrderItemsService } from "../../../../../../../data-access/orders/order-items/order-items.service";
@Component({
  selector: 'app-order-item-modal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './order-item-modal.component.html',
  styleUrl: './order-item-modal.component.css'
})
export class OrderItemModalComponent {
  @Input({ required: true }) orderId!: string;
  @Input() editing?: OrderItemDto;
  @Output() close = new EventEmitter<boolean>();

  loading = false;

  model: CreateOrderItemDto = {
    name: '',
    customerReference: '',
    quantity: 1,
    actualQuantity: 1,
    status: 'Active',
    quantityUnit: 'Pallets',
    handlingQuantity: 1,
    handlingUnit: 'Pallet',
    unitGrossWeight: undefined,
    weightUnit: 'Lb',
    length: undefined,
    width: undefined,
    height: undefined,
    dimensionUnit: 'In',
    volumeUnit: 'CuFt',
    minTemperature: undefined,
    maxTemperature: undefined,
    temperatureUnit: 'F',
    isHazmat: false,
    freightClass: '',
    hazardClass: '',
    identificationNumber: '',
    declaredValue: undefined,
    currency: 'USD',
    stackable: true,
    copyToLoad: true,
    notes: ''
  };

  constructor(private service: OrderItemsService) { }

  ngOnInit() {
    if (!this.editing) return;

    this.model = {
      name: this.editing.name,
      customerReference: this.editing.customerReference ?? '',
      quantity: this.editing.quantity,
      actualQuantity: this.editing.actualQuantity ?? this.editing.quantity,
      status: this.editing.status ?? 'Active',
      quantityUnit: this.editing.quantityUnit,
      handlingQuantity: this.editing.handlingQuantity ?? this.editing.actualQuantity ?? this.editing.quantity,
      handlingUnit: this.editing.handlingUnit ?? 'Pallet',
      unitNetWeight: this.editing.unitNetWeight ?? undefined,
      unitGrossWeight: this.editing.unitGrossWeight ?? undefined,
      weightUnit: this.editing.weightUnit ?? 'Lb',
      length: this.editing.length ?? undefined,
      width: this.editing.width ?? undefined,
      height: this.editing.height ?? undefined,
      dimensionUnit: this.editing.dimensionUnit ?? 'In',
      volume: this.editing.volume ?? undefined,
      volumeUnit: this.editing.volumeUnit ?? 'CuFt',
      minTemperature: this.editing.minTemperature ?? undefined,
      maxTemperature: this.editing.maxTemperature ?? undefined,
      temperatureUnit: this.editing.temperatureUnit ?? 'F',
      isHazmat: this.editing.isHazmat,
      freightClass: this.editing.freightClass ?? '',
      hazardClass: this.editing.hazardClass ?? '',
      identificationNumber: this.editing.identificationNumber ?? '',
      declaredValue: this.editing.declaredValue ?? undefined,
      currency: this.editing.currency ?? 'USD',
      stackable: this.editing.stackable ?? true,
      copyToLoad: this.editing.copyToLoad ?? true,
      notes: this.editing.notes ?? ''
    };
  }

  save() {
    if (!this.model.name || !this.model.quantity || !this.model.quantityUnit) return;

    this.loading = true;
    if (this.editing) {
      const dto: UpdateOrderItemDto = { ...this.model };
      this.service.update(this.orderId, this.editing.id, dto).subscribe({
        next: () => this.close.emit(true),
        error: () => this.loading = false
      });
      return;
    }

    this.service.create(this.orderId, this.model).subscribe({
      next: () => this.close.emit(true),
      error: () => this.loading = false
    });
  }

  cancel() {
    this.close.emit(false);
  }
}
