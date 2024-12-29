import { Component, inject, OnInit } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AppointmentService } from '../../../core/auth/appointment.service';
import { JwtService } from '../../../core/utility/jwt.service';
import Swal from 'sweetalert2';
import { ToasterService } from '../../../core/utility/toaster.service';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [NavbarComponent, RouterLink, CommonModule, ReactiveFormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  PatientId: number = 0;
  isupdate: Boolean = false;
  isLoading: Boolean = false;
  UpdateAppointmentForm!: FormGroup;
  currentEditedElement?: number;
  PatientName: any[] = [];
  selectedProduct: any;
  currentRole: string = '';
  todayDate = new Date().toISOString().split('T')[0];
  ngOnInit(): void {
    this.PatientId = this.jwtService.getUserId();
    this.currentRole = this.jwtService.getRole();

    if (this.currentRole === 'Patient') {
      this.getAllAppointmentForPatient(this.PatientId);
    } else if (this.currentRole === 'Provider') {
      this.getAllAppointmentForProvider(this.PatientId);
    }
  }
  closeModal() {
    const modal = document.getElementById('addProductModal');
    if (modal) {
      modal.style.display = 'none';
      modal.classList.remove('show');
      modal.setAttribute('aria-hidden', 'true');
    }
    // this.onClickReset();
  }

  constructor(private fb: FormBuilder) {
    this.UpdateAppointmentForm = this.fb.group({
      appointmentDate: [''],
      appointmentTime: [''],
      chiefComplaint: [''],
      fees: [''],
      providerName: [''],
      status: [''],
    });
  }
  AppointmentList: any[] = [];
  private appointmentService = inject(AppointmentService);
  private jwtService = inject(JwtService);
  private toasterService = inject(ToasterService);
  private router = inject(Router);
  getAllAppointmentForPatient(PatientId: any) {
    this.appointmentService.getPatientAppointment(PatientId).subscribe({
      next: (response) => {
        if (response) {
          this.AppointmentList = response.patientData;
          this.PatientName = response.userFullName;
          console.log(
            'all login patientData AppointmentList',
            this.AppointmentList
          );
        } else {
          console.log('response not handled properly');
        }
      },
      error: (error) => {
        console.log('getting error while patient Data', error);
      },
    });
  }

  getAllAppointmentForProvider(PatientId: any) {
    console.log('PatientId new Check', PatientId);
    this.appointmentService.getProviderAppointment(PatientId).subscribe({
      next: (response) => {
        if (response) {
          this.AppointmentList = response.patientData;
          this.PatientName = response.userFullName;
          console.log('appointmentProviderResponse', this.AppointmentList);
        } else {
          console.log('response not handled properly');
        }
      },
      error: (error) => {
        console.log('getting error while patient Data', error);
      },
    });
  }
  updateAppointmentModal(appointment: any) {
    this.currentEditedElement = appointment.appointmentId;

    this.UpdateAppointmentForm.patchValue(appointment);
    console.log('all patchvalue', appointment);

    this.UpdateAppointmentForm.get('appointmentDate')?.setValue(
      appointment.appointmentDate
        ? new Date(appointment.appointmentDate).toISOString().split('T')[0]
        : ''
    );

    const modal = document.getElementById('addProductModal');
    if (modal) {
      modal.style.display = 'block';
      modal.classList.add('show');
      modal.setAttribute('aria-hidden', 'false');
    }
  }
  deleteAppointment(id: number): void {
    Swal.fire({
      title: 'Are you sure?',
      text: 'Are you sure you want to caneled this appointment?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, canceled it!',
      cancelButtonText: 'No, keep it',
    }).then((result) => {
      if (result.isConfirmed) {
        this.isLoading = true;
        this.appointmentService.cancelledAppointment(id).subscribe(
          (response) => {
            console.log('login response', response);
            if (response.status == 200) {
              this.toasterService.showSuccess(response.message);
              this.isLoading = false;
              if (this.currentRole === 'Patient') {
                this.getAllAppointmentForPatient(this.PatientId);
              } else if (this.currentRole === 'Provider') {
                this.getAllAppointmentForProvider(this.PatientId);
              }
            } else {
              this.toasterService.showError('Unable to delete');
            }
          },
          (error) => {
            this.isLoading = false;
            this.toasterService.showError(error.message);
          }
        );
      } else {
        console.log('Product deletion canceled');
      }
    });
  }

  viewProduct(product: any): void {
    // this.selectedProduct = product;
    // console.log('selectedProduct', this.selectedProduct);
    const modal = document.getElementById('viewProductModal');
    if (modal) {
      modal.style.display = 'block';
      modal.classList.add('show');
      modal.setAttribute('aria-hidden', 'false');
    }
  }
  closeViewModal(): void {
    const modal = document.getElementById('viewProductModal');
    if (modal) {
      modal.style.display = 'none';
      modal.classList.remove('show');
      modal.setAttribute('aria-hidden', 'true');
    }
  }
  submitProduct() {
    const appointment = {
      appointmentId: this.currentEditedElement ?? null,
      appointmentDate: this.UpdateAppointmentForm.get('appointmentDate')?.value,
      appointmentTime: this.UpdateAppointmentForm.get('appointmentTime')?.value,
      chiefComplaint: this.UpdateAppointmentForm.get('chiefComplaint')?.value,
    };

    console.log('submit formdata', appointment);
    this.appointmentService.updatePatientAppointment(appointment).subscribe({
      next: (response) => {
        if (response.status == 200) {
          console.log(response);
          this.toasterService.showSuccess('appointment updated successfully');
          this.getAllAppointmentForPatient(this.PatientId);
          this.closeModal();
        } else {
          this.toasterService.showError('response not handled succeessfully');
        }
      },
      error: (errror) => {
        this.toasterService.showError(errror);
      },
    });
  }
  gotToBooking(appointmentId: number) {
    this.router.navigateByUrl(`/org/providerAppointment/${appointmentId}`);
  }
  onClickMessage(receiverId: number) {
    this.router.navigateByUrl(`/org/chat/${receiverId}`);
    console.log('receiverId', receiverId);
  }
}
