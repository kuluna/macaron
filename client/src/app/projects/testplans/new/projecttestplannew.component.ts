import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MdSnackBar } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Rx';
import { ProjectsClient, Testcase, Testplan, TestplanCreateRequest } from '../../../apiclient.service';

@Component({
  selector: 'app-projecttestplannew',
  templateUrl: './projecttestplannew.component.html',
  styleUrls: ['./projecttestplannew.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectTestplanNewComponent implements OnInit {
  submitting = false;
  moreCreate = false;

  branchNames: string[] = [];

  constructor(private router: Router,
              private activeRoute: ActivatedRoute,
              private snackBar: MdSnackBar,
              private projectsClient: ProjectsClient) { }

  ngOnInit() {
    this.activeRoute.params.map(params => params['projectId'] as number)
                    .switchMap(projectId => this.projectsClient.getTestcases(projectId))
                    .switchMap(testcases => Observable.from(testcases))
                    .distinct(testcase => testcase.branchName)
                    .map(testcase => testcase.branchName)
                    .subscribe(branchName => this.branchNames.push(branchName));
  }

  onSubmit(form: NgForm) {
    const value = form.value as TestplanCreateRequest;
    this.submitting = true;

    this.activeRoute.params.map(params => params['projectId'] as number)
                    .switchMap(projectId => this.projectsClient.postTestplan(projectId, value))
                    .subscribe(testplan => {
                      this.snackBar.open('Created.', null, { duration: 1500 });
                      if (this.moreCreate) {
                        form.reset();
                        form.resetForm();
                        this.submitting = false;
                      } else {
                        this.router.navigate(['../'], { relativeTo: this.activeRoute });
                      }
                    }, error => {
                      this.submitting = false;
                      console.warn(error);
                      this.snackBar.open('Error. Try again.', null, { duration: 1500 });
                    });
  }
}
