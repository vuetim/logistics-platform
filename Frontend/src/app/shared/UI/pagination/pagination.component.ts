import { Component, Input, Output, EventEmitter } from '@angular/core';
import { NgFor } from '@angular/common';
import { UiButtonComponent } from "../ui-button/ui-button.component";

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [NgFor],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css'
})
export class PaginationComponent {
  @Input() page = 1;
  @Input() totalPages = 1;
  @Output() pageChange = new EventEmitter<number>();

  change(p: number) {
    if (p < 1 || p > this.totalPages) return;
    this.page = p;
    this.pageChange.emit(p);
  }
}
