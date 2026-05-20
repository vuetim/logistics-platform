import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { RouterLink } from "@angular/router";
import { LoadStatus } from "../../../../core/enums/loads/load-status.enum";
import { LoadListItem } from "../../../../core/models/loads/load-list-item.model";

@Component({
  selector: 'app-load-summary-card',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './load-summary-card.component.html',
  styleUrl: './load-summary-card.component.css'
})
export class LoadSummaryCardComponent {
  @Input({ required: true }) load!: LoadListItem;

  get isCompleted() {
    return this.statusValue(this.load.status) === LoadStatus.Completed;
  }

  get finalStageLabel() {
    return 'Complete';
  }

  get routeStatusLabel() {
    const status = this.statusValue(this.load.status);
    if (status === LoadStatus.Completed) return 'Complete';
    if (status === LoadStatus.Delivered) return 'Delivered';
    if (status === LoadStatus.AtDelivery) return 'At delivery';
    if (status === LoadStatus.EnRouteToDelivery || status === LoadStatus.InTransit || status === LoadStatus.Loaded) return 'En route';
    if (status === LoadStatus.AtPickup) return 'At pickup';
    if (status === LoadStatus.EnRouteToPickup || status === LoadStatus.Dispatched) return 'Dispatched';
    if (status === LoadStatus.Accepted || !!this.load.carrierName) return 'Covered';
    if (status === LoadStatus.Tendered) return 'Tendered';
    if (status === LoadStatus.Planned) return 'Planned';
    return 'Open';
  }

  get statusLocation() {
    const status = this.statusValue(this.load.status);
    if (status === LoadStatus.Completed || status === LoadStatus.Delivered || status === LoadStatus.AtDelivery || status === LoadStatus.EnRouteToDelivery) {
      return this.load.destination || '-';
    }
    return this.load.origin || '-';
  }

  dateText(value?: string | null) {
    if (!value) return '-';
    const date = new Date(value);
    return Number.isNaN(date.getTime()) ? '-' : date.toLocaleString();
  }

  private statusValue(status: number | string | null | undefined) {
    if (typeof status === 'number') return status;
    if (!status) return -1;
    const numeric = Number(status);
    if (!Number.isNaN(numeric)) return numeric;
    const enumValue = (LoadStatus as Record<string, unknown>)[status];
    return typeof enumValue === 'number' ? enumValue : -1;
  }
}
