import { Component, OnInit } from '@angular/core';
import { MdSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { ProjectsClient, Testcase, TestrunCreateRequest, TestResult } from '../../apiclient.service';

@Component({
  selector: 'app-projecttestrun',
  templateUrl: './projecttestrun.component.html',
  styleUrls: ['./projecttestrun.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectTestrunComponent implements OnInit {
  projectId: number;
  testcases: Testcase[];

  constructor(private activeRoute: ActivatedRoute,
              private snackBar: MdSnackBar,
              private projectsClient: ProjectsClient) { }

  ngOnInit() {
    this.activeRoute.params.map(params => params['projectId'] as number)
                           .do(projectId => this.projectId = projectId)
                           .switchMap(projectId => this.projectsClient.getTestcases(projectId))
                           .subscribe(testcases => this.testcases = testcases);
  }

  sendTestResult(testcase: Testcase, milestoneId: number | null, result: TestResult) {
    const body = [ new TestrunCreateRequest(testcase.id, milestoneId, result, '00850bcb-b437-44fb-b184-a9af70a1abbf') ];
    this.projectsClient.postTestrun(this.projectId, body)
                       .subscribe(() => this.ngOnInit(), error => this.snackBar.open('Error. Try again.', null, { duration: 1500 }));
  }
}
