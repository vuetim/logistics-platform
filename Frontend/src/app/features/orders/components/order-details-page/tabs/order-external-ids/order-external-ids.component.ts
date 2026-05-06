import { NgFor, NgIf } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { AuthFacade } from "../../../../../../core/auth/auth.facade";
import { CreateOrderExternalIdDto } from "../../../../../../core/models/orders/order-external-ids/create-order-external-id.dto";
import { OrderExternalIdDto } from "../../../../../../core/models/orders/order-external-ids/order-external-id.model";
import { UpdateOrderExternalIdDto } from "../../../../../../core/models/orders/order-external-ids/update-order-external-id.dto";
import { OrderExternalIdsService } from "../../../../../../data-access/orders/order-external-ids/order-external-ids.service";
import { BaseEntityCrudTabComponent } from "../../../../../../shared/crud/base-entity-crud-tab.component";
import { UiButtonComponent } from "../../../../../../shared/UI/ui-button/ui-button.component";
import { OrderExternalIdModalComponent } from "./order-external-id-modal/order-external-id-modal.component";

@Component({
  selector: 'app-order-external-ids',
  standalone: true,
  imports: [NgIf, NgFor, UiButtonComponent, OrderExternalIdModalComponent],
  templateUrl: './order-external-ids.component.html',
  styleUrl: '../order-tab-shared.css'
})
export class OrderExternalIdsComponent
  extends BaseEntityCrudTabComponent<OrderExternalIdDto, CreateOrderExternalIdDto, UpdateOrderExternalIdDto>
  implements OnInit {
  private readonly typeLabelMap = new Map<string, string>([
    ['PO', 'PO Number'],
    ['BOL', 'BOL Number'],
    ['PRO', 'PRO Number'],
    ['ShipmentReference', 'Shipment Reference'],
    ['CustomerReference', 'Customer Reference'],
    ['CarrierReference', 'Carrier Reference'],
    ['AppointmentNumber', 'Appointment Number'],
    ['Other', 'Other']
  ]);
  private readonly relatedPartyLabelMap = new Map<string, string>([
    ['Customer', 'Customer'],
    ['Carrier', 'Carrier'],
    ['Warehouse', 'Warehouse'],
    ['Buyer', 'Buyer'],
    ['BrokerTeam', 'Broker Team'],
    ['Other', 'Other']
  ]);

  constructor(
    auth: AuthFacade,
    private service: OrderExternalIdsService
  ) {
    super(auth, {
      view: 'Load_View',
      create: 'Load_Update',
      update: 'Load_Update',
      delete: 'Load_Update'
    });
  }

  ngOnInit() {
    this.load();
  }

  protected fetch(orderId: string) {
    this.service.getByOrder(orderId).subscribe({
      next: res => this.finishLoad(res.map(x => ({
        ...x,
        type: (x.type ?? '').trim()
      }))),
      error: () => this.finishLoad([])
    });
  }

  protected create(dto: CreateOrderExternalIdDto) {
    this.service.create(this.parentId, dto).subscribe(() => this.finishSave());
  }

  protected update(id: string, dto: UpdateOrderExternalIdDto) {
    this.service.update(this.parentId, id, dto).subscribe(() => this.finishSave());
  }

  protected remove(id: string) {
    this.service.delete(this.parentId, id).subscribe(() => this.finishDelete());
  }

  typeLabel(value: string | null | undefined) {
    const type = (value ?? '').trim();
    if (!type) return 'Unknown';
    return this.typeLabelMap.get(type) ?? type;
  }

  relatedPartyLabel(value: string | null | undefined) {
    const party = (value ?? '').trim();
    if (!party) return '-';
    return this.relatedPartyLabelMap.get(party) ?? party;
  }
}
