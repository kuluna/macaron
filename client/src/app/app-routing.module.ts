import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SigninComponent } from './user/signin/signin.component';
import { ProjectsComponent } from './projects/projects.component';
import { ProjectDetailComponent } from './projects/detail/projectdetail.component';
import { ProjectTestcasesComponent } from './projects/testcases/projecttestcases.component';

const routes: Routes = [
  { path: 'user/signin', component: SigninComponent },
  { path: 'projects', component: ProjectsComponent },
  { path: 'projects/:projectId', children: [
      { path: '', component: ProjectDetailComponent },
      { path: 'testcases', children: [
        { path: '', component: ProjectTestcasesComponent }
      ]}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
