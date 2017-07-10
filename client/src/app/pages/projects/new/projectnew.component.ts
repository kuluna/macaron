import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MdSnackBar } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectsClient, ProjectCreateRequest } from '../../../services/apiclient.service';

@Component({
  selector: 'app-projectnew',
  templateUrl: './projectnew.component.html',
  styleUrls: ['./projectnew.component.scss'],
  providers: [ProjectsClient]
})
export class ProjectNewComponent implements OnInit {
  submitting = false;
  moreCreate = false;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private snackBar: MdSnackBar,
              private api: ProjectsClient) { }

  ngOnInit() {
  }

  onSubmit(form: NgForm) {
    const value = form.value as ProjectCreateRequest;

    this.submitting = true;
    this.api.addProject(value).subscribe(response => {
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
