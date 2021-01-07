import { Pipe, PipeTransform } from '@angular/core';
import { stringify } from 'querystring';

@Pipe({
  name: 'placeholder'
})
export class PlaceholderPipe implements PipeTransform {

  transform(value: string, placeholder: string): string {
    // get rid of null undefined
    if (!value){
      return placeholder;
    }
    // not a string, not null/undefined, therefore a real value
    if (!value.trim){
      return value;
    }

    const trimmed = value.trim();
    // no whitespace;
    if (trimmed){
      return trimmed;
    } else{
      return placeholder;
    }
  }

}
