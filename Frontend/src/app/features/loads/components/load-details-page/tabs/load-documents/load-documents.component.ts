import { CommonModule } from "@angular/common";
import { Component, Input, OnInit } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ToastrService } from "ngx-toastr";
import { LoadDocumentType } from "../../../../../../core/enums/loads/load-document-type.enum";
import { LoadDocumentDto } from "../../../../../../core/models/loads/load-details.dto";
import { LoadsService } from "../../../../../../data-access/loads/loads.service";

@Component({
  selector: 'app-load-documents',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './load-documents.component.html',
  styleUrl: '../load-tab-shared.css'
})
export class LoadDocumentsComponent implements OnInit {
  @Input({ required: true }) loadId!: string;
  documents: LoadDocumentDto[] = [];
  selectedFile?: File;
  documentType = LoadDocumentType.POD;
  isInternal = false;
  loading = false;

  readonly documentTypes = Object.keys(LoadDocumentType)
    .filter(k => !isNaN(Number((LoadDocumentType as any)[k])))
    .map(k => ({ label: this.humanize(k), value: (LoadDocumentType as any)[k] as number }));

  constructor(private loadsService: LoadsService, private toastr: ToastrService) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.loading = true;
    this.loadsService.getDocuments(this.loadId).subscribe({
      next: docs => {
        this.documents = docs;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    this.selectedFile = input.files?.[0];
  }

  upload() {
    if (!this.selectedFile) return;

    const data = new FormData();
    data.append('file', this.selectedFile);
    data.append('documentType', String(this.documentType));
    data.append('isInternal', String(this.isInternal));

    this.loading = true;
    this.loadsService.uploadDocument(this.loadId, data).subscribe({
      next: () => {
        this.selectedFile = undefined;
        this.isInternal = false;
        this.toastr.success("Document uploaded");
        this.load();
      },
      error: () => {
        this.loading = false;
        this.toastr.error("Failed to upload document");
      }
    });
  }

  delete(documentId: string) {
    if (!confirm("Delete this document?")) return;
    this.loadsService.deleteDocument(this.loadId, documentId).subscribe({
      next: () => this.load(),
      error: () => this.toastr.error("Failed to delete document")
    });
  }

  typeLabel(value: number | string) {
    if (typeof value === 'string') return this.humanize(value);
    return this.humanize(LoadDocumentType[value] ?? String(value));
  }

  private humanize(value: unknown) {
    return String(value).replace(/([A-Z])/g, ' $1').trim();
  }
}
