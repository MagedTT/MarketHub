import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditPromoCodeModal } from './edit-promo-code-modal';

describe('EditPromoCodeModal', () => {
  let component: EditPromoCodeModal;
  let fixture: ComponentFixture<EditPromoCodeModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditPromoCodeModal],
    }).compileComponents();

    fixture = TestBed.createComponent(EditPromoCodeModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
