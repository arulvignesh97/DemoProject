import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddworkitemComponent } from './addworkitem.component';

describe('AddworkitemComponent', () => {
  let component: AddworkitemComponent;
  let fixture: ComponentFixture<AddworkitemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddworkitemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddworkitemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
