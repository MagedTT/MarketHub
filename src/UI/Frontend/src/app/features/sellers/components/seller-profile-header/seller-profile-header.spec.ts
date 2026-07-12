import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerProfileHeader } from './seller-profile-header';

describe('SellerProfileHeader', () => {
  let component: SellerProfileHeader;
  let fixture: ComponentFixture<SellerProfileHeader>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SellerProfileHeader],
    }).compileComponents();

    fixture = TestBed.createComponent(SellerProfileHeader);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
