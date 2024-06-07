import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BasemeasuresearchComponent } from './basemeasuresearch.component';

describe('BasemeasuresearchComponent', () => {
  let component: BasemeasuresearchComponent;
  let fixture: ComponentFixture<BasemeasuresearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BasemeasuresearchComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BasemeasuresearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
