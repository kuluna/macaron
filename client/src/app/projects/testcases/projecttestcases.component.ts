import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProjectsClient, Testcase } from '../../apiclient.service';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'app-projecttestcases',
  templateUrl: './projecttestcases.component.html',
  styleUrls: ['./projecttestcases.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectTestcasesComponent implements OnInit {
  testcases: Testcase[];

  constructor(private activeRoute: ActivatedRoute, private projectsClient: ProjectsClient) { }

  ngOnInit() {
    this.activeRoute.params.map(params => params['projectId'] as number)
                           .switchMap(projectId => this.projectsClient.getTestcases(projectId))
                           .subscribe(testcases => this.testcases = testcases);
  }
}
