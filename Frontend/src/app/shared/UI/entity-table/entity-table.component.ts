import { Component, Input } from '@angular/core';
import { NgFor, NgIf, NgClass } from '@angular/common';
import { TableColumn, TableAction } from './entity-table.models';
import { UiButtonComponent } from '../ui-button/ui-button.component';


@Component({
  selector: 'app-entity-table',
  standalone: true,
  imports: [NgFor, NgIf, UiButtonComponent, NgClass, NgClass],
  templateUrl: './entity-table.component.html',
  styleUrl: './entity-table.component.css'
})
export class EntityTableComponent<T> {

  @Input() columns: TableColumn<T>[] = [];
  @Input() rows: T[] = [];
  @Input() actions: TableAction<T>[] = [];

  sortKey?: string;
  sortDir: 'asc' | 'desc' = 'asc';

  get sortedRows(): T[] {
    if (!this.sortKey) return this.rows;

    return [...this.rows].sort((a: any, b: any) => {
      const av = a[this.sortKey!];
      const bv = b[this.sortKey!];

      if (av == null) return 1;
      if (bv == null) return -1;

      return this.sortDir === 'asc'
        ? av > bv ? 1 : -1
        : av < bv ? 1 : -1;
    });
  }

  sort(col: TableColumn<T>) {
    if (!col.sortable) return;

    if (this.sortKey === col.key) {
      this.sortDir = this.sortDir === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortKey = col.key as string;
      this.sortDir = 'asc';
    }
  }
  getCellValue(row: T, col: TableColumn<T>): any {
    if (col.formatter) {
      return col.formatter(row);
    }

    return (row as any)[col.key as string];
  }

}
