import { Component, EventEmitter, Input, Output } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { CreateOrderExternalIdDto } from "../../../../../../../core/models/orders/order-external-ids/create-order-external-id.dto";
import { OrderExternalIdDto } from "../../../../../../../core/models/orders/order-external-ids/order-external-id.model";
import { UpdateOrderExternalIdDto } from "../../../../../../../core/models/orders/order-external-ids/update-order-external-id.dto";
import { OrderExternalIdsService } from "../../../../../../../data-access/orders/order-external-ids/order-external-ids.service";

@Component({
  selector: 'app-order-external-id-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './order-external-id-modal.component.html',
  styleUrl: './order-external-id-modal.component.css'
})
export class OrderExternalIdModalComponent {
  @Input({ required: true }) orderId!: string;
  @Input() editing?: OrderExternalIdDto;
  @Output() close = new EventEmitter<boolean>();

  loading = false;
  typeOptions = [
    { value: 'PO', label: 'PO Number' },
    { value: 'BOL', label: 'BOL Number' },
    { value: 'PRO', label: 'PRO Number' },
    { value: 'ShipmentReference', label: 'Shipment Reference' },
    { value: 'CustomerReference', label: 'Customer Reference' },
    { value: 'CarrierReference', label: 'Carrier Reference' },
    { value: 'AppointmentNumber', label: 'Appointment Number' },
    { value: 'Other', label: 'Other' }
  ];

  relatedPartyOptions = [
    { value: 'Customer', label: 'Customer' },
    { value: 'Carrier', label: 'Carrier' },
    { value: 'Warehouse', label: 'Warehouse' },
    { value: 'Buyer', label: 'Buyer' },
    { value: 'BrokerTeam', label: 'Broker Team' },
    { value: 'Other', label: 'Other' }
  ];

  model: CreateOrderExternalIdDto = {
    type: 'PO',
    value: '',
    relatedParty: 'Customer',
    copyToLoad: true
  };

  constructor(private service: OrderExternalIdsService) { }

  ngOnInit() {
    if (!this.editing) return;
    this.model = {
      type: this.normalizeType(this.editing.type),
      value: this.editing.value ?? '',
      relatedParty: this.normalizeRelatedParty(this.editing.relatedParty),
      copyToLoad: this.editing.copyToLoad
    };
  }

  save() {
    this.model.type = this.normalizeType(this.model.type);
    this.model.value = (this.model.value ?? '').trim();
    this.model.relatedParty = this.normalizeRelatedParty(this.model.relatedParty);

    if (!this.model.type || !this.model.value) return;
    this.loading = true;

    if (this.editing) {
      const dto: UpdateOrderExternalIdDto = { ...this.model };
      this.service.update(this.orderId, this.editing.id, dto).subscribe({
        next: () => this.close.emit(true),
        error: () => this.loading = false
      });
      return;
    }

    this.service.create(this.orderId, this.model).subscribe({
      next: () => this.close.emit(true),
      error: () => this.loading = false
    });
  }

  cancel() {
    this.close.emit(false);
  }

  private normalizeType(value: string | null | undefined): string {
    const trimmed = (value ?? '').trim();
    if (!trimmed) return 'PO';

    const supported = new Set(this.typeOptions.map(x => x.value));
    if (supported.has(trimmed)) return trimmed;

    const legacyMap: Record<string, string> = {
      'Po': 'PO',
      'Bol': 'BOL',
      'Pro': 'PRO',
      'PurchaseOrder': 'PO',
      'BillOfLading': 'BOL'
    };

    return legacyMap[trimmed] ?? 'Other';
  }

  private normalizeRelatedParty(value: string | null | undefined): string {
    const trimmed = (value ?? '').trim();
    if (!trimmed) return 'Customer';

    const supported = new Set(this.relatedPartyOptions.map(x => x.value));
    if (supported.has(trimmed)) return trimmed;

    const aliases: Record<string, string> = {
      'Broker Team': 'BrokerTeam',
      'BrokerTeam': 'BrokerTeam'
    };

    return aliases[trimmed] ?? 'Other';
  }
}
