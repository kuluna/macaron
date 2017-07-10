import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectsClient, Project } from '../../../services/apiclient.service';

@Component({
  selector: 'app-projectdetail',
  templateUrl: './projectdetail.component.html',
  styleUrls: ['./projectdetail.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectDetailComponent implements OnInit {
  project: Project;

  constructor(private activeRoute: ActivatedRoute, private projectsClient: ProjectsClient) { }

  ngOnInit() {
    this.activeRoute.params.map(params => params['projectId'] as number)
                           .switchMap(id => this.projectsClient.getProject(id))
                           .subscribe(project => this.project = project);
  }
}
