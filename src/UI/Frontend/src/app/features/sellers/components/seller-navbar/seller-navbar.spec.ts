import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerNavbar } from './seller-navbar';

describe('SellerNavbar', () => {
  let component: SellerNavbar;
  let fixture: ComponentFixture<SellerNavbar>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SellerNavbar],
    }).compileComponents();

    fixture = TestBed.createComponent(SellerNavbar);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
