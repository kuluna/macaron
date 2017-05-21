import { Component, OnInit } from '@angular/core';
import { MdSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
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
  selectTestcase: Testcase;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private snackBar: MdSnackBar,
              private projectsClient: ProjectsClient) { }

  ngOnInit() {
    this.testplans = this.route.params.map(params => params['projectId'] as number)
                                      .do(projectId => this.projectId = projectId)
                                      .switchMap(projectId => this.projectsClient.getTestplans(projectId));

    /* not test yet
    this.route.queryParams.filter(query => query['testplanId'])
                          .map(query => query['testplanId'] as number)
                          .zip(this.testplans)
                          .map(([testplanId, testplans]) => testplans.find(testplan => testplan.id === testplanId))
                          .subscribe(testplan => this.selectTestplan = testplan);
    */
  }

  onSelectTestplan(testplan: Testplan) {
    this.router.navigate(['./'], {
      queryParams: { testplanId: testplan.id, testcaseId: testplan.testcases[0].id },
      relativeTo: this.route
    });
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
