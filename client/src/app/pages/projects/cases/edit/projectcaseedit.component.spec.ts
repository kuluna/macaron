import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectCaseEditComponent } from './projectcaseedit.component';

describe('ProjectcaseeditComponent', () => {
  let component: ProjectCaseEditComponent;
  let fixture: ComponentFixture<ProjectCaseEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectCaseEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectCaseEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
