import { Component, OnInit } from '@angular/core';
import { MdSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiClient, Case, Plan, RunCreateRequest, TestResult } from '../../../services/apiclient.service';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'app-projectrun',
  templateUrl: './projectrun.component.html',
  styleUrls: ['./projectrun.component.scss'],
  providers: [ApiClient]
})
export class ProjectRunComponent implements OnInit {
  projectId: Observable<number>;
  plans: Plan[] = [];

  selectPlanId: number;
  selectPlan: Plan;
  selectCaseIndex = 0;
  selectCase: Case;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private snackBar: MdSnackBar,
              private api: ApiClient) { }

  ngOnInit() {
    this.projectId = this.route.params.map(params => Number(params['projectId']))
                                      .shareReplay();

    this.projectId.switchMap(projectId => this.api.getPlans(projectId, true))
                  .subscribe(plans => this.plans = plans);
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
