import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectMilestoneNewComponent } from './projectmilestonenew.component';

describe('ProjectmilestonenewComponent', () => {
  let component: ProjectMilestoneNewComponent;
  let fixture: ComponentFixture<ProjectMilestoneNewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectMilestoneNewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectMilestoneNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
