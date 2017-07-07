import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectsClient, Plan } from '../../apiclient.service';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'app-projecttestplans',
  templateUrl: './projecttestplans.component.html',
  styleUrls: ['./projecttestplans.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectTestplansComponent implements OnInit {
  projectId: Observable<number>;
  showCompletes = false;
  testplans: Observable<Plan>;

  activePlans: Plan[] = [] ;
  completedPlans: Plan[] = [];

  constructor(private activeRoute: ActivatedRoute, private projectsClient: ProjectsClient) { }

  ngOnInit() {
    this.projectId = this.activeRoute.params.map(params => Number(params['projectId']))
                                            .shareReplay();
    this.testplans = this.projectId.switchMap(id => this.projectsClient.getPlans(id, false))
                                   .switchMap(testplans => Observable.from(testplans))
                                   .publish().refCount();

    this.testplans.filter(testplan => !testplan.completed).subscribe(testplan => this.activePlans.push(testplan));
    this.testplans.filter(testplan => testplan.completed).subscribe(testplan => this.completedPlans.push(testplan));
  }
}
