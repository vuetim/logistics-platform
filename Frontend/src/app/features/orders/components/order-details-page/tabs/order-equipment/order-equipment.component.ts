import { NgFor, NgIf } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { AuthFacade } from "../../../../../../core/auth/auth.facade";
import { TemperatureUnit } from "../../../../../../core/enums/orders/temperature-unit.enum";
import { WeightUnit } from "../../../../../../core/enums/orders/weight-unit.enum";
import { CreateOrderEquipmentRequirementDto } from "../../../../../../core/models/orders/order-equipment/create-order-equipment-requirement.dto";
import { OrderEquipmentRequirementDto } from "../../../../../../core/models/orders/order-equipment/order-equipment-requirement.model";
import { UpdateOrderEquipmentRequirementDto } from "../../../../../../core/models/orders/order-equipment/update-order-equipment-requirement.dto";
import { enumToOptions } from "../../../../../../core/utils/enum-options";
import { OrderEquipmentService } from "../../../../../../data-access/orders/order-equipment/order-equipment.service";
import { BaseEntityCrudTabComponent } from "../../../../../../shared/crud/base-entity-crud-tab.component";
import { UiButtonComponent } from "../../../../../../shared/UI/ui-button/ui-button.component";
import { OrderEquipmentModalComponent } from "./order-equipment-modal/order-equipment-modal.component";

@Component({
  selector: 'app-order-equipment',
  standalone: true,
  imports: [NgIf, NgFor, UiButtonComponent, OrderEquipmentModalComponent],
  templateUrl: './order-equipment.component.html',
  styleUrl: '../order-tab-shared.css'
})
export class OrderEquipmentComponent
  extends BaseEntityCrudTabComponent<
    OrderEquipmentRequirementDto,
    CreateOrderEquipmentRequirementDto,
    UpdateOrderEquipmentRequirementDto
  >
  implements OnInit {
  private readonly weightUnitLookup = new Map(enumToOptions(WeightUnit).map((x: { value: number; label: string }) => [x.value, x.label]));
  private readonly temperatureUnitLookup = new Map(enumToOptions(TemperatureUnit).map((x: { value: number; label: string }) => [x.value, x.label]));

  constructor(
    auth: AuthFacade,
    private service: OrderEquipmentService
  ) {
    super(auth, {
      view: 'Load_View',
      create: 'Load_Create',
      update: 'Load_Update',
      delete: 'Load_Archive'
    });
  }

  ngOnInit() {
    this.load();
  }

  protected fetch(orderId: string) {
    this.service.getByOrder(orderId).subscribe({
      next: res => this.finishLoad(res.map(e => ({
        ...e,
        weightUnit: this.normalizeEnumValue(e.weightUnit, WeightUnit.Lb),
        temperatureUnit: this.normalizeEnumValue(e.temperatureUnit, TemperatureUnit.F)
      }))),
      error: () => this.finishLoad([])
    });
  }

  protected create(dto: CreateOrderEquipmentRequirementDto) {
    this.service.create(this.parentId, dto).subscribe(() => this.finishSave());
  }

  protected update(id: string, dto: UpdateOrderEquipmentRequirementDto) {
    this.service.update(this.parentId, id, dto).subscribe(() => this.finishSave());
  }

  protected remove(id: string) {
    this.service.delete(this.parentId, id).subscribe(() => this.finishDelete());
  }

  weightUnitLabel(value?: number | string | null) {
    if (value == null) return '';
    const normalized = this.normalizeEnumValue(value, WeightUnit.Lb);
    return this.weightUnitLookup.get(normalized) ?? value;
  }

  temperatureUnitLabel(value?: number | string | null) {
    if (value == null) return '';
    const normalized = this.normalizeEnumValue(value, TemperatureUnit.F);
    return this.temperatureUnitLookup.get(normalized) ?? value;
  }

  private normalizeEnumValue(value: number | string | null | undefined, fallback: number): number {
    if (typeof value === 'number') return value;
    if (typeof value === 'string') {
      const numeric = Number(value);
      if (!Number.isNaN(numeric)) return numeric;

      const fromWeight = (WeightUnit as Record<string, unknown>)[value];
      if (typeof fromWeight === 'number') return fromWeight;

      const fromTemp = (TemperatureUnit as Record<string, unknown>)[value];
      if (typeof fromTemp === 'number') return fromTemp;
    }
    return fallback;
  }
}
