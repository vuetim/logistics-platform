import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ToastrService } from "ngx-toastr";
import { AuthFacade } from "../../../../../core/auth/auth.facade";
import { Permission } from "../../../../../core/auth/permissions/permission.enum";
import { CarrierListItem } from "../../../../../core/models/carriers/carrier-list-item.model";
import { LoadExecutionDetailsDto } from "../../../../../core/models/loads/load-details.dto";
import { CarriersService } from "../../../../../data-access/carriers/carriers.service";
import { LoadsService } from "../../../../../data-access/loads/loads.service";
import { FieldInfoComponent } from "../../../../../shared/UI/field-info/field-info.component";

@Component({
  selector: 'app-load-operational-panel',
  standalone: true,
  imports: [CommonModule, FormsModule, FieldInfoComponent],
  templateUrl: './load-operational-panel.component.html',
  styleUrl: './load-operational-panel.component.css'
})
export class LoadOperationalPanelComponent implements OnInit, OnChanges {
  @Input({ required: true }) load!: LoadExecutionDetailsDto;
  @Output() saved = new EventEmitter<void>();
  carriers: CarrierListItem[] = [];
  saving = false;
  form: any = {};

  constructor(
    private carriersService: CarriersService,
    private loadsService: LoadsService,
    private toastr: ToastrService,
    private auth: AuthFacade
  ) { }

  ngOnInit() {
    this.reset();
    if (this.canEditCommercial()) {
      this.carriersService.getAll().subscribe({
        next: carriers => this.carriers = carriers,
        error: () => this.carriers = []
      });
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['load'] && !changes['load'].firstChange && !this.saving) {
      this.reset();
    }
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
    if (!this.canSave()) return;

    this.saving = true;
    const dto = this.buildUpdateDto();

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

  canSave() {
    return this.canEditCommercial() || this.canEditOperational();
  }

  canEditCommercial() {
    return this.auth.hasRole('Admin') || this.auth.hasPermission(Permission.Load_Update);
  }

  canEditOperational() {
    return this.canEditCommercial() || this.auth.hasPermission(Permission.Load_Operational_Update);
  }

  private buildUpdateDto() {
    const operationalFields = {
      origin: this.form.origin,
      destination: this.form.destination,
      bolNumber: this.form.bolNumber,
      proNumber: this.form.proNumber,
      rateConfirmationNumber: this.form.rateConfirmationNumber,
      trackingNumber: this.form.trackingNumber,
      driverName: this.form.driverName,
      driverPhone: this.form.driverPhone,
      driverEmail: this.form.driverEmail,
      truckNumber: this.form.truckNumber,
      trailerNumber: this.form.trailerNumber,
      carrierSCAC: this.form.carrierSCAC
    };

    if (!this.canEditCommercial()) {
      return operationalFields;
    }

    return {
      ...operationalFields,
      carrierId: this.form.carrierId || null,
      modeType: this.form.modeType,
      customerRate: this.form.customerRate,
      carrierRate: this.form.carrierRate,
      accessorials: this.form.accessorials
    };
  }
}
