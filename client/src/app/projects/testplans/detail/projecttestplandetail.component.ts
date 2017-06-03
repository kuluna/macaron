import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectsClient, Testplan } from '../../../apiclient.service';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'app-projecttestplandetail',
  templateUrl: './projecttestplandetail.component.html',
  styleUrls: ['./projecttestplandetail.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectTestplanDetailComponent implements OnInit {
  projectId: Observable<number>;
  testplan: Testplan;

  constructor(private route: ActivatedRoute,
              private api: ProjectsClient) {}

  ngOnInit() {
    const ids = this.route.params.map(params => {
      return { projectId: Number(params['projectId']), testplanId : Number(params['testplanId']) };
    }).shareReplay();

    this.projectId = ids.map(i => i.projectId);

    ids.switchMap(i => this.api.getTestplan(i.projectId, i.testplanId))
       .subscribe(testplan => this.testplan = testplan);
  }

}
