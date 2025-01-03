import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AppointmentService {
  constructor(private http: HttpClient) {}
  addAppointment(data: any): Observable<any> {
    return this.http.post(
      'https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/Appointment/bookAppointment',
      data
    );
  }
  getProviderList(userTypeId: any): Observable<any> {
    return this.http.get(
      `https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/Appointment/getProvider/${userTypeId}`
    );
  }
  getPatientAppointment(patientId: any): Observable<any> {
    return this.http.get(
      `https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/Appointment/getAllPatient/${patientId}`
    );
  }
  getProviderAppointment(patientId: number): Observable<any> {
    return this.http.get(
      `https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/Appointment/getAllPatientByProvider/${patientId}`
    );
  }
  cancelledAppointment(AppointmentId: number): Observable<any> {
    return this.http.delete(
      `https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/Appointment/DeleteAppointment/${AppointmentId}`
    );
  }
  updatePatientAppointment(AppointmentId: any): Observable<any> {
    return this.http.put(
      `https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/Appointment/UpdatePatientAppointment`,
      AppointmentId
    );
  }
  getAppointmetForSoap(AppointmentId: number): Observable<any> {
    return this.http.get(
      `https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/Appointment/getAppointmentById/${AppointmentId}`
    );
  }
  addSoapNote(soapNote: any): Observable<any> {
    return this.http.post(
      'https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/Appointment/AddSoapNote',
      soapNote
    );
  }
  addPayment(paymentData: any): Observable<any> {
    return this.http.post(
      'https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/Stripe/PaymentBookAppointment',
      paymentData
    );
  }

  getAllAppointmentWithoutStatus(patientId: number): Observable<any> {
    return this.http.get(
      `https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/Appointment/getAllPatientWihoutStatus/${patientId}`
    );
  }
  getAllProviderAppointmentWithoutStatus(patientId: number): Observable<any> {
    return this.http.get(
      `https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/Appointment/getAllProviderWihoutStatus/${patientId}`
    );
  }
}
