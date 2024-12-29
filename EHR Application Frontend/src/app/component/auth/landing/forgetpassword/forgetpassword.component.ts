import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';
import { ToasterService } from '../../../../core/utility/toaster.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-forgetpassword',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './forgetpassword.component.html',
  styleUrl: './forgetpassword.component.css',
})
export class ForgetpasswordComponent {
  emailForm!: FormGroup;
  resetPasswordForm!: FormGroup;
  isLoading: boolean = false;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private toasterService: ToasterService
  ) {}

  ngOnInit(): void {
    this.emailForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  sendResetLink(): void {
    if (this.emailForm.valid) {
      const payload = {
        email: this.emailForm.value.email,
      };
      this.authService.forgotPassword(payload).subscribe({
        next: (response) => {
          if (response.status == 200) {
            this.isLoading = true;
            this.router.navigateByUrl('/auth/login');
            this.toasterService.showSuccess(response.message);
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
  }
}
