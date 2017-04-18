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

  getProject(projectId: number): Observable<Project> {
    return this.http.get(env.apiBaseAddress + 'api/projects/' + projectId)
                    .map(res => res.json() as Project);
  }

  getMilestones(projectId: number): Observable<Milestone[]> {
    return this.http.get(env.apiBaseAddress + 'api/projects/' + projectId)
                    .map(res => res.json() as Milestone[]);
  }
}

export class Project {
  Id: number;
  Name: string;
  Description: string | null;
  Arcived: boolean;
  CreateDate: Date;
  LastUpdateDate: Date;
}

export class Milestone {
  Id: number;
  ProjectId: number;
  Progress: number;
  ExpectedCompleteDate: Date;
  CompleteDate: Date;
  Platforms: Array<Platform>;
  Testcases: Array<Testcase>;
}

export class Platform {
  Id: number;
  Name: string;
}

export class Testcase {
  Id: number;
  MilestoneId: number;
  TrackingId: number | null;
  BranchName: string;
  IsCommited: boolean;
  CommitMode: CommitMode;
  Order: number;
  MoreCareful: boolean;
  Estimates: number;
  Precondition: string | null;
  Test: string;
  Expect: string;
  LastUpdateDate: Date;
}

export enum CommitMode {
  Add, Modify, Delete
}
