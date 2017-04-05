import { Injectable } from '@angular/core';
import { environment as env } from '../environments/environment';

import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class ProjectsService {

  constructor(private http: Http) { }

  getProjects(): Observable<Project[]> {
    return this.http.get(env.apiBaseAddress + 'api/projects')
                    .map(res => res.json() as Project[]);
  }
}

export class Project {
  id: number;
  name: string;
  platforms: Array<Platform>;
  arcived: boolean;
  createDate: Date;
  lastUpdateDate: Date;
}

export class Platform {
  id: number;
  name: string;
}
