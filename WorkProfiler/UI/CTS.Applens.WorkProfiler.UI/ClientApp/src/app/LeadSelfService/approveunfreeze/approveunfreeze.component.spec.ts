import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApproveunfreezeComponent } from './approveunfreeze.component';

describe('ApproveunfreezeComponent', () => {
  let component: ApproveunfreezeComponent;
  let fixture: ComponentFixture<ApproveunfreezeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApproveunfreezeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApproveunfreezeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
