import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-total-x',
  imports: [],
  templateUrl: './total-x.html',
  styleUrl: './total-x.css',
})
export class TotalX {
  @Input() text: string = '';
  @Input() value: number = 0;
}
