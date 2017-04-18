import { TestBed, inject } from '@angular/core/testing';

import { ProjectsClient } from './apiclient.service';

describe('ApiclientService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ProjectsClient]
    });
  });

  it('should ...', inject([ProjectsClient], (service: ProjectsClient) => {
    expect(service).toBeTruthy();
  }));
});
