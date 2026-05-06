import { NgFor, NgIf } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { AuthFacade } from "../../../../../../core/auth/auth.facade";
import { CreateOrderItemDto } from "../../../../../../core/models/orders/order-items/create-order-item.dto";
import { OrderItemDto } from "../../../../../../core/models/orders/order-items/order-item.model";
import { UpdateOrderItemDto } from "../../../../../../core/models/orders/order-items/update-order-items.dto";

import { OrderItemsService } from "../../../../../../data-access/orders/order-items/order-items.service";

import { BaseEntityCrudTabComponent } from "../../../../../../shared/crud/base-entity-crud-tab.component";
import { UiButtonComponent } from "../../../../../../shared/UI/ui-button/ui-button.component";
import { OrderItemModalComponent } from "./order-item-modal/order-item-modal.component";

@Component({
  selector: 'app-order-items',
  standalone: true,
  imports: [NgIf, NgFor, UiButtonComponent, OrderItemModalComponent],
  templateUrl: './order-items.component.html',
  styleUrl: '../order-tab-shared.css'
})
export class OrderItemsComponent
  extends BaseEntityCrudTabComponent<OrderItemDto, CreateOrderItemDto, UpdateOrderItemDto>
  implements OnInit {

  constructor(
    auth: AuthFacade,
    private service: OrderItemsService
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
      next: res => this.finishLoad(res),
      error: () => this.finishLoad([])
    });
  }

  protected create(dto: CreateOrderItemDto) {
    this.service.create(this.parentId, dto).subscribe(() => this.finishSave());
  }

  protected update(id: string, dto: UpdateOrderItemDto) {
    this.service.update(this.parentId, id, dto).subscribe(() => this.finishSave());
  }

  protected remove(id: string) {
    this.service.delete(this.parentId, id).subscribe(() => this.finishDelete());
  }
}
