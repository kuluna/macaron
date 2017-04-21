import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectsClient, Milestone, Platform, Testcase } from '../../apiclient.service';

@Component({
  selector: 'app-projectmilestones',
  templateUrl: './projectmilestones.component.html',
  styleUrls: ['./projectmilestones.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectMilestonesComponent implements OnInit {
  milestones: Array<Milestone>;

  constructor(private activeRoute: ActivatedRoute, private projectsClient: ProjectsClient) { }

  ngOnInit() {
    this.activeRoute.params.map(params => params['projectId'] as number)
                           .switchMap(projectId => this.projectsClient.getMilestones(projectId))
                           .subscribe(milestones => this.milestones = milestones);
  }
}
