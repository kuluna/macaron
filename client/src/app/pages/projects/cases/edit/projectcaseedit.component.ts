import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MdSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/Rx';

import { ApiClient, Case, CaseUpdateRequest } from '../../../../services/apiclient.service'

@Component({
  selector: 'app-projectcaseedit',
  templateUrl: './projectcaseedit.component.html',
  styleUrls: ['./projectcaseedit.component.scss'],
  providers: [ApiClient]
})
export class ProjectCaseEditComponent implements OnInit {
  identity: Observable<[number, number]>;
  targetCase: Case;

  sectionNames: string[] = [];
  filterdNames: string[] = [];

  submitting = false;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private snackBar: MdSnackBar,
              private api: ApiClient) { }

  ngOnInit() {
    this.identity = this.route.params.map(p => [Number(p['projectId']), Number(p['caseId'])])
                                     .shareReplay();

    this.identity.switchMap(([projectId, caseId]) => this.api.getCase(projectId, caseId))
                 .subscribe(c => this.targetCase = c);
    this.identity.switchMap(([id, _]) => this.api.getSectionNames(id))
                 .subscribe(s => this.sectionNames = s.sectionNames);
  }

  filter(name: string) {
    this.filterdNames = name ? this.sectionNames.filter(n => n.indexOf(name) === 0) : this.sectionNames;
  }

  onSubmit(value: CaseUpdateRequest) {
    this.submitting = true;

    this.identity.switchMap(([projectId, caseId]) => this.api.putCase(projectId, caseId, value))
                 .subscribe(res => {
                   this.snackBar.open('Updated.', null, { duration: 1500 });
                   this.router.navigate(['../../'], { relativeTo: this.route });
                 }, error => {
                    this.submitting = false;
                    console.warn(error);
                    this.snackBar.open('Error. Try again.', null, { duration: 1500 });
                 });
  }
}
