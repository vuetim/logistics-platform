import { CommonModule } from '@angular/common';
import { Component, HostListener, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CustomersService } from '../../../../../data-access/customers/customers.service';
import { AuthFacade } from '../../../../../core/auth/auth.facade';
import { CustomerDetailsDto } from '../../../../../core/models/customers/customers-details-dto';
import { CustomerAddressesComponent } from './tabs/customer-addresses/customer-addresses.component';
import { PageLayoutComponent } from '../../../../../layout/app-shell/page-layout/page-layout/page-layout.component';
import { CustomerContactsComponent } from "./tabs/customer-contacts/customer-contacts.component";
import { CustomerNotesComponent } from './tabs/customer-notes/customer-notes.component';


type TabKey = 'overview' | 'addresses' | 'contacts' | 'notes';

@Component({
  selector: 'app-customer-details-page',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    CustomerAddressesComponent,
    CustomerContactsComponent,
    CustomerNotesComponent,
    PageLayoutComponent,
    CustomerContactsComponent
  ],
  templateUrl: './customer-details-page.component.html',
  styleUrl: './customer-details-page.component.css'
})
export class CustomerDetailsPageComponent implements OnInit {
  customerId!: string;
  customer?: CustomerDetailsDto;

  loading = true;
  tab: TabKey = 'overview';

  // permissions 
  canUpdate = false;
  canDelete = false;

  constructor(
    private route: ActivatedRoute,
    private customersService: CustomersService,
    public auth: AuthFacade,
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.customerId = this.route.snapshot.paramMap.get('id')!;
    this.canUpdate = this.auth.hasPermission('Customer_Update');
    this.canDelete = this.auth.hasPermission('Customer_Delete');

    this.load();
  }

  load() {
    this.loading = true;

    this.customersService.getCustomerDetails(this.customerId).subscribe({
      next: (res) => {
        this.customer = res;
        this.loading = false;
      },
      error: () => {
        this.toastr.error('Failed to load customer details');
        this.loading = false;
      }
    });
  }

  setTab(t: TabKey) {
    this.tab = t;
  }

  // child components kur bojnë change -> reload
  onChildChanged() {
    this.load();
  }



}
