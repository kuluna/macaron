import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-projectnav',
  template:
    `
    <nav md-tab-nav-bar color="primary" *ngIf="projectId">
      <a md-tab-link [routerLink]="['/projects', projectId]" [active]="activeTabName == 'overview'">Overview</a>
      <a md-tab-link [routerLink]="['/projects', projectId, 'testcases']" [active]="activeTabName == 'testcases'">Testcases</a>
      <a md-tab-link [routerLink]="['/projects', projectId, 'testplans']" [active]="activeTabName == 'testplans'">Testplans</a>
      <a md-tab-link [routerLink]="['/projects', projectId, 'testruns']" [active]="activeTabName == 'testruns'">Testruns</a>
    </nav>
    `
})
export class ProjectNavComponent {
  @Input()
  projectId: number;
  @Input()
  activeTabName: string;
}
