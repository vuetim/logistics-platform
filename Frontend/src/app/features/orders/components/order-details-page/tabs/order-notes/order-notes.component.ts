import { DatePipe, NgFor, NgIf } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { AuthFacade } from "../../../../../../core/auth/auth.facade";
import { CreateOrderNoteDto } from "../../../../../../core/models/orders/order-notes/create-order-note.dto";
import { OrderNoteDto } from "../../../../../../core/models/orders/order-notes/order-note.model";
import { UpdateOrderNoteDto } from "../../../../../../core/models/orders/order-notes/update-order-note.dto";
import { OrderNotesService } from "../../../../../../data-access/orders/order-notes/order-notes.service";
import { BaseEntityCrudTabComponent } from "../../../../../../shared/crud/base-entity-crud-tab.component";
import { UiButtonComponent } from "../../../../../../shared/UI/ui-button/ui-button.component";
import { OrderNoteModalComponent } from "./order-note-modal/order-note-modal.component";

@Component({
  selector: 'app-order-notes',
  standalone: true,
  imports: [NgIf, NgFor, DatePipe, UiButtonComponent, OrderNoteModalComponent],
  templateUrl: './order-notes.component.html',
  styleUrl: '../order-tab-shared.css'
})
export class OrderNotesComponent
  extends BaseEntityCrudTabComponent<OrderNoteDto, CreateOrderNoteDto, UpdateOrderNoteDto>
  implements OnInit {

  constructor(
    auth: AuthFacade,
    private service: OrderNotesService
  ) {
    super(auth, {
      view: 'LoadNote_View',
      create: 'LoadNote_Create_Public',
      update: 'LoadNote_Create_Internal',
      delete: 'LoadNote_Create_Internal'
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

  protected create(dto: CreateOrderNoteDto) {
    this.service.create(this.parentId, dto).subscribe(() => this.finishSave());
  }

  protected update(id: string, dto: UpdateOrderNoteDto) {
    this.service.update(this.parentId, id, dto).subscribe(() => this.finishSave());
  }

  protected remove(id: string) {
    this.service.delete(this.parentId, id).subscribe(() => this.finishDelete());
  }
}
