import { TestBed } from '@angular/core/testing';

import { AuthService2 } from './auth-service-2';

describe('AuthService2', () => {
  let service: AuthService2;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthService2);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
