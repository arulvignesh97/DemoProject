import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BasemeasuresComponent } from './basemeasures.component';

describe('BasemeasuresComponent', () => {
  let component: BasemeasuresComponent;
  let fixture: ComponentFixture<BasemeasuresComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BasemeasuresComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BasemeasuresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
