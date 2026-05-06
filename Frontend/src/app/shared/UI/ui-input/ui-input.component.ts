import {
  Component,
  Input,
  forwardRef
} from '@angular/core';
import {
  ControlValueAccessor,
  NG_VALUE_ACCESSOR
} from '@angular/forms';

@Component({
  selector: 'ui-input',
  standalone: true,
  templateUrl: './ui-input.component.html',
  styleUrls: ['./ui-input.component.css'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => UiInputComponent),
    multi: true
  }]
})
export class UiInputComponent implements ControlValueAccessor {

  @Input() type: 'text' | 'number' | 'email' | 'tel' | 'password' | 'date' = 'text';
  @Input() placeholder = '';
  @Input() disabled = false;
  @Input() min?: number;
  @Input() max?: number;
  @Input() step?: number;

  value: any = '';

  onChange = (_: any) => { };
  onTouched = () => { };

  writeValue(val: any) { this.value = val; }
  registerOnChange(fn: any) { this.onChange = fn; }
  registerOnTouched(fn: any) { this.onTouched = fn; }
  setDisabledState(state: boolean) { this.disabled = state; }

  update(val: any) {
    if (this.type === 'number') val = val === '' ? null : Number(val);
    this.value = val;
    this.onChange(val);
  }
}
