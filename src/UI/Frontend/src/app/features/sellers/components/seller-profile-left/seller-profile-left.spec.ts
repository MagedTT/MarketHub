import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerProfileLeft } from './seller-profile-left';

describe('SellerProfileLeft', () => {
  let component: SellerProfileLeft;
  let fixture: ComponentFixture<SellerProfileLeft>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SellerProfileLeft],
    }).compileComponents();

    fixture = TestBed.createComponent(SellerProfileLeft);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
