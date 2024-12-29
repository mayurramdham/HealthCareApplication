import {
  Component,
  inject,
  Inject,
  OnInit,
  runInInjectionContext,
} from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { AppointmentService } from '../../../core/auth/appointment.service';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';
import { ToasterService } from '../../../core/utility/toaster.service';

@Component({
  selector: 'app-soap',
  standalone: true,
  imports: [NavbarComponent, ReactiveFormsModule, CommonModule],
  templateUrl: './soap.component.html',
  styleUrl: './soap.component.css',
})
export class SoapComponent implements OnInit {
  private appointment = inject(AppointmentService);
  private toaster = inject(ToasterService);
  soapForm: FormGroup;
  constructor(private route: ActivatedRoute, private fb: FormBuilder) {
    this.soapForm = this.fb.group({
      subjective: ['', Validators.required],
      objective: ['', Validators.required],
      assessment: ['', Validators.required],
      plan: ['', Validators.required],
    });
  }
  appointmentId?: number;
  soapData: any = {};
  ngOnInit(): void {
    this.appointmentId = Number(this.route.snapshot.paramMap.get('id') || '');
    this.getPatientSoap(this.appointmentId);

    console.log('appointmentId', this.appointmentId);
  }

  getPatientSoap(appointmentId: number) {
    this.appointment.getAppointmetForSoap(appointmentId).subscribe({
      next: (response) => {
        if (response.status == 200) {
          this.soapData = response.appData;
          console.log('getDataForPatientAppoinmentSoap', this.soapData);
        }
      },
    });
  }
  saveSOAPNotes() {
    if (this.soapForm.valid) {
      const payload = {
        SoapNoteId: this.appointmentId,
        subjective: this.soapForm.get('subjective')?.value,
        assessment: this.soapForm.get('assessment')?.value,
        objective: this.soapForm.get('objective')?.value,
        plan: this.soapForm.get('plan')?.value,
      };
      console.log('payload', payload);
      this.appointment.addSoapNote(payload).subscribe({
        next: (response) => {
          if ((response.status = 200)) {
            this.toaster.showSuccess('SOAP notes saved successfully!');
            this.resetSOAPNotesForm();
            this.getPatientSoap(this.appointmentId ?? 0);
          } else {
            this.toaster.showError('response not handled properly');
          }
        },
        error: (error) => {
          this.toaster.showError(error);
        },
      });
    } else {
      alert('Please fill out all fields.');
    }
  }
  resetSOAPNotesForm() {
    this.soapForm.reset();
  }
  get age(): number {
    const dob = new Date(this.soapData.user.dateOfBirth);
    const ageDifMs = Date.now() - dob.getTime();
    const ageDate = new Date(ageDifMs);
    return Math.abs(ageDate.getUTCFullYear() - 1970);
  }
}
