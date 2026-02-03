import { Component, OnInit } from "@angular/core";
import { AuthFacade } from "../../../../../../../core/auth/auth.facade";
import { CreateCustomerAddressDto } from "../../../../../../../core/models/customers/addresses/create-customer-address.dto";
import { CustomerAddressDto } from "../../../../../../../core/models/customers/addresses/customer-address.dto";
import { UpdateCustomerAddressDto } from "../../../../../../../core/models/customers/addresses/update-address.dto";
import { CustomerAddressesService } from "../../../../../../../data-access/customers/addresses/address.service";
import { BaseCrudTabComponent } from "../../../../../../../shared/crud/customer-base-crud-tab.component";
import { UiButtonComponent } from "../../../../../../../shared/UI/ui-button/ui-button.component";
import { CreateAddressModalComponent } from "./create-address-modal/create-address-modal.component";
import { NgFor, NgIf } from "@angular/common";


@Component({
  selector: 'app-customer-addresses',
  standalone: true,
  imports: [UiButtonComponent, CreateAddressModalComponent, NgIf, NgFor],
  templateUrl: './customer-addresses.component.html'
})
export class CustomerAddressesComponent
  extends BaseCrudTabComponent<
    CustomerAddressDto,
    CreateCustomerAddressDto,
    UpdateCustomerAddressDto
  >
  implements OnInit {

  constructor(
    auth: AuthFacade,
    private service: CustomerAddressesService
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

  protected create(dto: CreateCustomerAddressDto) {
    this.service.create(dto)
      .subscribe(() => this.onModalClose(true));
  }

  protected update(id: string, dto: UpdateCustomerAddressDto) {
    this.service.update(id, dto)
      .subscribe(() => this.onModalClose(true));
  }

  protected remove(id: string) {
    this.service.delete(id)
      .subscribe(() => this.load());
  }
}
