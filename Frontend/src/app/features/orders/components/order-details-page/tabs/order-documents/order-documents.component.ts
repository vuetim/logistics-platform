import { DatePipe, NgFor, NgIf } from "@angular/common";
import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { AuthFacade } from "../../../../../../core/auth/auth.facade";
import { OrderDocumentType } from "../../../../../../core/enums/orders/order-document-type.enum";
import { OrderDocumentDto } from "../../../../../../core/models/orders/order-documents/order-document.model";
import { enumToOptions } from "../../../../../../core/utils/enum-options";
import { OrderDocumentsService } from "../../../../../../data-access/orders/order-documents/order-documents.service";
import { UiButtonComponent } from "../../../../../../shared/UI/ui-button/ui-button.component";

@Component({
  selector: 'app-order-documents',
  standalone: true,
  imports: [NgIf, NgFor, DatePipe, FormsModule, UiButtonComponent],
  templateUrl: './order-documents.component.html',
  styleUrl: '../order-tab-shared.css'
})
export class OrderDocumentsComponent implements OnInit {
  @Input({ required: true }) parentId!: string;
  @Output() changed = new EventEmitter<void>();

  canView = false;
  canUpload = false;
  canDelete = false;
  loading = false;
  docs: OrderDocumentDto[] = [];

  documentTypeOptions = enumToOptions(OrderDocumentType);
  private readonly documentTypeLookup = new Map(this.documentTypeOptions.map(x => [x.value, x.label]));
  documentType = OrderDocumentType.Other;
  isInternal = false;
  copyToLoad = true;
  selectedFile?: File;

  constructor(
    private service: OrderDocumentsService,
    private auth: AuthFacade
  ) { }

  ngOnInit() {
    this.canView = this.auth.hasPermission('LoadDocument_View');
    this.canUpload = this.auth.hasPermission('LoadDocument_Upload');
    this.canDelete = this.auth.hasPermission('LoadDocument_Delete');
    this.load();
  }

  load() {
    if (!this.canView || !this.parentId) return;
    this.loading = true;
    this.service.getByOrder(this.parentId).subscribe({
      next: res => {
        this.docs = res;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  onFileChange(event: Event) {
    const target = event.target as HTMLInputElement;
    this.selectedFile = target.files?.[0];
  }

  upload() {
    if (!this.canUpload || !this.selectedFile) return;

    const data = new FormData();
    data.append('file', this.selectedFile);
    data.append('documentType', String(this.documentType));
    data.append('isInternal', String(this.isInternal));
    data.append('copyToLoad', String(this.copyToLoad));

    this.loading = true;
    this.service.upload(this.parentId, data).subscribe({
      next: () => {
        this.selectedFile = undefined;
        this.changed.emit();
        this.load();
      },
      error: () => this.loading = false
    });
  }

  deleteDocument(id: string) {
    if (!this.canDelete) return;
    if (!confirm('Delete this document?')) return;

    this.service.delete(this.parentId, id).subscribe(() => {
      this.changed.emit();
      this.load();
    });
  }

  documentTypeLabel(value: number) {
    return this.documentTypeLookup.get(value) ?? value;
  }
}
