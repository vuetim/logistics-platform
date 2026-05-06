import { Component, forwardRef } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";

@Component({
  selector: 'ui-checkbox',
  standalone: true,
  templateUrl: './ui-checkbox.component.html',
  styleUrls: ['./ui-checkbox.component.css'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => UiCheckboxComponent),
    multi: true
  }]
})
export class UiCheckboxComponent implements ControlValueAccessor {

  disabled = false;
  value = false;

  onChange = (_: any) => { };
  onTouched = () => { };

  writeValue(val: any) { this.value = !!val; }
  registerOnChange(fn: any) { this.onChange = fn; }
  registerOnTouched(fn: any) { this.onTouched = fn; }
  setDisabledState(state: boolean) { this.disabled = state; }

  toggle(e: Event) {
    const v = (e.target as HTMLInputElement).checked;
    this.value = v;
    this.onChange(v);
  }
}
