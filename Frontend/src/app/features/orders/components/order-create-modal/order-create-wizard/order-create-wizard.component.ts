import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OrdersService } from '../../../../../data-access/orders/orders.service';
import { CreateOrderDto } from '../../../../../core/models/orders/create-order.dt';
import { CustomersService } from '../../../../../data-access/customers/customers.service';
import { CustomerListItem } from '../../../../../core/models/customers/customer-list-item.model';
import { CarriersService } from '../../../../../data-access/carriers/carriers.service';
import { CarrierListItem } from '../../../../../core/models/carriers/carrier-list-item.model';
import { FieldInfoComponent } from '../../../../../shared/UI/field-info/field-info.component';

@Component({
    selector: 'app-order-create-wizard',
    standalone: true,
    imports: [CommonModule, FormsModule, FieldInfoComponent],
    templateUrl: './order-create-wizard.component.html',
    styleUrl: './order-create-wizard.component.css'
})
export class OrderCreateWizardComponent implements OnInit {
    @Output() close = new EventEmitter<boolean>();

    order: CreateOrderDto = {
        customerId: '',
        orderType: 'Transportation',
        direction: 'Inbound',
        startDate: { date: '', timezone: null, hasTime: true },
        endDate: { date: '', timezone: null, hasTime: true },
        preferredCarrierId: null,
        plannedPickup: null,
        plannedDelivery: null,
        primaryPONumber: null,
        primaryBolNumber: null,
        primaryProNumber: null,
        commodity: null,
        totalWeight: null,
        totalPallets: null,
        totalVolume: null,
        dispatchNotes: null,
        deliveryNotes: null
    };
    startDateInput = '';
    endDateInput = '';
    plannedPickupInput: string | null = null;
    plannedDeliveryInput: string | null = null;

    customers: CustomerListItem[] = [];
    carriers: CarrierListItem[] = [];
    loading = false;
    validationMessage = '';

    constructor(
        private ordersService: OrdersService,
        private customersService: CustomersService,
        private carriersService: CarriersService
    ) { }

    ngOnInit(): void {
        this.customersService.getPaged({ page: 1, pageSize: 100 }).subscribe(res => this.customers = res.items);
        this.carriersService.getAll().subscribe({
            next: carriers => this.carriers = carriers,
            error: () => this.carriers = []
        });
    }

    submit() {
        this.validationMessage = '';
        if (!this.order.customerId || !this.startDateInput || !this.endDateInput) {
            this.validationMessage = 'Customer, order window start, and order window end are required.';
            return;
        }

        const timezone = Intl.DateTimeFormat().resolvedOptions().timeZone || 'UTC';
        const start = new Date(this.startDateInput);
        const end = new Date(this.endDateInput);
        const plannedPickup = this.plannedPickupInput ? new Date(this.plannedPickupInput) : null;
        const plannedDelivery = this.plannedDeliveryInput ? new Date(this.plannedDeliveryInput) : null;

        if (end < start) {
            this.validationMessage = 'Order window end cannot be before order window start.';
            return;
        }

        if (plannedPickup && plannedPickup < start) {
            this.validationMessage = 'Planned pickup cannot be before order window start.';
            return;
        }

        if (plannedDelivery && plannedDelivery > end) {
            this.validationMessage = 'Planned delivery cannot be after order window end.';
            return;
        }

        if (plannedPickup && plannedDelivery && plannedDelivery < plannedPickup) {
            this.validationMessage = 'Planned delivery cannot be before planned pickup.';
            return;
        }

        const startIso = start.toISOString();
        const endIso = end.toISOString();
        this.order.startDate = { date: startIso, timezone, hasTime: true };
        this.order.endDate = { date: endIso, timezone, hasTime: true };
        this.order.startDateType = { key: '33091', value: 'On a specific date' };
        this.order.endDateType = { key: '33091', value: 'On a specific date' };
        if (this.plannedPickupInput) {
            this.order.plannedPickup = {
                date: new Date(this.plannedPickupInput).toISOString(),
                timezone,
                hasTime: true
            };
        } else {
            this.order.plannedPickup = null;
        }
        if (this.plannedDeliveryInput) {
            this.order.plannedDelivery = {
                date: new Date(this.plannedDeliveryInput).toISOString(),
                timezone,
                hasTime: true
            };
        } else {
            this.order.plannedDelivery = null;
        }

        this.loading = true;
        this.ordersService.create(this.order).subscribe({
            next: (orderId) => {
                console.log('Order created successfully with ID:', orderId);
                this.loading = false;
                this.close.emit(true);
            },
            error: err => {
                this.loading = false;
                console.error('CREATE ORDER ERROR:', err);
                this.close.emit(false);
            }
        });
    }

    cancel() {
        this.close.emit(false);
    }
}
