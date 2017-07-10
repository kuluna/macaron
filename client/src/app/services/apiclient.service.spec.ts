import { TestBed, inject } from '@angular/core/testing';
import { HttpModule } from '@angular/http';

import { ProjectsClient } from './apiclient.service';

describe('ApiclientService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpModule],
      providers: [ProjectsClient]
    });
  });

  it('should ...', inject([ProjectsClient], (service: ProjectsClient) => {
    expect(service).toBeTruthy();
  }));
});
