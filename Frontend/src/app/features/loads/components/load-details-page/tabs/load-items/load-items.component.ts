import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { LoadItemDto } from "../../../../../../core/models/loads/load-details.dto";

@Component({
  selector: 'app-load-items',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './load-items.component.html',
  styleUrl: '../load-tab-shared.css'
})
export class LoadItemsComponent {
  @Input() items: LoadItemDto[] = [];
}
