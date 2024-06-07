import { TestBed } from '@angular/core/testing';

import { AuthenticationHandler } from './authentication.service';

describe('AuthenticationService', () => {
  let service: AuthenticationHandler;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthenticationHandler);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
