import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectTestplanNewComponent } from './projecttestplannew.component';

describe('ProjecttestplannewComponent', () => {
  let component: ProjectTestplanNewComponent;
  let fixture: ComponentFixture<ProjectTestplanNewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectTestplanNewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectTestplanNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
