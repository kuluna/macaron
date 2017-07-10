import { TestBed, inject } from '@angular/core/testing';
import { HttpModule } from '@angular/http';

import { ApiClient } from './apiclient.service';

describe('ApiclientService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpModule],
      providers: [ApiClient]
    });
  });

  it('should ...', inject([ApiClient], (service: ApiClient) => {
    expect(service).toBeTruthy();
  }));
});
