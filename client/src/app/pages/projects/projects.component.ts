import { Component, OnInit } from '@angular/core';
import { ApiClient, Project } from '../../services/apiclient.service';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.scss'],
  providers: [ApiClient]
})
export class ProjectsComponent implements OnInit {
  projects: Project[];

  constructor(private api: ApiClient) { }

  ngOnInit() {
    this.api.getProjects().subscribe(projects => this.projects = projects);
  }
}
