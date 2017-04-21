import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectsClient, MilestoneCreateRequest, Milestone } from '../../../apiclient.service';

@Component({
  selector: 'app-projectmilestonenew',
  templateUrl: './projectmilestonenew.component.html',
  styleUrls: ['./projectmilestonenew.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectMilestoneNewComponent implements OnInit {

  constructor(private router: Router, private activeRoute: ActivatedRoute, private projectsClient: ProjectsClient) { }

  ngOnInit() {
  }

  createMilestone(name: string) {
    const request = new MilestoneCreateRequest();
    request.name = name;
    this.activeRoute.params.map(params => params['projectId'] as number)
                           .switchMap(projectId => this.projectsClient.postMilestone(projectId, request))
                           .map(newMilestone => newMilestone.id)
                           .subscribe(milestoneId => this.router.navigate(['../'], { relativeTo: this.activeRoute }));
  }

}
