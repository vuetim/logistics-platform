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
  selector: 'ui-textarea',
  standalone: true,
  templateUrl: './ui-textarea.component.html',
  styleUrls: ['./ui-textarea.component.css'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => UiTextareaComponent),
    multi: true
  }]
})
export class UiTextareaComponent implements ControlValueAccessor {

  @Input() placeholder = '';
  @Input() rows = 3;
  @Input() disabled = false;

  value: string = '';

  onChange = (_: any) => { };
  onTouched = () => { };

  writeValue(val: any) {
    this.value = val ?? '';
  }

  registerOnChange(fn: any) {
    this.onChange = fn;
  }

  registerOnTouched(fn: any) {
    this.onTouched = fn;
  }

  setDisabledState(state: boolean) {
    this.disabled = state;
  }

  update(val: string) {
    this.value = val;
    this.onChange(val);
  }
}
