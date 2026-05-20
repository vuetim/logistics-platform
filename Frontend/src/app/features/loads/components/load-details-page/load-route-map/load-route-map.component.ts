import { CommonModule } from "@angular/common";
import { AfterViewInit, Component, ElementRef, Input, OnChanges, ViewChild } from "@angular/core";
import { LoadDetailsDto, LoadStopDetailsDto } from "../../../../../core/models/loads/load-details.dto";
import { StopType } from "../../../../../core/enums/orders/stop-type.enum";
import { LoadStatus } from "../../../../../core/enums/loads/load-status.enum";

@Component({
  selector: 'app-load-route-map',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './load-route-map.component.html',
  styleUrl: './load-route-map.component.css'
})
export class LoadRouteMapComponent implements AfterViewInit, OnChanges {
  @Input({ required: true }) load!: LoadDetailsDto;
  @Input() showMap = true;
  @ViewChild('mapCanvas') mapCanvas?: ElementRef<HTMLDivElement>;

  mapReady = false;
  mapError = '';

  private initialized = false;
  private map?: any;
  private leaflet?: any;
  readonly stages = ['Open', 'Plan', 'Ship', 'Bill', 'Completed'];

  ngAfterViewInit() {
    this.initialized = true;
    this.renderMap();
  }

  ngOnChanges() {
    if (this.initialized) {
      this.renderMap();
    }
  }

  get orderedStops() {
    return [...(this.load?.execution.stops || [])].sort((a, b) => a.sequence - b.sequence);
  }

  get mappedStops() {
    return this.orderedStops.filter(stop => this.hasCoordinates(stop));
  }

  get canUseMap() {
    return this.mappedStops.length >= 1;
  }

  get originStop() {
    return this.orderedStops[0];
  }

  get destinationStop() {
    return this.orderedStops[this.orderedStops.length - 1];
  }

  get trackingProvider() {
    return this.load?.execution.trackingProvider || 'Manual tracking';
  }

  get activeStageIndex() {
    const status = this.statusValue(this.load?.execution.status);
    if (status === LoadStatus.Draft) return 0;
    if (status === LoadStatus.Planned || status === LoadStatus.Tendered || status === LoadStatus.Accepted) return 1;
    if (status === LoadStatus.Completed) return 4;
    return 2;
  }

  get isCompleted() {
    return this.statusValue(this.load?.execution.status) === LoadStatus.Completed;
  }

  get finalStageLabel() {
    return 'Completed';
  }

  get routeStatusLabel() {
    return this.executionStatusLabel(this.load?.execution.status, !!this.load?.execution.carrierId);
  }

  get statusLocation() {
    if (this.isCompleted) return this.locationLine(this.destinationStop);
    const status = this.statusValue(this.load?.execution.status);
    if (status === LoadStatus.AtPickup || status === LoadStatus.EnRouteToPickup) return this.locationLine(this.originStop);
    if (status === LoadStatus.AtDelivery || status === LoadStatus.EnRouteToDelivery || status === LoadStatus.Delivered) return this.locationLine(this.destinationStop);
    return this.locationLine(this.currentStop);
  }

  get currentStop() {
    const stops = this.orderedStops;
    if (!stops.length) return undefined;
    const active = stops.find(s => Number(s.status) > 0 && Number(s.status) < 7);
    return active || stops[0];
  }

  get distanceText() {
    const miles = this.load?.execution.distanceMiles;
    return miles ? `${Number(miles).toLocaleString(undefined, { maximumFractionDigits: 0 })} mi` : '-';
  }

  get durationText() {
    const minutes = this.load?.execution.durationMinutes;
    if (!minutes) return '-';
    const hours = Math.floor(minutes / 60);
    const mins = minutes % 60;
    return hours > 0 ? `${hours}h ${mins}m` : `${mins}m`;
  }

  get equipmentText() {
    const equipment = this.load?.equipment || [];
    if (!equipment.length) return '-';
    const first = equipment[0];
    const type = this.equipmentTypeLabel(first.equipmentType);
    const length = first.length ? `${first.length}'` : '';
    return [length, type].filter(Boolean).join(' ') || type;
  }

  get customerBillText() {
    return this.money(this.load?.costSummary?.totalBillable ?? this.load?.execution.customerRate);
  }

  get carrierPayText() {
    return this.money(this.load?.costSummary?.totalPayable ?? this.load?.execution.carrierRate);
  }

  stopLabel(stop: LoadStopDetailsDto) {
    return stop.stopType === StopType.Pickup ? 'Pickup' : stop.stopType === StopType.Delivery ? 'Delivery' : 'Stop';
  }

