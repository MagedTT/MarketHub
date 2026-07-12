import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePromoCodeModal } from './create-promo-code-modal';

describe('CreatePromoCodeModal', () => {
  let component: CreatePromoCodeModal;
  let fixture: ComponentFixture<CreatePromoCodeModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreatePromoCodeModal],
    }).compileComponents();

    fixture = TestBed.createComponent(CreatePromoCodeModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
