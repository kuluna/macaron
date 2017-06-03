import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MdSnackBar } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectsClient, Testcase, TestcaseCreateRequest } from '../../../apiclient.service';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'app-projecttestcasenew',
  templateUrl: './projecttestcasenew.component.html',
  styleUrls: ['./projecttestcasenew.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectTestcaseNewComponent implements OnInit {
  projectId: Observable<number>;

  submitting = false;
  moreCreate = false;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private snackBar: MdSnackBar,
              private projectsClient: ProjectsClient) { }

  ngOnInit() {
    this.projectId = this.route.params.map(params => Number(params['projectId'])).shareReplay();
  }

  onSubmit(form: NgForm) {
    const value = form.value as TestcaseCreateRequest;
    // fix md-slide-toggle has no default value
    value.moreCareful = value.moreCareful ? value.moreCareful : false;

    this.submitting = true;
    this.projectId.switchMap(projectId => this.projectsClient.postTestcase(projectId, value))
                  .subscribe(testcase => {
                    this.snackBar.open('Created.', null, { duration: 1500 });
                    if (this.moreCreate) {
                      form.reset();
                      form.resetForm();
                      this.submitting = false;
                    } else {
                      this.router.navigate(['../'], { relativeTo: this.route });
                    }
                  }, error => {
                    this.submitting = false;
                    console.warn(error);
                    this.snackBar.open('Error. Try again.', null, { duration: 1500 });
                  });
  }
}
