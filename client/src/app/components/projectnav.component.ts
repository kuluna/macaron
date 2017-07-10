import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-projectnav',
  template:
    `
    <nav md-tab-nav-bar color="primary" *ngIf="projectId">
      <a md-tab-link [routerLink]="['/projects', projectId]" [active]="activeTabName == 'overview'">Overview</a>
      <a md-tab-link [routerLink]="['/projects', projectId, 'cases']" [active]="activeTabName == 'cases'">Cases</a>
      <a md-tab-link [routerLink]="['/projects', projectId, 'plans']" [active]="activeTabName == 'plans'">Plans</a>
      <a md-tab-link [routerLink]="['/projects', projectId, 'runs']" [active]="activeTabName == 'runs'">Runs</a>
    </nav>
    `
})
export class ProjectNavComponent {
  @Input()
  projectId: number;
  @Input()
  activeTabName: string;
}
