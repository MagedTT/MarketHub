import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'trim',
})
export class TrimPipe implements PipeTransform {
  transform(value: string, maximumCharacters: number = 30, delimiter: string = '...'): string {
    if (!value) return '';

    if (value.length <= maximumCharacters) return value;

    return value.substring(0, maximumCharacters) + delimiter;
  }
}