import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TotalX } from './total-x';

describe('TotalX', () => {
  let component: TotalX;
  let fixture: ComponentFixture<TotalX>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TotalX],
    }).compileComponents();

    fixture = TestBed.createComponent(TotalX);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
