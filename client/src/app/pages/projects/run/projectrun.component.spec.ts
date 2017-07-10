import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectTestrunComponent } from './projecttestrun.component';

describe('ProjecttestrunComponent', () => {
  let component: ProjectTestrunComponent;
  let fixture: ComponentFixture<ProjectTestrunComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectTestrunComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectTestrunComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
