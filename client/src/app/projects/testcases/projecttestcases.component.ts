import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProjectsClient, Testcase } from '../../apiclient.service';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'app-projecttestcases',
  templateUrl: './projecttestcases.component.html',
  styleUrls: ['./projecttestcases.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectTestcasesComponent implements OnInit {
  projectId: Observable<number>;
  groupd: Map<string, Testcase[]> = new Map();

  constructor(private activeRoute: ActivatedRoute, private projectsClient: ProjectsClient) { }

  ngOnInit() {
    this.projectId = this.activeRoute.params.map(params => params['projectId'] as number)
                                            .shareReplay();

    this.projectId.switchMap(projectId => this.projectsClient.getTestcases(projectId))
                  .subscribe(testcases => {
                    testcases.forEach(testcase => {
                      if (!this.groupd.has(testcase.sectionName)) {
                        this.groupd.set(testcase.sectionName, []);
                      }

                      const ts = this.groupd.get(testcase.sectionName);
                      ts.push(testcase);
                      this.groupd.set(testcase.sectionName, ts);
                    });
                  });
  }
}
