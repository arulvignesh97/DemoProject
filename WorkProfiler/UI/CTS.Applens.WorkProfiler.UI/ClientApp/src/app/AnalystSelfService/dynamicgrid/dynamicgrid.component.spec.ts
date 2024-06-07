import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DynamicgridComponent } from './dynamicgrid.component';

describe('DynamicgridComponent', () => {
  let component: DynamicgridComponent;
  let fixture: ComponentFixture<DynamicgridComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DynamicgridComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DynamicgridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
