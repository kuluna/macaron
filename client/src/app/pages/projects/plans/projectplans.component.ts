import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiClient, Plan } from '../../../services/apiclient.service';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'app-projectplans',
  templateUrl: './projectplans.component.html',
  styleUrls: ['./projectplans.component.scss'],
  providers: [ApiClient]
})
export class ProjectPlansComponent implements OnInit {
  projectId: Observable<number>;
  showCompletes = false;
  testplans: Observable<Plan>;

  activePlans: Plan[] = [] ;
  completedPlans: Plan[] = [];

  constructor(private route: ActivatedRoute, private api: ApiClient) { }

  ngOnInit() {
    this.projectId = this.route.params.map(params => Number(params['projectId']))
                                      .shareReplay();
    this.testplans = this.projectId.switchMap(id => this.api.getPlans(id, false))
                                   .switchMap(testplans => Observable.from(testplans))
                                   .publish().refCount();

    this.testplans.filter(testplan => !testplan.completed).subscribe(testplan => this.activePlans.push(testplan));
    this.testplans.filter(testplan => testplan.completed).subscribe(testplan => this.completedPlans.push(testplan));
  }
}
