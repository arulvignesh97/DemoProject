/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { LeadselfService } from './leadself.service';

describe('Service: Leadself', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LeadselfService]
    });
  });

  it('should ...', inject([LeadselfService], (service: LeadselfService) => {
    expect(service).toBeTruthy();
  }));
});
