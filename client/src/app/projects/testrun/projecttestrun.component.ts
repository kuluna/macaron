import { Component, OnInit } from '@angular/core';
import { MdSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { ProjectsClient, Testcase, Testplan, TestrunCreateRequest, TestResult } from '../../apiclient.service';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'app-projecttestrun',
  templateUrl: './projecttestrun.component.html',
  styleUrls: ['./projecttestrun.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectTestrunComponent implements OnInit {
  projectId: number;
  testplans: Observable<Testplan[]>;

  selectTestplan: Testplan;

  constructor(private activeRoute: ActivatedRoute,
              private snackBar: MdSnackBar,
              private projectsClient: ProjectsClient) { }

  ngOnInit() {
    this.testplans = this.activeRoute.params.map(params => params['projectId'] as number)
                           .do(projectId => this.projectId = projectId)
                           .switchMap(projectId => this.projectsClient.getTestplans(projectId));
  }

  sendTestResult(testcase: Testcase, testplanId: number, result: TestResult) {
    const body = [ new TestrunCreateRequest(testcase.id, testcase.revision, result, 'string') ];
    this.projectsClient.postTestrun(this.projectId, testplanId, body)
                       .subscribe((testplan) => {
                         const update = this.selectTestplan.testcases.find(t => t.id === testcase.id && t.revision === testcase.revision);
                         update.lastTestResult = result;
                       }, error => {
                         this.snackBar.open('Error. Try again.', null, { duration: 1500 });
                       });
  }
}
