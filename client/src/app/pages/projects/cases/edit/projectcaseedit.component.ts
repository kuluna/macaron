import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiClient } from '../../../../services/apiclient.service'
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'app-projectcaseedit',
  templateUrl: './projectcaseedit.component.html',
  styleUrls: ['./projectcaseedit.component.scss'],
  providers: [ApiClient]
})
export class ProjectCaseEditComponent implements OnInit {
  identity: Observable<Identity>;

  constructor(private route: ActivatedRoute,
              private api: ApiClient) { }

  ngOnInit() {
    this.identity = this.route.params.map(p => new Identity(Number(p['projectId']), Number(p['caseId'])))
                                     .shareReplay();
  }

}

class Identity {
  constructor(public projectId: number, public caseId: number) { }
}
