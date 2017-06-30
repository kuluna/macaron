import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProjectsClient, GroupedCase } from '../../apiclient.service';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'app-projecttestcases',
  templateUrl: './projecttestcases.component.html',
  styleUrls: ['./projecttestcases.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectTestcasesComponent implements OnInit {
  projectId: Observable<number>;
  cases: GroupedCase[];

  constructor(private route: ActivatedRoute, private projectsClient: ProjectsClient) { }

  ngOnInit() {
    this.projectId = this.route.params.map(params => Number(params['projectId'])).shareReplay();

    this.projectId.switchMap(projectId => this.projectsClient.getGroupedCases(projectId))
                  .subscribe(cases => { this.cases = cases });
  }
}
