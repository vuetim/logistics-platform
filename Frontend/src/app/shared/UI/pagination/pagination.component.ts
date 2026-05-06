import { Component, Input, Output, EventEmitter } from '@angular/core';
import { NgFor } from '@angular/common';

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

  get safeTotalPages(): number {
    return Math.max(1, this.totalPages || 1);
  }

  get pages(): number[] {
    return Array.from({ length: this.safeTotalPages }, (_, i) => i + 1);
  }

  change(p: number) {
    if (p < 1 || p > this.safeTotalPages) return;
    this.page = p;
    this.pageChange.emit(p);
  }
}
