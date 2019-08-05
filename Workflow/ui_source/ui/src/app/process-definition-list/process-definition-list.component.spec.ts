import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProcessDefinitionListComponent } from './process-definition-list.component';

describe('ProcessDefinitionListComponent', () => {
  let component: ProcessDefinitionListComponent;
  let fixture: ComponentFixture<ProcessDefinitionListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProcessDefinitionListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcessDefinitionListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
