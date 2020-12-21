import { TestBed } from '@angular/core/testing';

import { RsaCypherService } from './rsa-cypher.service';

describe('RsaCypherService', () => {
  let service: RsaCypherService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RsaCypherService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
