import {
  Component,
  inject,
  OnInit,
  runInInjectionContext,
} from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { AppointmentService } from '../../../core/auth/appointment.service';
import { JwtService } from '../../../core/utility/jwt.service';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-apppointment-history',
  standalone: true,
  imports: [NavbarComponent, CommonModule, DatePipe],
  templateUrl: './apppointment-history.component.html',
  styleUrl: './apppointment-history.component.css',
})
export class ApppointmentHistoryComponent implements OnInit {
  private appointment = inject(AppointmentService);
  private jwtService = inject(JwtService);
  patientId: number = 0;
  ngOnInit() {
    this.patientId = this.jwtService.getUserId();

    console.log('appointhistoryid', this.patientId);
    this.getAllStautsAppointment();
  }
  appointmentStatus: any[] = [];

  getAllStautsAppointment() {
    this.appointment.getAllAppointmentWithoutStatus(this.patientId).subscribe({
      next: (response) => {
        if (response.staus == 200) {
          this.appointmentStatus = response.patientData;
          console.log('patientStausData', this.appointmentStatus);
          console.log('patientStausDataResponse', response);
        } else {
          console.log('error in wthous staus api');
        }
      },
    });
  }

   // Example status

}
