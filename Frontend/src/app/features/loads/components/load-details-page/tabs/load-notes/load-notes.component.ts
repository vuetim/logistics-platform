import { CommonModule } from "@angular/common";
import { Component, Input, OnInit } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ToastrService } from "ngx-toastr";
import { LoadNoteDto } from "../../../../../../core/models/loads/load-details.dto";
import { LoadsService } from "../../../../../../data-access/loads/loads.service";

@Component({
  selector: 'app-load-notes',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './load-notes.component.html',
  styleUrl: '../load-tab-shared.css'
})
export class LoadNotesComponent implements OnInit {
  @Input({ required: true }) loadId!: string;
  notes: LoadNoteDto[] = [];
  text = '';
  isInternal = false;
  loading = false;

  constructor(private loadsService: LoadsService, private toastr: ToastrService) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.loading = true;
    this.loadsService.getNotes(this.loadId).subscribe({
      next: notes => {
        this.notes = notes;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  add() {
    const value = this.text.trim();
    if (!value) return;
    this.loading = true;
    this.loadsService.createNote(this.loadId, value, this.isInternal).subscribe({
      next: () => {
        this.text = '';
        this.isInternal = false;
        this.toastr.success("Note added");
        this.load();
      },
      error: () => {
        this.loading = false;
        this.toastr.error("Failed to add note");
      }
    });
  }

  noteText(note: LoadNoteDto) {
    return note.message || note.body || note.text || note.note || '-';
  }
}
