import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApppointmentHistoryComponent } from './apppointment-history.component';

describe('ApppointmentHistoryComponent', () => {
  let component: ApppointmentHistoryComponent;
  let fixture: ComponentFixture<ApppointmentHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApppointmentHistoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApppointmentHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
