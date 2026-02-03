import { Component, EventEmitter, input, Input, Output } from '@angular/core';
import { CreateCustomerNoteDto } from '../../../../../../../core/models/customers/notes/create-customer-note.dto';
import { CommonModule, NgFor } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-step-notes',
  standalone: true,
  imports: [NgFor, FormsModule],
  templateUrl: './step-notes.component.html',
  styleUrls: ['../wizard.styles.css', './step-notes.component.css']
})
export class StepNotesComponent {

  @Input({ required: true })
  notes!: CreateCustomerNoteDto[];

  @Output() next = new EventEmitter<void>();
  @Output() back = new EventEmitter<void>();

  draft: CreateCustomerNoteDto = this.empty();

  empty(): CreateCustomerNoteDto {
    return { title: '', message: '' };
  }

  isValid(): boolean {
    return !!(this.draft.title && this.draft.message);
  }

  add() {
    if (!this.isValid()) return;

    this.notes.push({ ...this.draft });
    this.draft = this.empty();
  }

  remove(i: number) {
    this.notes.splice(i, 1);
  }
}