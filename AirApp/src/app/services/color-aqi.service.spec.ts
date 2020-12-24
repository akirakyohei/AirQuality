import { TestBed } from '@angular/core/testing';

import { ColorAqiService } from './color-aqi.service';

describe('ColorAqiService', () => {
  let service: ColorAqiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ColorAqiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
