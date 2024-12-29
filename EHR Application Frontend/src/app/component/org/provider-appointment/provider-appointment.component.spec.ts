import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProviderAppointmentComponent } from './provider-appointment.component';

describe('ProviderAppointmentComponent', () => {
  let component: ProviderAppointmentComponent;
  let fixture: ComponentFixture<ProviderAppointmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProviderAppointmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProviderAppointmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
