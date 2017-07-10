import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { MaterialModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MarkdownModule } from 'angular2-markdown';
import { NgPipesModule } from 'ngx-pipes';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BodyContentComponent } from './components/bodycontent.component';
import { ProjectNavComponent } from './components/projectnav.component';

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

@NgModule({
  declarations: [
    AppComponent,
    BodyContentComponent,
    ProjectNavComponent,
    SigninComponent,
    ProjectsComponent,
    ProjectDetailComponent,
    ProjectNewComponent,
    ProjectCasesComponent,
    ProjectCaseNewComponent,
    ProjectRunComponent,
    ProjectPlansComponent,
    ProjectPlanDetailComponent,
    ProjectPlanNewComponent
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    FormsModule,
    HttpModule,
    NgPipesModule,
    MaterialModule,
    MarkdownModule,
    FlexLayoutModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
