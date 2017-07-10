import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectNavComponent } from './projectnav.component';

describe('BodycontentComponent', () => {
  let component: ProjectNavComponent;
  let fixture: ComponentFixture<ProjectNavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectNavComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
