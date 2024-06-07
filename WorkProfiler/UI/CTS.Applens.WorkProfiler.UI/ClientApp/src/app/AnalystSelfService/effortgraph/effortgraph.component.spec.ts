import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EffortgraphComponent } from './effortgraph.component';

describe('EffortgraphComponent', () => {
  let component: EffortgraphComponent;
  let fixture: ComponentFixture<EffortgraphComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EffortgraphComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EffortgraphComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
