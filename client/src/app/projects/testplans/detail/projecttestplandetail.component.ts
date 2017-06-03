import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectsClient, Testcase, Testplan } from '../../../apiclient.service';
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
  grouped: Map<string, Testcase[]> = new Map();


  constructor(private route: ActivatedRoute,
              private api: ProjectsClient) {}

  ngOnInit() {
    const ids = this.route.params.map(params => {
      return { projectId: Number(params['projectId']), testplanId : Number(params['testplanId']) };
    }).shareReplay();

    this.projectId = ids.map(i => i.projectId);

    ids.switchMap(i => this.api.getTestplan(i.projectId, i.testplanId))
       .subscribe(testplan => {
         this.testplan = testplan;
         testplan.testcases.forEach(testcase => {
           if (!this.grouped.has(testcase.branchName)) {
             this.grouped.set(testcase.branchName, []);
           }

           const ts = this.grouped.get(testcase.branchName);
           ts.push(testcase);
           this.grouped.set(testcase.branchName, ts);
         });
       });
  }

}
