import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiClient, Project } from '../../../services/apiclient.service';

@Component({
  selector: 'app-projectdetail',
  templateUrl: './projectdetail.component.html',
  styleUrls: ['./projectdetail.component.scss'],
  providers: [ApiClient]
})
export class ProjectDetailComponent implements OnInit {
  project: Project;

  constructor(private route: ActivatedRoute, private api: ApiClient) { }

  ngOnInit() {
    this.route.params.map(params => Number(params['projectId']))
                     .switchMap(id => this.api.getProject(id))
                     .subscribe(project => this.project = project);
  }
}
