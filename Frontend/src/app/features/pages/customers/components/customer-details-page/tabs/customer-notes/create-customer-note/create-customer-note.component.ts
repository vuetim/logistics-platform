import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CreateCustomerNoteDto } from '../../../../../../../../core/models/customers/notes/create-customer-note.dto';
import { CustomerNoteDto } from '../../../../../../../../core/models/customers/notes/customer-note.dto';
import { UpdateCustomerNoteDto } from '../../../../../../../../core/models/customers/notes/update-customer-note.dto';
import { CustomerNotesService } from '../../../../../../../../data-access/customers/notes/notes.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-create-customer-note',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-customer-note.component.html',
  styleUrls: ['./create-customer-note.component.css', '../../customer-addresses/create-address-modal/create-address-modal.component.css']
})
export class CreateCustomerNoteComponent {



  @Input({ required: true }) customerId!: string;
  @Input() editing?: CustomerNoteDto;

  @Output() close = new EventEmitter<boolean>();

  loading = false;

  model: CreateCustomerNoteDto = {
    customerId: '',
    title: '',
    message: '',



  };

  constructor(private service: CustomerNotesService) { }

  ngOnInit() {
    this.model.customerId = this.customerId;

    if (this.editing) {
      this.model = {
        customerId: this.customerId,
        title: this.editing.title,
        message: this.editing.message,

      };
    }
  }

  save() {
    if (!this.model.title || !this.model.message)
      return;

    this.loading = true;

    if (this.editing) {
      const dto: UpdateCustomerNoteDto = { ...this.model };
      this.service.update(this.editing.id, dto).subscribe({
        next: () => this.close.emit(true),
        error: () => this.loading = false
      });
    } else {
      this.service.create(this.model).subscribe({
        next: () => this.close.emit(true),
        error: () => this.loading = false
      });
    }
  }

  cancel() {
    this.close.emit(false);
  }
}
