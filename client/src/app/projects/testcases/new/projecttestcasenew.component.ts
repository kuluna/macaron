import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MdSnackBar } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectsClient, Testcase, TestcaseCreateRequest } from '../../../apiclient.service';

import { BodyContentComponent } from '../../../bodycontent.component';

@Component({
  selector: 'app-projecttestcasenew',
  templateUrl: './projecttestcasenew.component.html',
  styleUrls: ['./projecttestcasenew.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectTestcaseNewComponent implements OnInit {
  submitting: boolean;

  alwaysOk = false;

  constructor(private router: Router,
              private activeRoute: ActivatedRoute,
              private snackBar: MdSnackBar,
              private projectsClient: ProjectsClient) { }

  ngOnInit() {
  }

  onSubmit(value: TestcaseCreateRequest, form: NgForm) {
    // fix md-slide-toggle has no default value
    value.moreCareful = value.moreCareful ? value.moreCareful : false;

    console.log(JSON.stringify(value));
    this.submitting = true;

    this.activeRoute.params.map(params => params['projectId'] as number)
                           .switchMap(projectId => this.projectsClient.postTestcase(projectId, value))
                           .subscribe(testcase => {
                             this.snackBar.open('Created.', null, { duration: 1500 });
                             this.router.navigate(['../'], { relativeTo: this.activeRoute });
                           }, error => {
                             this.submitting = false;
                             console.log(error);
                             this.snackBar.open(error, null, { duration: 1500 });
                           });
  }
}
