import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectsClient, Case, Plan } from '../../../apiclient.service';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'app-projecttestplandetail',
  templateUrl: './projecttestplandetail.component.html',
  styleUrls: ['./projecttestplandetail.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectTestplanDetailComponent implements OnInit {
  projectId: Observable<number>;
  testplan: Plan;
  grouped: Map<string, Case[]> = new Map();


  constructor(private route: ActivatedRoute,
              private api: ProjectsClient) {}

  ngOnInit() {
    const ids = this.route.params.map(params => {
      return { projectId: Number(params['projectId']), testplanId : Number(params['testplanId']) };
    }).shareReplay();

    this.projectId = ids.map(i => i.projectId);

    ids.switchMap(i => this.api.getPlan(i.projectId, i.testplanId))
       .subscribe(testplan => {
         this.testplan = testplan;
         testplan.cases.forEach(testcase => {
           if (!this.grouped.has(testcase.sectionName)) {
             this.grouped.set(testcase.sectionName, []);
           }

           const ts = this.grouped.get(testcase.sectionName);
           ts.push(testcase);
           this.grouped.set(testcase.sectionName, ts);
         });
       });
  }

}
