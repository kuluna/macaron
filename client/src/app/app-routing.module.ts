import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SigninComponent } from './user/signin/signin.component';
import { ProjectsComponent } from './projects/projects.component';
import { ProjectDetailComponent } from './projects/detail/projectdetail.component';
import { ProjectTestcasesComponent } from './projects/testcases/projecttestcases.component';
import { ProjectTestcaseNewComponent } from './projects/testcases/new/projecttestcasenew.component';
import { ProjectTestrunComponent } from './projects/testrun/projecttestrun.component';

const routes: Routes = [
  { path: 'user/signin', component: SigninComponent },
  { path: 'projects', component: ProjectsComponent },
  { path: 'projects/:projectId', children: [
      { path: '', component: ProjectDetailComponent },
      { path: 'testcases', children: [
        { path: '', component: ProjectTestcasesComponent },
        { path: 'new', component: ProjectTestcaseNewComponent }
      ]},
      { path: 'testrun', component: ProjectTestrunComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
