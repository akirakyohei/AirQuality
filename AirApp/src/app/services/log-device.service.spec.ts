/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { LogDeviceService } from './log-device.service';

describe('Service: LogDevice', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LogDeviceService]
    });
  });

  it('should ...', inject([LogDeviceService], (service: LogDeviceService) => {
    expect(service).toBeTruthy();
  }));
});
