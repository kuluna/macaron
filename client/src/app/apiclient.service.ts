import { Injectable } from '@angular/core';
import { environment as env } from '../environments/environment';

import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class ProjectsClient {

  constructor(private http: Http) { }

  getProjects(): Observable<Project[]> {
    return this.get<Project[]>('api/projects');
  }

  getProject(projectId: number, detail = true): Observable<Project> {
    return this.get<Project>('api/projects/' + projectId + '?detail=' + detail);
  }

  addProject(body: ProjectCreateRequest): Observable<Response> {
    return this.post<Response>('api/projects', body);
  }

  getTestcases(projectId: number): Observable<Testcase[]> {
    return this.get<Testcase[]>('api/projects/' + projectId + '/testcases');
  }

  postTestcase(projectId: number, body: TestcaseCreateRequest): Observable<Testcase> {
    return this.post<Testcase>('api/projects/' + projectId + '/testcases', body);
  }

  getTestplans(projectId: number, testable: boolean): Observable<Testplan[]> {
    return this.get<Testplan[]>('api/projects/' + projectId + '/testplans?testable=' + testable);
  }

  postTestplan(projectId: number, body: TestplanCreateRequest): Observable<Testplan> {
    return this.post<Testplan>('api/projects/' + projectId + '/testplans', body);
  }

  postTestrun(projectId: number, testplanId: number, body: TestrunCreateRequest[]): Observable<Testplan> {
    return this.post<Testplan>('api/projects/' + projectId + '/testplans/' + testplanId + '/testruns', body);
  }

  // abstracted http get function
  private get<T>(path: string): Observable<T> {
    console.log('run http get: ' + path);
    return this.http.get(env.apiBaseAddress + path).map(res => res.json() as T);
  }

  // abstracted http post function
  private post<T>(path: string, body: any): Observable<T> {
    console.log('run http post: ' + path);
    return this.http.post(env.apiBaseAddress + path, body).map(res => res.json() as T);
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
  revision: number;
  sectionName: string | null;
  branchName: string;
  commitMode: CommitMode;
  order: number;
  moreCareful: boolean;
  estimates: number;
  precondition: string | null;
  test: string;
  expect: string;
  testRuns: Testrun[];
  lastTestResult: TestResult;
  lastUpdateDate: Date;
}

export class Testplan {
  id: number;
  projectId: number;
  name: string;
  testcases: Testcase[];
  leader: any;
  dueDate: Date | null;
  completed: boolean;
  lastUpdateDate: Date;
}

export class Testrun {
  id: number;
  testplanId: number;
  testcaseId: number;
  revision: number;
  result: TestResult;
  testUser: any;
  createDate: Date;
  lastUpdateDate: Date;
}

// requests

export class ProjectCreateRequest {
  name: string;
  description: string | null;
}

export class TestcaseCreateRequest {
  sectionName: string | null;
  branchName: string;
  moreCareful: boolean;
  estimates: number;
  precondition: string | null;
  test: string;
  expect: string;
}

export class TestplanCreateRequest {
  name: string;
  testPattern: TestPattern;
  branchName: string | null;
  TestcaseIds: number[] | null;
  leaderName: string;
  dueDate: Date | null;
}

export class TestrunCreateRequest {
  constructor(
    public testcaseId: number,
    public revision: number,
    public result: TestResult,
    public testUsername: string) { }
}

// string enum

export type CommitMode = 'Commited' | 'Add' | 'Modify' | 'Delete';

export type TestResult = 'NotTest' | 'Ok' | 'Ng';

export type TestPattern = 'Branch' | 'Ng' | 'Custom';
