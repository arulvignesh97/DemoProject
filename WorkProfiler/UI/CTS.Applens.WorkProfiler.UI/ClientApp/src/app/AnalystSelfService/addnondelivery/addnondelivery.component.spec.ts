import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddnondeliveryComponent } from './addnondelivery.component';

describe('AddnondeliveryComponent', () => {
  let component: AddnondeliveryComponent;
  let fixture: ComponentFixture<AddnondeliveryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddnondeliveryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddnondeliveryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
