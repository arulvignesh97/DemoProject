import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InfoiconComponent } from './infoicon.component';

describe('InfoiconComponent', () => {
  let component: InfoiconComponent;
  let fixture: ComponentFixture<InfoiconComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InfoiconComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InfoiconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
