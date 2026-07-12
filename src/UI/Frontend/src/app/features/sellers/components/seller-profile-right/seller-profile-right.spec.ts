import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerProfileRight } from './seller-profile-right';

describe('SellerProfileRight', () => {
  let component: SellerProfileRight;
  let fixture: ComponentFixture<SellerProfileRight>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SellerProfileRight],
    }).compileComponents();

    fixture = TestBed.createComponent(SellerProfileRight);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
