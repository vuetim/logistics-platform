import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { CarrierListItem } from "../../../../../core/models/carriers/carrier-list-item.model";
import { CreateLoadFromOrderDto } from "../../../../../core/models/orders/create-load-from-order.dto";
import { OrderDetailsDto } from "../../../../../core/models/orders/order-details.dto";
import { CarriersService } from "../../../../../data-access/carriers/carriers.service";

@Component({
  selector: 'app-create-load-from-order-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-load-from-order-modal.component.html',
  styleUrl: './create-load-from-order-modal.component.css'
})
export class CreateLoadFromOrderModalComponent implements OnInit {
  @Input({ required: true }) order!: OrderDetailsDto;
  @Output() close = new EventEmitter<CreateLoadFromOrderDto | null>();

  carriers: CarrierListItem[] = [];
  loadingCarriers = false;

  model = {
    carrierId: '',
    carrierRate: null as number | null,
    plannedPickupDate: null as string | null,
    plannedDeliveryDate: null as string | null,
    rateConfirmationNumber: ''
  };

  constructor(private carriersService: CarriersService) { }

  ngOnInit() {
    this.model.carrierId = this.order.preferredCarrierId ?? '';
    this.model.plannedPickupDate = this.toInputDate(this.order.plannedPickup?.date);
    this.model.plannedDeliveryDate = this.toInputDate(this.order.plannedDelivery?.date);

    this.loadingCarriers = true;
    this.carriersService.getAll().subscribe({
      next: carriers => {
        this.carriers = carriers;
        this.loadingCarriers = false;
      },
      error: () => {
        this.carriers = [];
        this.loadingCarriers = false;
      }
    });
  }

  submit() {
    this.close.emit({
      orderId: this.order.id,
      carrierId: this.model.carrierId || null,
      carrierRate: this.model.carrierRate,
      plannedPickupDate: this.toApiDate(this.model.plannedPickupDate),
      plannedDeliveryDate: this.toApiDate(this.model.plannedDeliveryDate),
      rateConfirmationNumber: this.model.rateConfirmationNumber?.trim() || null,
      splitOrder: false
    });
  }

  cancel() {
    this.close.emit(null);
  }

  private toInputDate(value?: string | null) {
    if (!value) return null;
    return value.slice(0, 16);
  }

  private toApiDate(value?: string | null) {
    return value?.trim() ? new Date(value).toISOString() : null;
  }
}
