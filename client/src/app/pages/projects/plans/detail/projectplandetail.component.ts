import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiClient, GroupedCase, GroupedPlan, PlanUpdateRequest } from '../../../../services/apiclient.service';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'app-projectplandetail',
  templateUrl: './projectplandetail.component.html',
  styleUrls: ['./projectplandetail.component.scss'],
  providers: [ApiClient]
})
export class ProjectPlanDetailComponent implements OnInit {
  projectId: Observable<number>;
  plan: GroupedPlan;

  submitting = false;

  constructor(private route: ActivatedRoute,
              private api: ApiClient) {}

  ngOnInit() {
    const ids = this.route.params.map(params => {
      return { projectId: Number(params['projectId']), planId : Number(params['planId']) };
    }).shareReplay();

    this.projectId = ids.map(i => i.projectId);

    ids.switchMap(i => this.api.getGroupedPlan(i.projectId, i.planId))
       .subscribe(plan => {
         this.plan = plan;
       });
  }

  toggleComplete() {
    this.submitting = true;
    const body = new PlanUpdateRequest(this.plan, true);
    console.log(body);

    this.api.putPlan(this.plan.projectId, this.plan.id, body)
            .subscribe(plan => { this.plan.completed = true });
  }
}
