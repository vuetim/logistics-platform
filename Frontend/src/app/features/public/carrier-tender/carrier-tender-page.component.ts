import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { CarrierTendersService } from "../../../data-access/carrier-tenders/carrier-tenders.service";
import { PublicCarrierTenderDto } from "../../../core/models/carriers/public-carrier-tender.dto";

@Component({
    selector: 'app-carrier-tender-page',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './carrier-tender-page.component.html',
    styleUrl: './carrier-tender-page.component.css'
})
export class CarrierTenderPageComponent implements OnInit {
    token = '';
    tender?: PublicCarrierTenderDto;
    loading = true;
    submitting = false;
    error = '';
    completedMessage = '';
    form = {
        contactName: '',
        contactEmail: '',
        contactPhone: '',
        notes: ''
    };

    constructor(
        private route: ActivatedRoute,
        private tenders: CarrierTendersService
    ) { }

    ngOnInit() {
        this.token = this.route.snapshot.paramMap.get('token') || '';
        this.tenders.get(this.token).subscribe({
            next: tender => {
                this.tender = tender;
                this.loading = false;
            },
            error: () => {
                this.error = 'Tender not found or no longer available.';
                this.loading = false;
            }
        });
    }

    accept() {
        if (!this.tender || this.submitting) return;
        this.respond('accept');
    }

    reject() {
        if (!this.tender || this.submitting) return;
        this.respond('reject');
    }

    money(value?: number | null) {
        return Number(value ?? 0).toLocaleString(undefined, {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });
    }

    private respond(action: 'accept' | 'reject') {
        this.submitting = true;
        const payload = {
            contactName: this.form.contactName || null,
            contactEmail: this.form.contactEmail || null,
            contactPhone: this.form.contactPhone || null,
            notes: this.form.notes || null
        };
        const request = action === 'accept'
            ? this.tenders.accept(this.token, payload)
            : this.tenders.reject(this.token, payload);

        request.subscribe({
            next: () => {
                this.submitting = false;
                this.completedMessage = action === 'accept'
                    ? 'Tender accepted. A rate confirmation has been sent.'
                    : 'Tender rejected. Thank you for the update.';
            },
            error: err => {
                this.submitting = false;
                this.error = typeof err?.error === 'string'
                    ? err.error
                    : err?.error?.message || 'Unable to submit tender response.';
            }
        });
    }
}
