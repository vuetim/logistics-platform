import { DatePipe, NgFor, NgIf } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { AuthFacade } from "../../../../../../core/auth/auth.facade";
import { StopType } from "../../../../../../core/enums/orders/stop-type.enum";
import { CreateOrderRouteDto } from "../../../../../../core/models/orders/order-routes/create-order-route.dto";
import { OrderRouteDto } from "../../../../../../core/models/orders/order-routes/order-route.model";
import { UpdateOrderRouteDto } from "../../../../../../core/models/orders/order-routes/update-order-route.dto";
import { enumToOptions } from "../../../../../../core/utils/enum-options";
import { OrderRoutesService } from "../../../../../../data-access/orders/order-routes/order-routes.service";
import { BaseEntityCrudTabComponent } from "../../../../../../shared/crud/base-entity-crud-tab.component";
import { UiButtonComponent } from "../../../../../../shared/UI/ui-button/ui-button.component";
import { OrderRouteModalComponent } from "./order-route-modal/order-route-modal.component";

@Component({
  selector: 'app-order-routes',
  standalone: true,
  imports: [NgIf, NgFor, DatePipe, UiButtonComponent, OrderRouteModalComponent],
  templateUrl: './order-routes.component.html',
  styleUrl: '../order-tab-shared.css'
})
export class OrderRoutesComponent
  extends BaseEntityCrudTabComponent<OrderRouteDto, CreateOrderRouteDto, UpdateOrderRouteDto>
  implements OnInit {
  private readonly stopTypeLookup = new Map(enumToOptions(StopType).map((x: { value: number; label: string }) => [x.value, x.label]));

  constructor(
    auth: AuthFacade,
    private service: OrderRoutesService
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
      next: res => this.finishLoad(res.map(route => ({
        ...route,
        stopType: this.normalizeStopType(route.stopType)
      }))),
      error: () => this.finishLoad([])
    });
  }

  protected create(dto: CreateOrderRouteDto) {
    this.service.create(this.parentId, dto).subscribe(() => this.finishSave());
  }

  protected update(id: string, dto: UpdateOrderRouteDto) {
    this.service.update(this.parentId, id, dto).subscribe(() => this.finishSave());
  }

  protected remove(id: string) {
    this.service.delete(this.parentId, id).subscribe(() => this.finishDelete());
  }

  stopTypeLabel(value: number | string) {
    const normalized = this.normalizeStopType(value);
    return this.stopTypeLookup.get(normalized) ?? value;
  }

  private normalizeStopType(value: number | string | null | undefined): number {
    if (typeof value === 'number') return value;
    if (typeof value === 'string') {
      const numeric = Number(value);
      if (!Number.isNaN(numeric)) return numeric;
      const fromEnum = (StopType as Record<string, unknown>)[value];
      if (typeof fromEnum === 'number') return fromEnum;
    }
    return StopType.Pickup;
  }
}
