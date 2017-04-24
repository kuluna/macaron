import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectTestcasesComponent } from './projecttestcases.component';

describe('TestcasesComponent', () => {
  let component: ProjectTestcasesComponent;
  let fixture: ComponentFixture<ProjectTestcasesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectTestcasesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectTestcasesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
