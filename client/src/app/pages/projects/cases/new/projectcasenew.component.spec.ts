import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectCaseNewComponent } from './projectcasenew.component';

describe('ProjecttestcasenewComponent', () => {
  let component: ProjectCaseNewComponent;
  let fixture: ComponentFixture<ProjectCaseNewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectCaseNewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectCaseNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
