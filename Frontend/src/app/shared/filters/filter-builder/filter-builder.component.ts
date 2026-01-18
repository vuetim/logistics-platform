import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule, NgFor } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FilterConfig } from './filter.types';

@Component({
  selector: 'app-filter-builder',
  standalone: true,
  imports: [CommonModule, FormsModule, NgFor,],
  templateUrl: './filter-builder.component.html',
  styleUrl: './filter-builder.component.css'
})
export class FilterBuilderComponent {

  @Input() filters: FilterConfig[] = [];
  @Output() filterChange = new EventEmitter<{ key: string; value: any }>();

  values: Record<string, any> = {};

  ngOnInit() {
    for (const f of this.filters) {
      if (!(f.key in this.values)) {
        this.values[f.key] = null;
      }
    }
  }

  onChange(key: string) {
    const value = this.values[key];
    this.filterChange.emit({
      key,
      value: value === '' ? null : value
    });
  }

  clear() {
    this.values = {};
    this.filterChange.emit({ key: '__clear__', value: null });
  }
}

