import { NgIf, NgFor } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { AuthFacade } from "../../../../../../../core/auth/auth.facade";
import { CreateCustomerContactDto } from "../../../../../../../core/models/customers/contacts/create-customer-contact.dto";
import { CustomerContactDto } from "../../../../../../../core/models/customers/contacts/customer-contact.dto";
import { UpdateCustomerContactDto } from "../../../../../../../core/models/customers/contacts/update-customer-contact.dto";
import { CustomerContactsService } from "../../../../../../../data-access/customers/contacts/contact.service";
import { BaseCrudTabComponent } from "../../../../../../../shared/crud/customer-base-crud-tab.component";
import { UiButtonComponent } from "../../../../../../../shared/UI/ui-button/ui-button.component";
import { CreateContactModalComponent } from "./create-contact-modal/create-contact-modal.component";

@Component({
  selector: 'app-customer-contacts',
  standalone: true,
  imports: [UiButtonComponent, NgIf, NgFor, CreateContactModalComponent],
  templateUrl: './customer-contacts.component.html'
})
export class CustomerContactsComponent
  extends BaseCrudTabComponent<
    CustomerContactDto,
    CreateCustomerContactDto,
    UpdateCustomerContactDto
  >
  implements OnInit {

  constructor(
    auth: AuthFacade,
    private service: CustomerContactsService
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

  protected create(dto: CreateCustomerContactDto) {
    this.service.create(dto)
      .subscribe(() => this.onModalClose(true));
  }

  protected update(id: string, dto: UpdateCustomerContactDto) {
    this.service.update(id, dto)
      .subscribe(() => this.onModalClose(true));
  }

  protected remove(id: string) {
    this.service.delete(id)
      .subscribe(() => this.load());
  }
}
