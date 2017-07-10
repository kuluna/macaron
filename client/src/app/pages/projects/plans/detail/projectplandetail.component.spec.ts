import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectTestplanDetailComponent } from './projecttestplandetail.component';

describe('ProjecttestplandetailComponent', () => {
  let component: ProjectTestplanDetailComponent;
  let fixture: ComponentFixture<ProjectTestplanDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectTestplanDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectTestplanDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
