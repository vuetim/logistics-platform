import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { TemperatureUnit } from "../../../../../../../core/enums/orders/temperature-unit.enum";
import { WeightUnit } from "../../../../../../../core/enums/orders/weight-unit.enum";
import { enumToOptions } from "../../../../../../../core/utils/enum-options";
import { CreateOrderEquipmentRequirementDto } from "../../../../../../../core/models/orders/order-equipment/create-order-equipment-requirement.dto";
import { OrderEquipmentRequirementDto } from "../../../../../../../core/models/orders/order-equipment/order-equipment-requirement.model";
import { UpdateOrderEquipmentRequirementDto } from "../../../../../../../core/models/orders/order-equipment/update-order-equipment-requirement.dto";
import { OrderEquipmentService } from "../../../../../../../data-access/orders/order-equipment/order-equipment.service";

@Component({
  selector: 'app-order-equipment-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './order-equipment-modal.component.html',
  styleUrl: './order-equipment-modal.component.css'
})
export class OrderEquipmentModalComponent {
  @Input({ required: true }) orderId!: string;
  @Input() editing?: OrderEquipmentRequirementDto;
  @Output() close = new EventEmitter<boolean>();

  loading = false;
  weightUnitOptions = enumToOptions(WeightUnit);
  temperatureUnitOptions = enumToOptions(TemperatureUnit);

  model: CreateOrderEquipmentRequirementDto = {
    equipmentType: '',
    equipmentSize: '',
    maxWeight: null,
    weightUnit: WeightUnit.Lb,
    minTemperature: 0,
    maxTemperature: 0,
    temperatureUnit: TemperatureUnit.F,
    quantity: 1,
    isMandatory: true,
    copyToLoad: true,
    isPrefered: false,
    notes: ''
  };

  constructor(private service: OrderEquipmentService) { }

  ngOnInit() {
    if (!this.editing) return;

    this.model = {
      equipmentType: this.editing.equipmentType,
      equipmentSize: this.editing.equipmentSize ?? '',
      maxWeight: this.editing.maxWeight ?? null,
      weightUnit: this.normalizeWeightUnit(this.editing.weightUnit),
      minTemperature: this.editing.minTemperature ?? 0,
      maxTemperature: this.editing.maxTemperature ?? 0,
      temperatureUnit: this.normalizeTemperatureUnit(this.editing.temperatureUnit),
      quantity: this.editing.quantity,
      isMandatory: this.editing.isMandatory,
      copyToLoad: this.editing.copyToLoad,
      isPrefered: this.editing.isPrefered ?? false,
      notes: this.editing.notes ?? ''
    };
  }

  save() {
    if (!this.model.equipmentType || !this.model.quantity) return;

    this.loading = true;

    if (this.editing) {
      const dto: UpdateOrderEquipmentRequirementDto = { ...this.model };
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

  private normalizeWeightUnit(value: number | string | null | undefined): WeightUnit {
    if (typeof value === 'number') return value as WeightUnit;
    if (typeof value === 'string') {
      const numeric = Number(value);
      if (!Number.isNaN(numeric)) return numeric as WeightUnit;
      const fromEnum = (WeightUnit as Record<string, unknown>)[value];
      if (typeof fromEnum === 'number') return fromEnum as WeightUnit;
    }
    return WeightUnit.Lb;
  }

  private normalizeTemperatureUnit(value: number | string | null | undefined): TemperatureUnit {
    if (typeof value === 'number') return value as TemperatureUnit;
    if (typeof value === 'string') {
      const numeric = Number(value);
      if (!Number.isNaN(numeric)) return numeric as TemperatureUnit;
      const fromEnum = (TemperatureUnit as Record<string, unknown>)[value];
      if (typeof fromEnum === 'number') return fromEnum as TemperatureUnit;
    }
    return TemperatureUnit.F;
  }
}
