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
  testplans: Testplan[] = [];

  selectTestplanId: number;
  selectTestplan: Testplan;
  selectTestcaseId: number;
  selectTestcase: Testcase;
  testcasesRow = 0;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private snackBar: MdSnackBar,
              private projectsClient: ProjectsClient) { }

  ngOnInit() {
    this.route.params.map(params => Number(params['projectId']))
                     .do(projectId => this.projectId = projectId)
                     .switchMap(projectId => this.projectsClient.getTestplans(projectId, true))
                     .subscribe(testplans => this.testplans = testplans);

    this.route.queryParams.filter(query => query['testplanId'])
                          .map(query => Number(query['testplanId']))
                          .filter(f => this.testplans.length > 0)
                          .map((testplanId) => this.testplans.find(t => t.id === testplanId))
                          .subscribe(testplan => {
                            this.selectTestplan = testplan;
                            this.selectTestplanId = testplan.id;
                          });

    this.route.queryParams.filter(query => query['testcaseId'])
                          .map(query => Number(query['testcaseId']))
                          .filter(f => this.testplans.length > 0)
                          .map(testcaseId => {
                            return this.testplans.find(t => t.id === this.selectTestplanId).testcases.find(t => t.id === testcaseId);
                          })
                          .subscribe(testcase => {
                            this.selectTestcaseId = testcase.id;
                            this.selectTestcase = testcase;
                          });
    this.route.queryParams.subscribe(params => console.log('update queries.'));
  }

  onSelectTestplan(testplanId: number) {
    const testplan = this.testplans.find(t => t.id === testplanId);
    this.selectTestplanId = testplan.id;
    if (testplan.testcases.length > 0) {
      this.selectTestcaseId = testplan.testcases[0].id;
    }

    this.router.navigate(['./'], {
      queryParams: { testplanId: this.selectTestplanId, testcaseId: this.selectTestcaseId },
      relativeTo: this.route
    });
  }

  onSelectTestcase(row: number) {
    const newTestcaseId = this.selectTestplan.testcases[row].id;
    this.router.navigate(['./'], {
      queryParams: { testplanId: this.selectTestplanId, testcaseId: newTestcaseId },
      relativeTo: this.route
    });
    this.testcasesRow = row;
  }

  sendTestResult(testcase: Testcase, testplanId: number, result: TestResult) {
    const body = [ new TestrunCreateRequest(testcase.id, testcase.revision, result, 'string') ];
    this.projectsClient.postTestrun(this.projectId, testplanId, body)
                       .subscribe((testplan) => {
                         const update = this.selectTestplan.testcases.find(t => t.id === testcase.id && t.revision === testcase.revision);
                         update.lastTestResult = result;
                         if (update.id === this.selectTestcaseId) {
                           this.selectTestcase = update;
                         }
                       }, error => {
                         this.snackBar.open('Error. Try again.', null, { duration: 1500 });
                       });
  }
}
