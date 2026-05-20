import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, OnChanges, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { LoadItemDto } from "../../../../../../core/models/loads/load-details.dto";

@Component({
  selector: 'app-load-item-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="modal-backdrop" (click)="cancel()">
      <div class="modal" (click)="$event.stopPropagation()">
        <div class="modal-header">
          <h3>Edit Load Item</h3>
          <button type="button" (click)="cancel()">x</button>
        </div>

        <div class="modal-grid">
          <label>Handling qty<input type="number" min="0" [(ngModel)]="model.handlingQuantity" /></label>
          <label>Handling unit<input maxlength="40" [(ngModel)]="model.handlingUnit" /></label>
          <label>Net weight<input type="number" min="0" [(ngModel)]="model.unitNetWeight" /></label>
          <label>Gross weight<input type="number" min="0" [(ngModel)]="model.unitGrossWeight" /></label>
          <label>Weight unit<input maxlength="20" [(ngModel)]="model.weightUnit" /></label>
          <label>Length<input type="number" min="0" [(ngModel)]="model.length" /></label>
          <label>Width<input type="number" min="0" [(ngModel)]="model.width" /></label>
          <label>Height<input type="number" min="0" [(ngModel)]="model.height" /></label>
          <label>Dimension unit<input maxlength="20" [(ngModel)]="model.dimensionUnit" /></label>
          <label>Volume<input type="number" min="0" [(ngModel)]="model.volume" /></label>
          <label>Volume unit<input maxlength="20" [(ngModel)]="model.volumeUnit" /></label>
          <label>Min temp<input type="number" [(ngModel)]="model.minTemperature" /></label>
          <label>Max temp<input type="number" [(ngModel)]="model.maxTemperature" /></label>
          <label>Temp unit<input maxlength="20" [(ngModel)]="model.temperatureUnit" /></label>
          <label>Declared value<input type="number" min="0" [(ngModel)]="model.declaredValue" /></label>
          <label>Currency<input maxlength="10" [(ngModel)]="model.currency" /></label>
          <label class="check"><input type="checkbox" [(ngModel)]="model.stackable" /> Stackable</label>
          <label class="full">Notes<textarea maxlength="1000" [(ngModel)]="model.notes"></textarea></label>
        </div>

        <div class="modal-actions">
          <button type="button" (click)="cancel()">Cancel</button>
          <button type="button" class="primary" (click)="save()">Save</button>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .modal-backdrop {
      position: fixed;
      inset: 0;
      z-index: 3010;
      display: grid;
      place-items: center;
      padding: 20px;
      background: rgba(15, 23, 42, 0.35);
    }

    .modal {
      width: min(820px, calc(100vw - 40px));
      max-height: calc(100vh - 40px);
      overflow: auto;
      border: 1px solid #dbe4ef;
      border-radius: 8px;
      background: #ffffff;
      box-shadow: 0 24px 48px rgba(15, 23, 42, 0.18);
    }

    .modal-header {
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 14px 16px;
      border-bottom: 1px solid #e5e7eb;
    }

    .modal-header h3 {
      margin: 0;
      font-size: 16px;
    }

    .modal-header button,
    .modal-actions button {
      border: 1px solid #cbd5e1;
      border-radius: 8px;
      background: #ffffff;
      padding: 8px 12px;
      font-weight: 700;
    }

    .modal-grid {
      display: grid;
      grid-template-columns: repeat(3, minmax(160px, 1fr));
      gap: 10px;
      padding: 16px;
    }

    label {
      display: flex;
      flex-direction: column;
      gap: 5px;
      color: #475569;
      font-size: 12px;
      font-weight: 700;
    }

    input,
    textarea {
      border: 1px solid #cbd5e1;
      border-radius: 8px;
      padding: 9px 10px;
    }

    textarea {
      min-height: 80px;
      resize: vertical;
    }

    .check {
      justify-content: end;
      flex-direction: row;
      align-items: center;
    }

    .full {
      grid-column: 1 / -1;
    }

    .modal-actions {
      display: flex;
      justify-content: flex-end;
      gap: 8px;
      padding: 12px 16px;
      border-top: 1px solid #e5e7eb;
    }

    .modal-actions .primary {
      border-color: #2563eb;
      background: #2563eb;
      color: #ffffff;
    }

    @media (max-width: 760px) {
      .modal-grid {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class LoadItemModalComponent implements OnChanges {
  @Input({ required: true }) item!: LoadItemDto;
  @Output() close = new EventEmitter<Partial<LoadItemDto> | null>();

  model: Partial<LoadItemDto> = {};

  ngOnChanges() {
    this.model = {
      handlingQuantity: this.item.handlingQuantity ?? null,
      handlingUnit: this.item.handlingUnit ?? '',
      unitNetWeight: this.item.unitNetWeight ?? null,
      unitGrossWeight: this.item.unitGrossWeight ?? null,
      weightUnit: this.item.weightUnit ?? '',
      length: this.item.length ?? null,
      width: this.item.width ?? null,
      height: this.item.height ?? null,
      dimensionUnit: this.item.dimensionUnit ?? '',
      volume: this.item.volume ?? null,
      volumeUnit: this.item.volumeUnit ?? '',
      minTemperature: this.item.minTemperature ?? null,
      maxTemperature: this.item.maxTemperature ?? null,
      temperatureUnit: this.item.temperatureUnit ?? '',
      declaredValue: this.item.declaredValue ?? null,
      currency: this.item.currency ?? '',
      stackable: this.item.stackable ?? true,
      notes: this.item.notes ?? ''
    };
  }

  cancel() {
    this.close.emit(null);
  }

  save() {
    this.close.emit(this.model);
  }
}
