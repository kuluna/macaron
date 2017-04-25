import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProjectsClient, Testcase } from '../../../apiclient.service';

import { BodyContentComponent } from '../../../bodycontent.component';

@Component({
  selector: 'app-projecttestcasenew',
  templateUrl: './projecttestcasenew.component.html',
  styleUrls: ['./projecttestcasenew.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectTestcaseNewComponent implements OnInit {

  constructor(private activeRoute: ActivatedRoute, private projectsClient: ProjectsClient) { }

  ngOnInit() {
  }

}
