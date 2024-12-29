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
      'https://localhost:7059/api/User/RegisterProvider',
      patient
    );
  }
  loginData(data: any): Observable<any> {
    return this.http.post<any>('https://localhost:7059/api/User/login', data);
  }

  forgotPassword(useremail: object): Observable<any> {
    return this.http.post<any>(
      'https://localhost:7059/api/User/forgetPassword',
      useremail
    );
  }

  verfiOtp(data: any): Observable<any> {
    return this.http.post<any>(
      'https://localhost:7059/api/User/verifyotp',
      data
    );
  }
  getUserById(userId: Number): Observable<any> {
    return this.http.get<any>(
      `https://localhost:7059/api/User/getUserById/${userId}`
    );
  }
  changePassword(emailId: any): Observable<any> {
    return this.http.post<any>(
      'https://localhost:7059/api/User/ChangePassword',
      emailId
    );
  }
  updateUserProfile(userData: any): Observable<any> {
    return this.http.put<any>(
      'https://localhost:7059/api/User/UpdateUser',
      userData
    );
  }
}
