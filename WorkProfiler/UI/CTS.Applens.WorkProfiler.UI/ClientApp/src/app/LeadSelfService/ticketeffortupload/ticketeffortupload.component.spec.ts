import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketeffortuploadComponent } from './ticketeffortupload.component';

describe('TicketeffortuploadComponent', () => {
  let component: TicketeffortuploadComponent;
  let fixture: ComponentFixture<TicketeffortuploadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TicketeffortuploadComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketeffortuploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
