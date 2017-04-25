import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectTestcaseNewComponent } from './projecttestcasenew.component';

describe('ProjecttestcasenewComponent', () => {
  let component: ProjectTestcaseNewComponent;
  let fixture: ComponentFixture<ProjectTestcaseNewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectTestcaseNewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectTestcaseNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
