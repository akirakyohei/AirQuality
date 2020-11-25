import { TestBed } from '@angular/core/testing';

import { ReportAqiService } from './report-aqi.service';

describe('ReportAqiService', () => {
  let service: ReportAqiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ReportAqiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
