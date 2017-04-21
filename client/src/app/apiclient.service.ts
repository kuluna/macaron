import { Injectable } from '@angular/core';
import { environment as env } from '../environments/environment';

import { Http } from '@angular/http';
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

  getMilestones(projectId: number): Observable<Milestone[]> {
    return this.http.get(env.apiBaseAddress + 'api/projects/' + projectId + '/milestones')
                    .map(res => res.json() as Milestone[]);
  }

  postMilestone(projectId: number, request: MilestoneCreateRequest): Observable<Milestone> {
    return this.http.post(env.apiBaseAddress + 'api/projects/' + projectId + '/milestones', request)
                    .map(res => res.json() as Milestone);
  }
}


// models

export class Project {
  id: number;
  name: string;
  description: string | null;
  arcived: boolean;
  milestones: Milestone[];
  createDate: Date;
  lastUpdateDate: Date;
}

export class Milestone {
  id: number;
  name: string;
  projectId: number;
  progress: number;
  expectedCompleteDate: Date;
  completeDate: Date;
  platforms: Platform[];
  testcases: Testcase[];
}

export class Platform {
  id: number;
  name: string;
}

export class Testcase {
  id: number;
  milestoneId: number;
  trackingId: number | null;
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
}

export enum CommitMode {
  Add, Modify, Delete
}

export class MilestoneCreateRequest {
  name: string;
}