  stopClass(stop: LoadStopDetailsDto) {
    if (stop.stopType === StopType.Pickup) return 'pickup';
    if (stop.stopType === StopType.Delivery) return 'delivery';
    return 'midpoint';
  }

  stageClass(index: number) {
    if (this.isCompleted) return 'current';
    if (index < this.activeStageIndex) return 'current';
    if (index === this.activeStageIndex) return 'active';
    return '';
  }

  locationLine(stop?: LoadStopDetailsDto) {
    if (!stop) return '-';
    return [stop.city, stop.state].filter(Boolean).join(', ') || stop.locationName || '-';
  }

  dateText(value?: string | null) {
    if (!value) return '-';
    const date = new Date(value);
    return Number.isNaN(date.getTime()) ? '-' : date.toLocaleString();
  }

  private async renderMap() {
    if (!this.mapCanvas) return;
    this.mapError = '';

    if (this.mappedStops.length < 1) {
      this.mapError = 'Stop coordinates are required to render the route map.';
      return;
    }

    try {
      const L = await this.loadLeaflet();
      this.mapCanvas.nativeElement.replaceChildren();
      this.map?.remove();

      const points = this.mappedStops.map(stop => this.latLng(stop));
      this.map = L.map(this.mapCanvas.nativeElement, {
        zoomControl: false,
        attributionControl: true
      });

      L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 18,
        attribution: '&copy; OpenStreetMap contributors'
      }).addTo(this.map);

      L.control.zoom({ position: 'bottomright' }).addTo(this.map);

      this.mappedStops.forEach((stop, index) => {
        L.marker(this.latLng(stop), {
          icon: L.divIcon({
            className: `route-marker ${this.stopClass(stop)}`,
            html: `<span>${index + 1}</span>`,
            iconSize: [30, 30],
            iconAnchor: [15, 15]
          })
        })
          .bindPopup(`<strong>${this.stopLabel(stop)}</strong><br>${stop.locationName || this.locationLine(stop)}`)
          .addTo(this.map);
      });

      if (points.length > 1) {
        L.polyline(points, {
          color: '#2563eb',
          weight: 5,
          opacity: 0.9
        }).addTo(this.map);
      }

      this.map.fitBounds(L.latLngBounds(points), { padding: [28, 28] });
      this.mapReady = true;
    } catch {
      this.mapError = 'Leaflet map package is not installed.';
    }
  }

  private async loadLeaflet() {
    if (this.leaflet) return this.leaflet;
    this.leaflet = await import('leaflet');
    return this.leaflet;
  }

  private hasCoordinates(stop: LoadStopDetailsDto) {
    return stop.latitude != null && stop.longitude != null;
  }

  private latLng(stop: LoadStopDetailsDto) {
    return [Number(stop.latitude), Number(stop.longitude)];
  }

  private executionStatusLabel(status: number | string | null | undefined, hasCarrier: boolean) {
    const value = this.statusValue(status);
    if (value === LoadStatus.Completed) return 'Completed';
    if (value === LoadStatus.Delivered) return 'Delivered';
    if (value === LoadStatus.AtDelivery) return 'At delivery';
    if (value === LoadStatus.EnRouteToDelivery || value === LoadStatus.InTransit || value === LoadStatus.Loaded) return 'En route';
    if (value === LoadStatus.AtPickup) return 'At pickup';
    if (value === LoadStatus.EnRouteToPickup || value === LoadStatus.Dispatched) return 'Dispatched';
    if (value === LoadStatus.Accepted || hasCarrier) return 'Covered';
    if (value === LoadStatus.Tendered) return 'Tendered';
    if (value === LoadStatus.Planned) return 'Planned';
    if (value === LoadStatus.Rejected) return 'Rejected';
    if (value === LoadStatus.Cancelled) return 'Cancelled';
    return 'Open';
  }

  private statusValue(status: number | string | null | undefined) {
    if (typeof status === 'number') return status;
    if (!status) return -1;
    const numeric = Number(status);
    if (!Number.isNaN(numeric)) return numeric;
    const enumValue = (LoadStatus as Record<string, unknown>)[status];
    return typeof enumValue === 'number' ? enumValue : -1;
  }

  private equipmentTypeLabel(value: number | string | null | undefined) {
    if (value == null) return '-';
    const labels: Record<number, string> = {
      0: 'Dry Van',
      1: 'Reefer',
      2: 'Flatbed',
      3: 'Step Deck',
      4: 'Power Only'
    };
    if (typeof value === 'number') return labels[value] ?? String(value);
    return String(value).replace(/([A-Z])/g, ' $1').trim();
  }

  private money(value?: number | null) {
    return Number(value ?? 0).toLocaleString(undefined, {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    });
  }
}
