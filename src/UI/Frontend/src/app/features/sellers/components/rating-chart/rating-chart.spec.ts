import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RatingChart } from './rating-chart';

describe('RatingChart', () => {
  let component: RatingChart;
  let fixture: ComponentFixture<RatingChart>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RatingChart],
    }).compileComponents();

    fixture = TestBed.createComponent(RatingChart);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
