import { TestBed } from '@angular/core/testing';

import { MainspringMetricsService } from './mainspring-metrics.service';

describe('MainspringMetricsService', () => {
  let service: MainspringMetricsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MainspringMetricsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
