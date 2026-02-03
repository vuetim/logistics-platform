import { Component, OnInit } from '@angular/core';
import { BaseCrudTabComponent } from '../../../../../../../shared/crud/customer-base-crud-tab.component';
import { CustomerNoteDto } from '../../../../../../../core/models/customers/notes/customer-note.dto';
import { CreateCustomerNoteDto } from '../../../../../../../core/models/customers/notes/create-customer-note.dto';
import { UpdateCustomerNoteDto } from '../../../../../../../core/models/customers/notes/update-customer-note.dto';
import { AuthFacade } from '../../../../../../../core/auth/auth.facade';
import { CustomerNotesService } from '../../../../../../../data-access/customers/notes/notes.service';
import { UiButtonComponent } from '../../../../../../../shared/UI/ui-button/ui-button.component';
import { NgFor, NgIf } from '@angular/common';
import { CreateCustomerNoteComponent } from './create-customer-note/create-customer-note.component';

@Component({
  selector: 'app-customer-notes',
  standalone: true,
  imports: [UiButtonComponent, NgIf, NgFor, CreateCustomerNoteComponent],
  templateUrl: './customer-notes.component.html',
  styleUrl: './customer-notes.component.css'
})
export class CustomerNotesComponent extends BaseCrudTabComponent<
  CustomerNoteDto,
  CreateCustomerNoteDto,
  UpdateCustomerNoteDto
>
  implements OnInit {

  constructor(
    auth: AuthFacade,
    private service: CustomerNotesService
  ) {
    super(auth, {
      view: 'Customer_View',
      create: 'Customer_Create',
      update: 'Customer_Update',
      delete: 'Customer_Delete'
    });
  }

  ngOnInit() {
    this.load();
  }

  protected fetch(customerId: string) {
    this.service.getByCustomer(customerId)
      .subscribe(res => this.items = res);

  }

  protected create(dto: CreateCustomerNoteDto) {
    this.service.create(dto)
      .subscribe(() => this.onModalClose(true));
  }

  protected update(id: string, dto: UpdateCustomerNoteDto) {
    this.service.update(id, dto)
      .subscribe(() => this.onModalClose(true));
  }

  protected remove(id: string) {
    this.service.delete(id)
      .subscribe(() => this.load());
  }
}
