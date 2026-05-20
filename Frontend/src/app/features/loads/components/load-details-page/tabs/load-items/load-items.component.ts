import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, Output } from "@angular/core";
import { LoadItemDto } from "../../../../../../core/models/loads/load-details.dto";
import { LoadsService } from "../../../../../../data-access/loads/loads.service";
import { LoadItemModalComponent } from "./load-item-modal.component";

@Component({
  selector: 'app-load-items',
  standalone: true,
  imports: [CommonModule, LoadItemModalComponent],
  templateUrl: './load-items.component.html',
  styleUrl: '../load-tab-shared.css'
})
export class LoadItemsComponent {
  @Input({ required: true }) loadId!: string;
  @Input() items: LoadItemDto[] = [];
  @Output() changed = new EventEmitter<void>();

  editing?: LoadItemDto;
  busyItemId = '';

  constructor(private loads: LoadsService) {}

  edit(item: LoadItemDto) {
    this.editing = item;
  }

  closeEdit(dto: Partial<LoadItemDto> | null) {
    if (!this.editing) return;
    const item = this.editing;
    this.editing = undefined;
    if (!dto) return;

    this.busyItemId = item.id;
    this.loads.updateItem(this.loadId, item.id, dto).subscribe({
      next: () => {
        this.busyItemId = '';
        this.changed.emit();
      },
      error: () => {
        this.busyItemId = '';
      }
    });
  }

  remove(item: LoadItemDto) {
    if (!confirm(`Remove load item "${item.name}"?`)) return;
    this.busyItemId = item.id;
    this.loads.deleteItem(this.loadId, item.id).subscribe({
      next: () => {
        this.busyItemId = '';
        this.changed.emit();
      },
      error: () => {
        this.busyItemId = '';
      }
    });
  }
}
