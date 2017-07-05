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
  projectId: Observable<number>;
  plans: Plan[] = [];

  selectPlanId: number;
  selectPlan: Plan;
  selectCaseIndex = 0;
  selectCase: Case;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private snackBar: MdSnackBar,
              private api: ProjectsClient) { }

  ngOnInit() {
    this.projectId = this.route.params.map(params => Number(params['projectId']))
                                      .shareReplay();

    this.projectId.switchMap(projectId => this.api.getPlans(projectId, true))
                  .subscribe(plans => this.plans = plans);
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

  onSelectPlan(planid: number) {
    this.selectPlan = this.plans.filter(p => p.id === planid)[0];
    this.selectCase = this.selectPlan.cases[0];
  }

  onSelectCase(row: number) {
    this.selectCaseIndex = row;
    this.selectCase = this.selectPlan.cases[row];
  }

  sendResult(targetCase: Case, planId: number, result: TestResult) {
    const body = [ new RunCreateRequest(targetCase.id, targetCase.revision, result, 'string') ];

    this.projectId.switchMap(pId => this.api.postRun(pId, planId, body))
                  .subscribe((plan) => {
                  }, error => {
                    this.snackBar.open('Error. Try again.', null, { duration: 1500 });
                  });
  }
}
