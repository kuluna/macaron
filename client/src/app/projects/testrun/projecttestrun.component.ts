import { Component, OnInit } from '@angular/core';
import { MdSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectsClient, Case, Plan, RunCreateRequest, TestResult } from '../../apiclient.service';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'app-projecttestrun',
  templateUrl: './projecttestrun.component.html',
  styleUrls: ['./projecttestrun.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectTestrunComponent implements OnInit {
  projectId: number;
  testplans: Plan[] = [];

  selectTestplanId: number;
  selectTestplan: Plan;
  selectTestcaseId: number;
  selectTestcase: Case;
  testcasesRow = 0;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private snackBar: MdSnackBar,
              private projectsClient: ProjectsClient) { }

  ngOnInit() {
    this.route.params.map(params => Number(params['projectId']))
                     .do(projectId => this.projectId = projectId)
                     .switchMap(projectId => this.projectsClient.getPlans(projectId, true))
                     .subscribe(testplans => this.testplans = testplans);
/*
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
                            return this.testplans.find(t => t.id === this.selectTestplanId).cases.find(t => t.id === testcaseId);
                          })
                          .subscribe(testcase => {
                            this.selectTestcaseId = testcase.id;
                            this.selectTestcase = testcase;
                          });
*/
  }
/*
  onSelectTestplan(testplanId: number) {
    const testplan = this.testplans.find(t => t.id === testplanId);
    this.selectTestplanId = testplan.id;
    if (testplan.cases.length > 0) {
      // this.selectTestcaseId = testplan.cases[0].id;
    }

    this.router.navigate(['./'], {
      queryParams: { testplanId: this.selectTestplanId, testcaseId: this.selectTestcaseId },
      relativeTo: this.route
    });
  }

  onSelectTestcase(row: number) {
    const newTestcaseId = this.selectTestplan.cases[row].id;
    this.router.navigate(['./'], {
      queryParams: { testplanId: this.selectTestplanId, testcaseId: newTestcaseId },
      relativeTo: this.route
    });
    this.testcasesRow = row;
  }

  sendTestResult(testcase: Case, testplanId: number, result: TestResult) {
    const body = [ new RunCreateRequest(testcase.id, testcase.revision, result, 'string') ];
    this.projectsClient.postRun(this.projectId, testplanId, body)
                       .subscribe((testplan) => {
                         const update = this.selectTestplan.cases.find(t => t.id === testcase.id && t.revision === testcase.revision);
                         update.lastResult = result;
                         if (update.id === this.selectTestcaseId) {
                           this.selectTestcase = update;
                         }
                       }, error => {
                         this.snackBar.open('Error. Try again.', null, { duration: 1500 });
                       });
  }
*/
}
