import { Injectable } from '@angular/core';
import { environment as env } from '../environments/environment';

import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class ProjectsClient {

  constructor(private http: Http) { }

  getProjects(): Observable<Project[]> {
    return this.http.get(env.apiBaseAddress + 'api/projects')
                    .map(res => res.json() as Project[]);
  }

  getProject(projectId: number, detail = true): Observable<Project> {
    return this.http.get(env.apiBaseAddress + 'api/projects/' + projectId + '?detail=' + detail)
                    .map(res => res.json() as Project);
  }

  getTestcases(projectId: number): Observable<Testcase[]> {
    return this.http.get(env.apiBaseAddress + 'api/projects/' + projectId + '/testcases')
                    .map(res => res.json() as Testcase[]);
  }

  postTestcase(projectId: number, body: TestcaseCreateRequest): Observable<Testcase> {
    return this.http.post(env.apiBaseAddress + 'api/projects/' + projectId + '/testcases', body)
                    .map(res => res.json() as Testcase);
  }

  postTestrun(projectId: number, body: TestrunCreateRequest[]): Observable<Response> {
    return this.http.post(env.apiBaseAddress + 'api/projects/' + projectId + '/testruns', body);
  }
}

// models

export class Project {
  id: number;
  name: string;
  description: string | null;
  arcived: boolean;
  testcases: Testcase[];
  createDate: Date;
  lastUpdateDate: Date;
}

export class Testcase {
  id: number;
  projectId: number;
  trackingId: number | null;
  sectionName: string | null;
  branchName: string;
  isCommited: boolean;
  commitMode: CommitMode;
  order: number;
  moreCareful: boolean;
  estimates: number;
  precondition: string | null;
  test: string;
  expect: string;
  lastUpdateDate: Date;
  lastTestResult: TestResult;
}

export type CommitMode = 'Add' | 'Modify' | 'Delete';

export type TestResult = 'NotTest' | 'Ok' | 'Ng';

// requests

export class TestcaseCreateRequest {
  trackingId: number | null;
  sectionName: string | null;
  branchName: string;
  moreCareful: boolean;
  estimates: number;
  precondition: string | null;
  test: string;
  expect: string;
}

export class TestrunCreateRequest {
  constructor(
    public testcaseId: number,
    public milestoneId: number | null,
    public result: TestResult,
    public testUserId: string) { }
}
