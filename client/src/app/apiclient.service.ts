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

  getTestcases(projectId: number): Observable<Testcase[]> {
    return this.get<Testcase[]>('api/projects/' + projectId + '/testcases');
  }

  postTestcase(projectId: number, body: TestcaseCreateRequest): Observable<Testcase> {
    return this.post<Testcase>('api/projects/' + projectId + '/testcases', body);
  }

  getTestplans(projectId: number): Observable<Testplan[]> {
    return this.get<Testplan[]>('api/projects/' + projectId + '/testplans');
  }

  postTestrun(projectId: number, body: TestrunCreateRequest[]): Observable<Response> {
    return this.post<Response>('api/projects/' + projectId + '/testruns', body);
  }

  // abstracted http get function
  private get<T>(path: string): Observable<T> {
    return this.http.get(env.apiBaseAddress + path).map(res => res.json() as T);
  }

  // abstracted http post function
  private post<T>(path: string, body: any): Observable<T> {
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
  createdDate: Date;
  lastUpdateDate: Date;
}

export type CommitMode = 'Commited' | 'Add' | 'Modify' | 'Delete';

export type TestResult = 'NotTest' | 'Ok' | 'Ng';

export class Testplan {
  id: number;
  projectId: number;
  name: string;
  testcases: Testcase[];
  testruns: any;
  leaderId: string;
  dueDate: Date | null;
  completed: boolean;
  lastUpdateDate: Date;
}

// requests

export class TestcaseCreateRequest {
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
