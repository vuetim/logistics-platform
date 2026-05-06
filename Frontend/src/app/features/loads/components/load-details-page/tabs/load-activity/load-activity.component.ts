import { CommonModule } from "@angular/common";
import { Component, Input, OnInit } from "@angular/core";
import { LoadActivityDto } from "../../../../../../core/models/loads/load-details.dto";
import { LoadsService } from "../../../../../../data-access/loads/loads.service";

@Component({
  selector: 'app-load-activity',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './load-activity.component.html',
  styleUrl: '../load-tab-shared.css'
})
export class LoadActivityComponent implements OnInit {
  @Input({ required: true }) loadId!: string;
  activity: LoadActivityDto[] = [];
  loading = false;

  constructor(private loadsService: LoadsService) {}

  ngOnInit() {
    this.loading = true;
    this.loadsService.getActivity(this.loadId).subscribe({
      next: rows => {
        this.activity = rows;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  text(row: LoadActivityDto) {
    return row.action || row.description || row.message || row.details || '-';
  }

  type(row: LoadActivityDto) {
    return row.field || row.activityType || '-';
  }

  user(row: LoadActivityDto) {
    return row.performedBy || row.userName || '-';
  }
}
