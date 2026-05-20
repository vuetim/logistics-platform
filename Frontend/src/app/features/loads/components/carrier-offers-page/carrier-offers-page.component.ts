import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { RouterModule } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { AuthFacade } from "../../../../core/auth/auth.facade";
import { Permission } from "../../../../core/auth/permissions/permission.enum";
import { OpenCarrierOfferDto } from "../../../../core/models/loads/load-details.dto";
import { LoadsService } from "../../../../data-access/loads/loads.service";
import { PageLayoutComponent } from "../../../../layout/app-shell/page-layout/page-layout/page-layout.component";

@Component({
  selector: 'app-carrier-offers-page',
  standalone: true,
  imports: [CommonModule, RouterModule, PageLayoutComponent],
  templateUrl: './carrier-offers-page.component.html',
  styleUrl: './carrier-offers-page.component.css'
})
export class CarrierOffersPageComponent implements OnInit {
  offers: OpenCarrierOfferDto[] = [];
  loading = true;
  actionId?: string;

  constructor(
    private loadsService: LoadsService,
    private toastr: ToastrService,
    private auth: AuthFacade
  ) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.loading = true;
    this.loadsService.getOpenCarrierOffers().subscribe({
      next: res => {
        this.offers = res;
        this.loading = false;
      },
      error: err => {
        this.loading = false;
        this.toastr.error(this.errorMessage(err), "Failed to load carrier offers");
      }
    });
  }

  accept(offer: OpenCarrierOfferDto) {
    if (!this.canAccept()) return;
    this.actionId = offer.assignmentId;
    this.loadsService.acceptCarrierAssignment(offer.loadId, offer.assignmentId).subscribe({
      next: () => {
        this.toastr.success("Carrier marked covered");
        this.actionId = undefined;
        this.load();
      },
      error: err => {
        this.actionId = undefined;
        this.toastr.error(this.errorMessage(err), "Failed to accept carrier offer");
      }
    });
  }

  reject(offer: OpenCarrierOfferDto) {
    if (!this.canReject()) return;
    this.actionId = offer.assignmentId;
    this.loadsService.rejectCarrierAssignment(offer.loadId, offer.assignmentId).subscribe({
      next: () => {
        this.toastr.success("Carrier offer rejected");
        this.actionId = undefined;
        this.load();
      },
      error: err => {
        this.actionId = undefined;
        this.toastr.error(this.errorMessage(err), "Failed to reject carrier offer");
      }
    });
  }

  canAccept() {
    return this.auth.hasRole('Admin') || this.auth.hasPermission(Permission.CarrierOffer_Accept);
  }

  canReject() {
    return this.auth.hasRole('Admin') || this.auth.hasPermission(Permission.CarrierOffer_Reject);
  }

  money(value?: number | null) {
    return Number(value ?? 0).toLocaleString(undefined, {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    });
  }

  isExpiringSoon(offer: OpenCarrierOfferDto) {
    if (!offer.tenderExpiresAt) return false;
    const expires = new Date(offer.tenderExpiresAt).getTime();
    if (Number.isNaN(expires)) return false;
    return expires - Date.now() <= 1000 * 60 * 60 * 6;
  }

  private errorMessage(err: any) {
    if (!err?.error) return "Unexpected server error.";
    if (typeof err.error === 'string') return err.error;
    return err.error.message || err.error.title || "Unexpected server error.";
  }
}
