import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../../../core/auth/auth.service';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { ToasterService } from '../../../../core/utility/toaster.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  isLoading: boolean = false;
  loginValue: any = {};
  showPassword: boolean = false;
  authService = inject(AuthService);
  toasterService = inject(ToasterService);
  router = inject(Router);
  LoginData: FormGroup = new FormGroup({
    userName: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  });
  loginUser() {
    this.loginValue = this.LoginData.value;
    this.isLoading = true;
    localStorage.setItem('userName', this.LoginData.get('userName')?.value);
    this.authService.loginData(this.loginValue).subscribe({
      next: (response: any) => {
        if (response.status == 200) {
          this.isLoading = true;
          this.toasterService.showSuccess(response.message);
          this.router.navigateByUrl('/auth/verifyOtp');
        } else {
          this.isLoading = false;
          this.toasterService.showError(response.message);
        }
      },

      error: (error) => {
        this.isLoading = false;
        this.toasterService.showError(error.message);
      },
    });
  }

  onClickShowPassword() {
    this.showPassword = !this.showPassword;
  }
}
