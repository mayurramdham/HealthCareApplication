import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) {}
  registerPatient(patient: any): Observable<any> {
    return this.http.post(
      'https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/User/RegisterProvider',
      patient
    );
  }
  loginData(data: any): Observable<any> {
    return this.http.post<any>(
      'https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/User/login',
      data
    );
  }

  forgotPassword(useremail: object): Observable<any> {
    return this.http.post<any>(
      'https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/User/forgetPassword',
      useremail
    );
  }

  verfiOtp(data: any): Observable<any> {
    return this.http.post<any>(
      'https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/User/verifyotp',
      data
    );
  }
  getUserById(userId: Number): Observable<any> {
    return this.http.get<any>(
      `https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/User/getUserById/${userId}`
    );
  }
  changePassword(emailId: any): Observable<any> {
    return this.http.post<any>(
      'https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/User/ChangePassword',
      emailId
    );
  }
  updateUserProfile(userData: any): Observable<any> {
    return this.http.put<any>(
      'https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/User/UpdateUser',
      userData
    );
  }
}
