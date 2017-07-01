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

  getCases(projectId: number): Observable<Case[]> {
    return this.get<Case[]>('api/projects/' + projectId + '/cases');
  }

  getGroupedCases(projectId: number): Observable<GroupedCase[]> {
    return this.get<GroupedCase[]>('api/projects/' + projectId + '/cases?groupBySection=true');
  }

  postCase(projectId: number, body: CaseCreateRequest): Observable<Case> {
    return this.post<Case>('api/projects/' + projectId + '/cases', body);
  }

  getPlans(projectId: number, runnable: boolean): Observable<Plan[]> {
    return this.get<Plan[]>('api/projects/' + projectId + '/plans?runnable=' + runnable);
  }

  getPlan(projectId: number, planId: number): Observable<Plan> {
    return this.get<Plan>('api/projects/' + projectId + '/plans/' + planId);
  }

  postPlan(projectId: number, body: PlanCreateRequest): Observable<Plan> {
    return this.post<Plan>('api/projects/' + projectId + '/plans', body);
  }

  postRun(projectId: number, planId: number, body: RunCreateRequest[]): Observable<Plan> {
    return this.post<Plan>('api/projects/' + projectId + '/plans/' + planId + '/runs', body);
  }

  // generic http get function
  private get<T>(path: string): Observable<T> {
    console.log('run http get: ' + path);
    return this.http.get(env.apiBaseAddress + path).map(res => res.json() as T);
  }

  // generic http post function
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
  isArcived: boolean;
  cases: Case[];
  createDate: Date;
  lastUpdateDate: Date;
}

export class Case {
  id: number;
  projectId: number;
  revision: number;
  order: number;
  sectionName: string | null;
  isCarefully: boolean;
  estimates: number;
  precondition: string | null;
  step: string;
  Expectation: string | null;
  runs: Run[];
  lastResult: TestResult;
  lastUpdateDate: Date;
}

export class GroupedCase {
  sectionName: string;
  cases: Case[];
}

export class Plan {
  id: number;
  projectId: number;
  name: string;
  cases: GroupedCase[];
  runs: Run[];
  leaderName: string;
  dueDate: Date | null;
  completed: boolean;
  createdDate: Date;
  lastUpdateDate: Date;
}

export class Run {
  id: number;
  projectId: number;
  planId: number;
  caseId: number;
  caseRevision: number;
  result: TestResult;
  userName: string;
  createDate: Date;
  lastUpdateDate: Date;
}

export class CaseIdentity {
  id: number;
  revision: number;
}

// requests

export class ProjectCreateRequest {
  name: string;
  description: string | null;
}

export class CaseCreateRequest {
  sectionName: string | null;
  isCarefully: boolean;
  estimates: number;
  precondition: string | null;
  step: string;
  expectation: string | null;
}

export class PlanCreateRequest {
  name: string;
  testPattern: TestPattern;
  sections: string[] | null;
  CaseIds: CaseIdentity[] | null;
  leaderName: string;
  dueDate: Date | null;
}

export class RunCreateRequest {
  constructor(
    public caseId: number,
    public caseRevision: number,
    public result: TestResult,
    public userName: string) { }
}

// string enum

export type TestResult = 'NotTest' | 'Ok' | 'Ng';

export type TestPattern = 'Section' | 'Ng' | 'Custom';
