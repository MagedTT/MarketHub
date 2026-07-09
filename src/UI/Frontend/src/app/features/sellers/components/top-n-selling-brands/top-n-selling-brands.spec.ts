import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopNSellingBrands } from './top-n-selling-brands';

describe('TopNSellingBrands', () => {
  let component: TopNSellingBrands;
  let fixture: ComponentFixture<TopNSellingBrands>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TopNSellingBrands],
    }).compileComponents();

    fixture = TestBed.createComponent(TopNSellingBrands);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
