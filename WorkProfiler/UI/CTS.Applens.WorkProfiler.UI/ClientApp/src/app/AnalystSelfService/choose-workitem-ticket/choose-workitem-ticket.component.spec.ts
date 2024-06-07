import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseWorkitemTicketComponent } from './choose-workitem-ticket.component';

describe('ChooseWorkitemTicketComponent', () => {
  let component: ChooseWorkitemTicketComponent;
  let fixture: ComponentFixture<ChooseWorkitemTicketComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseWorkitemTicketComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseWorkitemTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
