import { Component, EventEmitter, Output } from '@angular/core';
import { OrderCreateWizardComponent } from './order-create-wizard/order-create-wizard.component';

@Component({
    selector: 'app-order-create-modal',
    standalone: true,
    imports: [OrderCreateWizardComponent],
    templateUrl: './order-create-modal.component.html',
    styleUrl: './order-create-modal.component.css'
})
export class OrderCreateModalComponent {
    @Output() close = new EventEmitter<boolean>();
}
