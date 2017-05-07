import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectsClient, Testplan } from '../../apiclient.service';
import { Observable, ConnectableObservable } from 'rxjs/Rx';

@Component({
  selector: 'app-projecttestplans',
  templateUrl: './projecttestplans.component.html',
  styleUrls: ['./projecttestplans.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectTestplansComponent implements OnInit {
  testplans: Observable<Testplan>;

  activePlans: Testplan[] = [] ;
  completedPlans: Testplan[] = [];

  constructor(private activeRoute: ActivatedRoute, private projectsClient: ProjectsClient) { }

  ngOnInit() {
    this.testplans = this.activeRoute.params.map(params => params['projectId'] as number)
                           .switchMap(id => this.projectsClient.getTestplans(id))
                           .switchMap(testplans => Observable.from(testplans));

    this.testplans.filter(testplan => !testplan.completed).subscribe(testplan => this.activePlans.push(testplan));
    // this.testplans.filter(testplan => testplan.completed);
  }
}
