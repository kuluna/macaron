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

  selectTestplanId: number;
  selectTestplan: Testplan;
  selectTestcaseId: number;
  selectTestcase: Testcase;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private snackBar: MdSnackBar,
              private projectsClient: ProjectsClient) { }

  ngOnInit() {
    this.testplans = this.route.params.map(params => Number(params['projectId']))
                                      .do(projectId => this.projectId = projectId)
                                      .switchMap(projectId => this.projectsClient.getTestplans(projectId));

    this.route.queryParams.filter(query => query['testplanId'])
                          .map(query => Number(query['testplanId']))
                          .zip(this.testplans)
                          .map(([testplanId, testplans]) => testplans.find(t => t.id === testplanId))
                          .subscribe(testplan => {
                            this.selectTestplan = testplan;
                            this.selectTestplanId = testplan.id;
                          });

    this.route.queryParams.filter(query => query['testcaseId'])
                          .map(query => Number(query['testcaseId']))
                          .zip(this.testplans)
                          .map(([testcaseId, testplans]) => testplans.find(t => t.id === this.selectTestplanId).testcases.find(t => t.id === testcaseId))
                          .subscribe(testcase => {
                            this.selectTestcaseId = testcase.id;
                            this.selectTestcase = testcase;
                          });
  }

  onSelectTestplan(testplanId: number) {
    this.testplans.map(testplans => testplans.find(t => t.id === testplanId))
                  .subscribe(testplan => {
                    this.selectTestplanId = testplan.id;
                    this.selectTestcaseId = testplan.testcases[0].id;

                    this.router.navigate(['./'], {
                      queryParams: { testplanId: this.selectTestplanId, testcaseId: this.selectTestcaseId },
                      relativeTo: this.route
                    });
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
