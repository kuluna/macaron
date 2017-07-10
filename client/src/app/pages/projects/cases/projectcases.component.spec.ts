import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectCasesComponent } from './projectcases.component';

describe('TestcasesComponent', () => {
  let component: ProjectCasesComponent;
  let fixture: ComponentFixture<ProjectCasesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectCasesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectCasesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
