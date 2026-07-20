import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InactiveSeller } from './inactive-seller';

describe('InactiveSeller', () => {
  let component: InactiveSeller;
  let fixture: ComponentFixture<InactiveSeller>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InactiveSeller],
    }).compileComponents();

    fixture = TestBed.createComponent(InactiveSeller);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
