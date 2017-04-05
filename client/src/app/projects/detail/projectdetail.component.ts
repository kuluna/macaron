import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { ProjectsService, Project, Platform } from '../../projects.service';

@Component({
  selector: 'app-projectdetail',
  templateUrl: './projectdetail.component.html',
  styleUrls: ['./projectdetail.component.scss'],
  providers: [ProjectsService]
})
export class ProjectDetailComponent implements OnInit {
  id: number;

  constructor(private activeRoute: ActivatedRoute, private projectsService: ProjectsService) { }

  ngOnInit() {
    this.activeRoute.params.subscribe(params => {
      this.id = params['id'];
    });
  }

}
