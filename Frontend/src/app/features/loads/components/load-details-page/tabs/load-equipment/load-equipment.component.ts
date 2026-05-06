import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { EquipmentType } from "../../../../../../core/enums/loads/equipment-type.enum";
import { TemperatureUnit } from "../../../../../../core/enums/orders/temperature-unit.enum";
import { WeightUnit } from "../../../../../../core/enums/orders/weight-unit.enum";
import { LoadEquipmentDto } from "../../../../../../core/models/loads/load-details.dto";

@Component({
  selector: 'app-load-equipment',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './load-equipment.component.html',
  styleUrl: '../load-tab-shared.css'
})
export class LoadEquipmentComponent {
  @Input() equipment: LoadEquipmentDto[] = [];

  equipmentLabel(value: number | string) {
    if (typeof value === 'string') return value;
    return EquipmentType[value] ?? String(value);
  }

  weightUnitLabel(value: number | string) {
    if (typeof value === 'string') return value;
    return WeightUnit[value] ?? String(value);
  }

  tempUnitLabel(value: number | string) {
    if (typeof value === 'string') return value;
    return TemperatureUnit[value] ?? String(value);
  }
}
