import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SigninComponent } from './user/signin/signin.component';

import { ProjectsComponent } from './projects/projects.component';
import { ProjectDetailComponent } from './projects/detail/projectdetail.component';
import { ProjectNewComponent } from './projects/new/projectnew.component';

import { ProjectTestcasesComponent } from './projects/testcases/projecttestcases.component';
import { ProjectTestcaseNewComponent } from './projects/testcases/new/projecttestcasenew.component';

import { ProjectTestplansComponent } from './projects/testplans/projecttestplans.component';
import { ProjectTestplanDetailComponent } from './projects/testplans/detail/projecttestplandetail.component';
import { ProjectTestplanNewComponent } from './projects/testplans/new/projecttestplannew.component';

import { ProjectTestrunComponent } from './projects/testrun/projecttestrun.component';

const routes: Routes = [
  { path: 'user/signin', component: SigninComponent },
  { path: 'projects', children: [
    { path: '', component: ProjectsComponent },
    { path: 'new', component: ProjectNewComponent }
  ]},
  { path: 'projects/:projectId', children: [
      { path: '', component: ProjectDetailComponent },
      { path: 'cases', children: [
        { path: '', component: ProjectTestcasesComponent },
        { path: 'new', component: ProjectTestcaseNewComponent }
      ]},
      { path: 'plans', children: [
        { path: '', component: ProjectTestplansComponent },
        { path: 'new', component: ProjectTestplanNewComponent },
        { path: ':planId', component: ProjectTestplanDetailComponent }
      ] },
      { path: 'runs', component: ProjectTestrunComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
