import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { MaterialModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { BodyContentComponent } from './bodycontent.component';
import { ProjectNavComponent } from './projectnav.component';

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

@NgModule({
  declarations: [
    AppComponent,
    BodyContentComponent,
    ProjectNavComponent,
    SigninComponent,
    ProjectsComponent,
    ProjectDetailComponent,
    ProjectNewComponent,
    ProjectTestcasesComponent,
    ProjectTestcaseNewComponent,
    ProjectTestrunComponent,
    ProjectTestplansComponent,
    ProjectTestplanDetailComponent,
    ProjectTestplanNewComponent
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    FormsModule,
    HttpModule,
    MaterialModule,
    FlexLayoutModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
