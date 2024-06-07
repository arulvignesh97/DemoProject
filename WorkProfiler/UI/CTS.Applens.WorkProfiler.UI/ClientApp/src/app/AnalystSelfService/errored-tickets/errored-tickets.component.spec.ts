import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ErroredTicketsComponent } from './errored-tickets.component';

describe('ErroredTicketsComponent', () => {
  let component: ErroredTicketsComponent;
  let fixture: ComponentFixture<ErroredTicketsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ErroredTicketsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ErroredTicketsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
