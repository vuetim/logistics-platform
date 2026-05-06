import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { CreateOrderNoteDto } from "../../../../../../../core/models/orders/order-notes/create-order-note.dto";
import { OrderNoteDto } from "../../../../../../../core/models/orders/order-notes/order-note.model";
import { UpdateOrderNoteDto } from "../../../../../../../core/models/orders/order-notes/update-order-note.dto";
import { OrderNotesService } from "../../../../../../../data-access/orders/order-notes/order-notes.service";

@Component({
  selector: 'app-order-note-modal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './order-note-modal.component.html',
  styleUrl: './order-note-modal.component.css'
})
export class OrderNoteModalComponent {
  @Input({ required: true }) orderId!: string;
  @Input() editing?: OrderNoteDto;
  @Output() close = new EventEmitter<boolean>();

  loading = false;

  model: CreateOrderNoteDto = {
    message: '',
    isInternal: false
  };

  constructor(private service: OrderNotesService) { }

  ngOnInit() {
    if (!this.editing) return;
    this.model = {
      message: this.editing.message,
      isInternal: this.editing.isInternal
    };
  }

  save() {
    if (!this.model.message?.trim()) return;
    this.loading = true;

    if (this.editing) {
      const dto: UpdateOrderNoteDto = { ...this.model };
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
