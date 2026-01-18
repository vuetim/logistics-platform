import { Component, Input } from '@angular/core';
import { BreadcrumbService } from '../../../breadcrumb.service';
import { NgFor, NgIf } from '@angular/common';


@Component({
  selector: 'app-page-layout',
  standalone: true,
  imports: [NgFor],
  templateUrl: './page-layout.component.html',
  styleUrl: './page-layout.component.css'
})
export class PageLayoutComponent {
  @Input() title = 'Logistics Platform';
  constructor(public breadcrumb: BreadcrumbService) { }
}
