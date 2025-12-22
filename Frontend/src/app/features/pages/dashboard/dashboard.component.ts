import { Component } from '@angular/core';
import { PageLayoutComponent } from '../../../layout/app-shell/page-layout/page-layout/page-layout.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [PageLayoutComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {

}
