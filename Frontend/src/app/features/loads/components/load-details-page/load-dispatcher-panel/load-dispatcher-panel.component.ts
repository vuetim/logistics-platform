import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ToastrService } from "ngx-toastr";
import { CarrierListItem } from "../../../../../core/models/carriers/carrier-list-item.model";
import { LoadExecutionDetailsDto } from "../../../../../core/models/loads/load-details.dto";
import { CarriersService } from "../../../../../data-access/carriers/carriers.service";
import { LoadsService } from "../../../../../data-access/loads/loads.service";

@Component({
  selector: 'app-load-dispatcher-panel',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './load-dispatcher-panel.component.html',
  styleUrl: './load-dispatcher-panel.component.css'
})
export class LoadDispatcherPanelComponent implements OnInit {
  @Input({ required: true }) load!: LoadExecutionDetailsDto;
  @Output() saved = new EventEmitter<void>();
  carriers: CarrierListItem[] = [];
  saving = false;
  form: any = {};

  constructor(
    private carriersService: CarriersService,
    private loadsService: LoadsService,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.reset();
    this.carriersService.getAll().subscribe({
      next: carriers => this.carriers = carriers,
      error: () => this.carriers = []
    });
  }

  reset() {
    this.form = {
      carrierId: this.load.carrierId || '',
      modeType: this.load.mode,
      origin: this.load.origin || '',
      destination: this.load.destination || '',
      customerRate: this.load.customerRate ?? null,
      carrierRate: this.load.carrierRate ?? null,
      accessorials: this.load.accessorials ?? null,
      bolNumber: this.load.bolNumber || '',
      proNumber: this.load.proNumber || '',
      rateConfirmationNumber: this.load.rateConfirmationNumber || '',
      trackingNumber: this.load.trackingNumber || '',
      driverName: this.load.driverName || '',
      driverPhone: this.load.driverPhone || '',
      driverEmail: this.load.driverEmail || '',
      truckNumber: this.load.truckNumber || '',
      trailerNumber: this.load.trailerNumber || '',
      carrierSCAC: this.load.carrierSCAC || ''
    };
  }

  save() {
    this.saving = true;
    const dto = {
      ...this.form,
      carrierId: this.form.carrierId || null
    };

    this.loadsService.update(this.load.id, dto).subscribe({
      next: () => {
        this.saving = false;
        this.toastr.success("Load updated");
        this.saved.emit();
      },
      error: err => {
        this.saving = false;
        this.toastr.error(this.errorMessage(err), "Failed to update load");
      }
    });
  }

  private errorMessage(err: any) {
    if (!err?.error) return "Unexpected server error.";
    if (typeof err.error === 'string') return err.error;
    return err.error.message || err.error.title || "Unexpected server error.";
  }
}
