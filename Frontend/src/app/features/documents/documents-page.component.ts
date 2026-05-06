import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { RouterLink } from "@angular/router";
import { catchError, forkJoin, map, of, switchMap } from "rxjs";
import { LoadsService } from "../../data-access/loads/loads.service";
import { OrdersService } from "../../data-access/orders/orders.service";
import { OrderDocumentsService } from "../../data-access/orders/order-documents/order-documents.service";
import { LoadDocumentDto } from "../../core/models/loads/load-details.dto";
import { OrderDocumentDto } from "../../core/models/orders/order-documents/order-document.model";
import { LoadDocumentType } from "../../core/enums/loads/load-document-type.enum";
import { OrderDocumentType } from "../../core/enums/orders/order-document-type.enum";

type DocumentRow = {
  id: string;
  entityType: 'Load' | 'Order';
  entityId: string;
  entityNumber: string;
  documentType: number | string;
  fileUrl: string;
  uploadedAt?: string | null;
  isInternal: boolean;
  copyToLoad?: boolean;
};

@Component({
  selector: 'app-documents-page',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './documents-page.component.html',
  styleUrl: './documents-page.component.css'
})
export class DocumentsPageComponent implements OnInit {
  rows: DocumentRow[] = [];
  loading = false;
  error = '';

  constructor(
    private loadsService: LoadsService,
    private ordersService: OrdersService,
    private orderDocumentsService: OrderDocumentsService
  ) {}

  ngOnInit() {
    this.loading = true;

    const loads$ = this.loadsService.getPaged({ page: 1, pageSize: 50, sortBy: 'createdAt', sortDirection: 'desc' }).pipe(
      switchMap(result => {
        const loads = result.items || [];
        if (!loads.length) return of([] as DocumentRow[]);

        return forkJoin(loads.map(load =>
          this.loadsService.getDocuments(load.id).pipe(
            map(docs => (docs || []).map(doc => this.mapLoadDocument(load.id, load.loadNumber, doc))),
            catchError(() => of([] as DocumentRow[]))
          )
        )).pipe(map(groups => groups.flat()));
      }),
      catchError(() => of([] as DocumentRow[]))
    );

    const orders$ = this.ordersService.getPaged({ page: 1, pageSize: 50, sortBy: 'createdAt', sortDirection: 'desc' } as any).pipe(
      switchMap(result => {
        const orders = result.items || [];
        if (!orders.length) return of([] as DocumentRow[]);

        return forkJoin(orders.map(order =>
          this.orderDocumentsService.getByOrder(order.id).pipe(
            map(docs => (docs || []).map(doc => this.mapOrderDocument(order.id, order.orderNumber, doc))),
            catchError(() => of([] as DocumentRow[]))
          )
        )).pipe(map(groups => groups.flat()));
      }),
      catchError(() => of([] as DocumentRow[]))
    );

    forkJoin([loads$, orders$]).subscribe({
      next: ([loadDocs, orderDocs]) => {
        this.rows = [...loadDocs, ...orderDocs]
          .sort((a, b) => String(b.uploadedAt ?? '').localeCompare(String(a.uploadedAt ?? '')));
        this.loading = false;
      },
      error: err => {
        this.error = this.errorMessage(err);
        this.loading = false;
      }
    });
  }

  entityLink(row: DocumentRow) {
    return row.entityType === 'Load'
      ? ['/loads', row.entityId]
      : ['/orders', row.entityId];
  }

  typeLabel(value: number | string, entityType?: 'Load' | 'Order') {
    if (typeof value === 'string') return this.humanize(value);
    const source = entityType === 'Order' ? OrderDocumentType : LoadDocumentType;
    return this.humanize((source as any)[value] ?? String(value));
  }

  private mapLoadDocument(loadId: string, loadNumber: string, doc: LoadDocumentDto): DocumentRow {
    return {
      id: doc.id,
      entityType: 'Load',
      entityId: loadId,
      entityNumber: loadNumber,
      documentType: doc.documentType,
      fileUrl: doc.fileUrl,
      uploadedAt: doc.uploadedAt,
      isInternal: doc.isInternal
    };
  }

  private mapOrderDocument(orderId: string, orderNumber: string, doc: OrderDocumentDto): DocumentRow {
    return {
      id: doc.id,
      entityType: 'Order',
      entityId: orderId,
      entityNumber: orderNumber,
      documentType: doc.documentType,
      fileUrl: doc.fileUrl,
      uploadedAt: doc.createdAt,
      isInternal: doc.isInternal,
      copyToLoad: doc.copyToLoad
    };
  }

  private humanize(value: unknown) {
    return String(value).replace(/([A-Z])/g, ' $1').trim();
  }

  private errorMessage(err: any) {
    if (!err?.error) return "Documents unavailable";
    if (typeof err.error === 'string') return err.error;
    return err.error.message || err.error.title || "Documents unavailable";
  }
}
