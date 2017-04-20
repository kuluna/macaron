import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectsClient, Project, Milestone } from '../../apiclient.service';

@Component({
  selector: 'app-projectoverview',
  templateUrl: './projectoverview.component.html',
  styleUrls: ['./projectoverview.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectOverviewComponent implements OnInit {
  project: Project;

  constructor(private activeRoute: ActivatedRoute, private projectsClient: ProjectsClient) { }

  ngOnInit() {
    this.activeRoute.params.map(params => params['id'] as number)
                           .switchMap(id => this.projectsClient.getProject(id))
                           .subscribe(project => this.project = project);
  }
}
