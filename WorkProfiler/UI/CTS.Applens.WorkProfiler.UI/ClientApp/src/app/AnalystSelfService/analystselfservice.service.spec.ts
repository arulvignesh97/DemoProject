import { TestBed } from '@angular/core/testing';

import { AnalystselfserviceService } from './analystselfservice.service';

describe('AnalystselfserviceService', () => {
  let service: AnalystselfserviceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AnalystselfserviceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
