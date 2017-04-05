import { Component, OnInit } from '@angular/core';
import { ProjectsService, Project, Platform } from '../projects.service';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.scss'],
  providers: [ProjectsService]
})
export class ProjectsComponent implements OnInit {
  projects: Project[];

  constructor(private projectsService: ProjectsService) { }

  ngOnInit() {
    this.projectsService.getProjects()
        .subscribe(projects => {
          this.projects = projects;
          console.log(JSON.stringify(projects));
        });
  }
}
