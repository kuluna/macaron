import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SigninComponent } from './user/signin/signin.component';
import { ProjectsComponent } from './projects/projects.component';
import { ProjectDetailComponent } from './projects/detail/projectdetail.component';
import { ProjectMilestonesComponent } from './projects/milestones/projectmilestones.component';
import { ProjectMilestoneNewComponent } from './projects/milestones/new/projectmilestonenew.component';

const routes: Routes = [
  { path: 'user/signin', component: SigninComponent },
  { path: 'projects', component: ProjectsComponent },
  { path: 'projects/:projectId', children: [
      { path: '', component: ProjectDetailComponent },
      { path: 'milestones', children: [
        { path: '', component: ProjectMilestonesComponent },
        { path: 'new', component: ProjectMilestoneNewComponent }
      ]}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
