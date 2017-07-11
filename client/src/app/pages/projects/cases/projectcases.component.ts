import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiClient, Case, GroupedCase } from '../../../services/apiclient.service';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'app-projectcases',
  templateUrl: './projectcases.component.html',
  styleUrls: ['./projectcases.component.scss'],
  providers: [ApiClient]
})
export class ProjectCasesComponent implements OnInit {
  projectId: Observable<number>;
  cases: GroupedCase[];

  constructor(private route: ActivatedRoute,
              private router: Router,
              private api: ApiClient) { }

  ngOnInit() {
    this.projectId = this.route.params.map(params => Number(params['projectId'])).shareReplay();

    this.projectId.switchMap(projectId => this.api.getGroupedCases(projectId))
                  .subscribe(cases => { this.cases = cases });
  }

  editCase(caseId: number) {
    this.router.navigate([caseId, 'edit'], { relativeTo: this.route });
  }
}
