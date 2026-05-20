import { Component, Input } from "@angular/core";

@Component({
  selector: 'app-field-info',
  standalone: true,
  template: `
    <span class="field-info" [attr.aria-label]="label" [attr.data-tip]="text">i</span>
  `,
  styles: [`
    :host {
      display: inline-flex;
      vertical-align: middle;
    }

    .field-info {
      position: relative;
      display: inline-grid;
      place-items: center;
      flex: 0 0 auto;
      width: 16px;
      height: 16px;
      border: 1px solid #cbd5e1;
      border-radius: 999px;
      background: #ffffff;
      color: #2563eb;
      font-size: 11px;
      font-weight: 800;
      line-height: 1;
      cursor: help;
      user-select: none;
    }

    .field-info::after {
      content: attr(data-tip);
      position: absolute;
      left: calc(100% + 8px);
      top: 50%;
      z-index: 10000;
      width: min(280px, 70vw);
      padding: 9px 10px;
      border: 1px solid #dbe4ef;
      border-radius: 8px;
      background: #0f172a;
      color: #ffffff;
      box-shadow: 0 14px 30px rgba(15, 23, 42, 0.18);
      font-size: 12px;
      font-weight: 500;
      letter-spacing: 0;
      line-height: 1.35;
      opacity: 0;
      pointer-events: none;
      text-align: left;
      text-transform: none;
      transform: translate(0, -50%);
      transition: opacity 0.12s ease, transform 0.12s ease;
    }

    .field-info:hover::after {
      opacity: 1;
      transform: translate(0, -50%);
    }
  `]
})
export class FieldInfoComponent {
  @Input({ required: true }) text = '';
  @Input() label = 'Field help';
}
