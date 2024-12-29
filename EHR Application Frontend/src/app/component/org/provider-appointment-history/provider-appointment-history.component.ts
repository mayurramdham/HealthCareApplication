import { Component, inject } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { AppointmentService } from '../../../core/auth/appointment.service';
import { JwtService } from '../../../core/utility/jwt.service';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-provider-appointment-history',
  standalone: true,
  imports: [NavbarComponent, CommonModule, DatePipe],
  templateUrl: './provider-appointment-history.component.html',
  styleUrl: './provider-appointment-history.component.css',
})
export class ProviderAppointmentHistoryComponent {
  private appointment = inject(AppointmentService);
  private jwtService = inject(JwtService);
  patientId: number = 0;
  ngOnInit() {
    this.patientId = this.jwtService.getUserId();
    this.getAllStautsAppointment();
  }
  appointmentStatus: any[] = [];

  getAllStautsAppointment() {
    this.appointment
      .getAllProviderAppointmentWithoutStatus(this.patientId)
      .subscribe({
        next: (response) => {
          if (response.staus == 200) {
            this.appointmentStatus = response.patientData;
          } else {
            console.log('error in wthous staus api');
          }
        },
      });
  }
}
