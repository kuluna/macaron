import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MdSnackBar } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiClient, Case, CaseCreateRequest } from '../../../../services/apiclient.service';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'app-projectcasenew',
  templateUrl: './projectcasenew.component.html',
  styleUrls: ['./projectcasenew.component.scss'],
  providers: [ApiClient]
})
export class ProjectCaseNewComponent implements OnInit {
  projectId: Observable<number>;

  submitting = false;
  moreCreate = false;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private snackBar: MdSnackBar,
              private api: ApiClient) { }

  ngOnInit() {
    this.projectId = this.route.params.map(params => Number(params['projectId'])).shareReplay();
  }

  onSubmit(form: NgForm) {
    const value = form.value as CaseCreateRequest;
    // fix md-slide-toggle has no default value
    value.isCarefully = value.isCarefully ? value.isCarefully : false;

    this.submitting = true;
    this.projectId.switchMap(projectId => this.api.postCase(projectId, value))
                  .subscribe(_ => {
                    this.snackBar.open('Created.', null, { duration: 1500 });
                    if (this.moreCreate) {
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
