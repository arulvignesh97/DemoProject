import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketAttributesComponent } from './ticket-attributes.component';

describe('TicketAttributesComponent', () => {
  let component: TicketAttributesComponent;
  let fixture: ComponentFixture<TicketAttributesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TicketAttributesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketAttributesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
