import { Component, Input } from '@angular/core';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-ui-card',
  standalone: true,
  imports: [NgIf],
  templateUrl: './ui-card.component.html',
  styleUrl: './ui-card.component.css'
})
export class UiCardComponent {
  @Input() cardTitle?: string;
}
