import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProviderAppointmentHistoryComponent } from './provider-appointment-history.component';

describe('ProviderAppointmentHistoryComponent', () => {
  let component: ProviderAppointmentHistoryComponent;
  let fixture: ComponentFixture<ProviderAppointmentHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProviderAppointmentHistoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProviderAppointmentHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
