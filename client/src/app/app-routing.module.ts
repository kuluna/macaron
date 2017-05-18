import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SigninComponent } from './user/signin/signin.component';
import { ProjectsComponent } from './projects/projects.component';
import { ProjectDetailComponent } from './projects/detail/projectdetail.component';
import { ProjectTestcasesComponent } from './projects/testcases/projecttestcases.component';
import { ProjectTestcaseNewComponent } from './projects/testcases/new/projecttestcasenew.component';
import { ProjectTestplansComponent } from './projects/testplans/projecttestplans.component';
import { ProjectTestrunComponent } from './projects/testrun/projecttestrun.component';
import { ProjectTestplanNewComponent } from './projects/testplans/new/projecttestplannew.component';

const routes: Routes = [
  { path: 'user/signin', component: SigninComponent },
  { path: 'projects', component: ProjectsComponent },
  { path: 'projects/:projectId', children: [
      { path: '', component: ProjectDetailComponent },
      { path: 'testcases', children: [
        { path: '', component: ProjectTestcasesComponent },
        { path: 'new', component: ProjectTestcaseNewComponent }
      ]},
      { path: 'testplans', children: [
        { path: '', component: ProjectTestplansComponent },
        { path: 'new', component: ProjectTestplanNewComponent }
      ] },
      { path: 'testruns', component: ProjectTestrunComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
