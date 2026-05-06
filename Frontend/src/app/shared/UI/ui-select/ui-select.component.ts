import { NgFor } from "@angular/common";
import { Component, forwardRef, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from "@angular/forms";

@Component({
  selector: 'ui-select',
  standalone: true,
  imports: [NgFor],
  templateUrl: './ui-select.component.html',
  styleUrls: ['./ui-select.component.css'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => UiSelectComponent),
    multi: true
  }]
})
export class UiSelectComponent implements ControlValueAccessor {

  @Input() options: { label: string; value: any }[] = [];
  @Input() disabled = false;

  value: any;

  onChange = (_: any) => { };
  onTouched = () => { };

  writeValue(val: any) { this.value = val; }
  registerOnChange(fn: any) { this.onChange = fn; }
  registerOnTouched(fn: any) { this.onTouched = fn; }
  setDisabledState(state: boolean) { this.disabled = state; }

  update(val: any) {
    this.value = val;
    this.onChange(val);
  }
}
