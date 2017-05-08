import { Component, Directive, Input } from '@angular/core';

@Component({
  selector: 'app-projectnav',
  template:
    `
    <nav md-tab-nav-bar color="primary">
      <a md-tab-link [routerLink]="['./']" [active]="true">Overview</a>
      <a md-tab-link [routerLink]="['testcases']">Testcases</a>
      <a md-tab-link [routerLink]="['testplans']">Testplans</a>
      <a md-tab-link [routerLink]="['testrun']">Testrun</a>
    </nav>
    `
})
export class ProjectNavComponent {}
