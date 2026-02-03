import { Component, OnInit } from '@angular/core';
import { PageLayoutComponent } from "../../../../../layout/app-shell/page-layout/page-layout/page-layout.component";
import { GenericListPage } from '../../../../../shared/list/generic-list-page';
import { CustomersQueryParameters } from '../../../../../core/models/customers/customers-query-parameters.model';
import { EntityTableComponent } from '../../../../../shared/UI/entity-table/entity-table.component';
import { PaginationComponent } from '../../../../../shared/UI/pagination/pagination.component';
import { FilterBuilderComponent } from '../../../../../shared/filters/filter-builder/filter-builder.component';
import { UiCardComponent } from '../../../../../shared/UI/ui-card/ui-card.component';
import { UiButtonComponent } from '../../../../../shared/UI/ui-button/ui-button.component';
import { CustomerListItem } from '../../../../../core/models/customers/customer-list-item.model';
import { CustomersService } from '../../../../../data-access/customers/customers.service';
import { CUSTOMER_FILTERS } from '../../../users/filters/customers.filters';
import { CUSTOMER_STATUS_MAP } from '../../../../../shared/status/customer-status.map';
import { KeyValuePipe, NgFor, NgIf } from '@angular/common';
import { TableAction } from '../../../../../shared/UI/entity-table/entity-table.models';
import { CustomerDto } from '../../../../../core/models/customers/customer.dto';
import { AuthFacade } from '../../../../../core/auth/auth.facade';
import { ToastrService } from 'ngx-toastr';
import { CreateCustomerWizardComponent } from './create-customer-wizard/create-customer-wizard.component';
import { CreateCustomerModalComponent } from './create-customer-modal/create-customer-modal.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-customers-page',
  standalone: true,
  imports: [NgIf,
    PageLayoutComponent,
    EntityTableComponent,
    PaginationComponent,
    FilterBuilderComponent,
    UiCardComponent,
    UiButtonComponent,
    CreateCustomerModalComponent
  ],
  templateUrl: './customers-page.component.html'
})
export class CustomersPageComponent
  extends GenericListPage<CustomersQueryParameters>
  implements OnInit {

  customers: CustomerListItem[] = [];
  creatingCustomer = false;

  canCreate = false;
  canUpdate = false;
  canDelete = false;

  filtersConfig = CUSTOMER_FILTERS;

  columns = [
    { key: 'name', label: 'Name', sortable: true },
    { key: 'email', label: 'Email', sortable: true },
    { key: 'phone', label: 'Phone' },
    {
      key: 'isActive',
      label: 'Status',
      formatter: (c: CustomerListItem) => {
        const key: 'true' | 'false' = c.isActive ? 'true' : 'false';
        return CUSTOMER_STATUS_MAP[key].label;
      },
      classFn: (c: CustomerListItem) => {
        const key: 'true' | 'false' = c.isActive ? 'true' : 'false';
        return CUSTOMER_STATUS_MAP[key].class;
      }
    }
  ];

  actions: TableAction<CustomerListItem>[] = [
    {
      label: 'View',
      variant: 'ghost',
      handler: c => this.view(c)
    },
    {
      label: 'Delete',
      variant: 'danger',
      visible: () => this.auth.hasPermission('Customer_Delete'),
      handler: c => this.delete(c.id)
    }
  ];





  constructor(
    private customersService: CustomersService,
    public auth: AuthFacade,
    private toastr: ToastrService,
    private router: Router
  ) {
    super();
  }

  ngOnInit() {
    this.canCreate = this.auth.hasPermission('Customer_Create');
    this.canUpdate = this.auth.hasPermission('Customer_Update');
    this.canDelete = this.auth.hasPermission('Customer_Delete');
    this.activeFilters['isActive'] = true;
    this.reload();
  }

  protected loadData(query: CustomersQueryParameters) {
    this.customersService.getPaged(query).subscribe(res => {
      this.customers = res.items;
      this.totalCount = res.total;
    });
  }



  view(customer: CustomerListItem) {
    this.router.navigate(['/customers', customer.id]);
  }

  openCreateCustomer() {
    this.creatingCustomer = true;
  }
  onCreateCustomerClose(created: boolean) {
    this.creatingCustomer = false;

    if (created) {
      this.toastr.success('Customer created successfully');
      this.reload();
    }
  }
  override onFilterChange(e: { key: string; value: any }) {
    if (e.key === '__clear__') {
      // default per customer
      this.activeFilters = { isActive: true };
      this.page = 1;
      this.reload();
      return;
    }

    super.onFilterChange(e);
  }


  delete(id: string) {
    if (!this.canDelete) return;
    if (!confirm('Delete customer?')) return;

    this.customersService.deleteCustomer(id).subscribe({
      next: () => {
        this.toastr.success('Customer deleted');
        this.reload();
      },
      error: err => {
        console.error(err);
        this.toastr.error('Failed to delete customer');
      }
    });
  }

}


