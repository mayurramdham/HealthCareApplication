import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../../../core/auth/auth.service';
import { ToasterService } from '../../../../core/utility/toaster.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-verify-otp',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './verify-otp.component.html',
  styleUrl: './verify-otp.component.css',
})
export class VerifyOtpComponent {
  authService = inject(AuthService);
  toasterService = inject(ToasterService);
  router = inject(Router);
  isLoading: boolean = false;
  otpDataValue: any = {};
  otpData: FormGroup = new FormGroup({
    userName: new FormControl(''),
    otpCode: new FormControl('', [
      Validators.required,
      Validators.pattern(/^\d{6}$/), // Ensures only 6-digit numbers are valid
    ]),
  });

  otpVerify() {
    this.isLoading = true;
    if (this.otpData.value) {
      const otpValue = Number(this.otpData.get('otpCode')?.value);
      const payload = {
        OtpCode: otpValue,
        userName: localStorage.getItem('userName'),
      };
      console.log('payload', payload);
      console.log('OTP value:', otpValue, typeof otpValue);
      this.authService.verfiOtp(payload).subscribe(
        (res: any) => {
          console.log('api response', res);
          console.error('error at verify');
          if (res.status == 200) {
            this.toasterService.showSuccess('Otp Verified Successfully');
            localStorage.removeItem('email');
            localStorage.setItem('accessToken', res.token);
            this.router.navigateByUrl('/org/home');
          } else {
            this.isLoading = true;
            this.toasterService.showError('Invalid otp or otp expired');
          }
        },
        (error) => {
          console.log(error);
          this.isLoading = false;
          this.toasterService.showError('unable to get the response');
        }
      );
    } else {
      this.toasterService.showError('Please enter a valid OTP');
    }
  }
}
