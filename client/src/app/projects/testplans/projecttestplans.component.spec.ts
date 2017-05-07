import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectTestplansComponent } from './projecttestplans.component';

describe('ProjecttestplansComponent', () => {
  let component: ProjectTestplansComponent;
  let fixture: ComponentFixture<ProjectTestplansComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectTestplansComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectTestplansComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
