import { Component, OnInit } from '@angular/core';
import { ProjectsClient, Project } from '../apiclient.service';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectsComponent implements OnInit {
  projects: Project[];

  constructor(private projectsClient: ProjectsClient) { }

  ngOnInit() {
    this.projectsClient.getProjects()
        .subscribe(projects => {
          this.projects = projects;
          console.log(JSON.stringify(projects));
        });
  }
}
