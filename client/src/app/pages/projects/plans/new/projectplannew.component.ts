import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MdSnackBar, MdCheckboxChange } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Rx';
import { ApiClient, Case, Plan, PlanCreateRequest } from '../../../../services/apiclient.service';

@Component({
  selector: 'app-projectplannew',
  templateUrl: './projectplannew.component.html',
  styleUrls: ['./projectplannew.component.scss'],
  providers: [ApiClient]
})
export class ProjectPlanNewComponent implements OnInit {
  projectId: Observable<number>;
  submitting = false;
  moreCreate = false;
  selectedSections: string[] = [];

  sectionNames: Observable<string[]>;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private snackBar: MdSnackBar,
              private api: ApiClient) { }

  ngOnInit() {
    this.projectId = this.route.params.map(params => Number(params['projectId']))
                                      .shareReplay();
    this.sectionNames = this.projectId.switchMap(id => this.api.getGroupedCases(id))
                                      .map(cases => cases.map(group => group.sectionName));
  }

  onChangeSelect(e: MdCheckboxChange) {
    if (e.checked) {
      if (this.selectedSections.indexOf(e.source.value) === -1) {
        this.selectedSections.push(e.source.value);
      }
    } else {
      const index = this.selectedSections.indexOf(e.source.value);
      this.selectedSections.splice(index, 1);
    }
    console.log(this.selectedSections);
  }

  onSubmit(form: NgForm) {
    const value = form.value as PlanCreateRequest;
    value.sections = this.selectedSections;
    this.submitting = true;

    this.route.params.map(params => Number(params['projectId']))
                     .switchMap(projectId => this.api.postPlan(projectId, value))
                     .subscribe(testplan => {
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
