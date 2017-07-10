import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SigninComponent } from './pages/user/signin/signin.component';

import { ProjectsComponent } from './pages/projects/projects.component';
import { ProjectDetailComponent } from './pages/projects/detail/projectdetail.component';
import { ProjectNewComponent } from './pages/projects/new/projectnew.component';

import { ProjectCasesComponent } from './pages/projects/cases/projectcases.component';
import { ProjectCaseNewComponent } from './pages/projects/cases/new/projectcasenew.component';

import { ProjectPlansComponent } from './pages/projects/plans/projectplans.component';
import { ProjectPlanDetailComponent } from './pages/projects/plans/detail/projectplandetail.component';
import { ProjectPlanNewComponent } from './pages/projects/plans/new/projectplannew.component';

import { ProjectRunComponent } from './pages/projects/run/projectrun.component';

const routes: Routes = [
  { path: 'user/signin', component: SigninComponent },
  { path: 'projects', children: [
    { path: '', component: ProjectsComponent },
    { path: 'new', component: ProjectNewComponent }
  ]},
  { path: 'projects/:projectId', children: [
      { path: '', component: ProjectDetailComponent },
      { path: 'cases', children: [
        { path: '', component: ProjectCasesComponent },
        { path: 'new', component: ProjectCaseNewComponent }
      ]},
      { path: 'plans', children: [
        { path: '', component: ProjectPlansComponent },
        { path: 'new', component: ProjectPlanNewComponent },
        { path: ':planId', component: ProjectPlanDetailComponent }
      ] },
      { path: 'runs', component: ProjectRunComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
