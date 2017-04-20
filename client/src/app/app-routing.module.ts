import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SigninComponent } from './user/signin/signin.component';
import { ProjectsComponent } from './projects/projects.component';
import { ProjectOverviewComponent } from './projects/overview/projectoverview.component';

const routes: Routes = [
  {
    path: 'user/signin',
    component: SigninComponent
  },
  {
    path: 'projects',
    component: ProjectsComponent
  },
  {
    path: 'projects/:id',
    children: [
      {
        path: '', redirectTo: 'overview', pathMatch: 'full',
      },
      {
        path: 'overview', component: ProjectOverviewComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
