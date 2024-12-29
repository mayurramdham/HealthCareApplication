import { Component, Inject, inject } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ToasterService } from '../../../core/utility/toaster.service';
import { JwtService } from '../../../core/utility/jwt.service';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/auth/auth.service';
import { CommonModule, formatDate } from '@angular/common';
import { NavbarComponent } from '../navbar/navbar.component';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NavbarComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css',
})
export class ProfileComponent {
  userData: any;
  userId: number = 0;
  updateProfileForm!: FormGroup;
  StateName: any;
  countryName: any;
  specilization: any;
  changePasswordForm: FormGroup;
  jwtService = inject(JwtService);
  toasterService = inject(ToasterService);
  private authService = inject(AuthService);
  private router = inject(Router);
  todayDate = new Date().toISOString().split('T')[0];

  constructor(private fb: FormBuilder) {
    this.changePasswordForm = this.fb.group(
      {
        email: ['', [Validators.required, Validators.minLength(3)]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required, Validators.minLength(6)]],
      },
      {
        validator: this.passwordsMatchValidator,
      }
    );
  }
  onOpenupdateProfileModal(userData: any) {
    const modal = document.getElementById('updateProfileModal');
    if (modal) {
      modal.style.display = 'block';
      modal.classList.add('show');
      modal.setAttribute('aria-hidden', 'false');
    }
    this.updateProfileForm.patchValue(userData);
    this.updateProfileForm
      .get('dateOfBirth')
      ?.setValue(
        userData.dateOfBirth
          ? new Date(userData.dateOfBirth).toISOString().split('T')[0]
          : ''
      );

    console.log('updatefrompatchvalue', userData);
    console.log('updatefromvalue2', this.updateProfileForm.value.dateOfBirth);
  }

  onCloseupdateProfileModal() {
    const modal = document.getElementById('updateProfileModal');
    if (modal) {
      modal.style.display = 'none';
      modal.classList.remove('show');
      modal.setAttribute('aria-hidden', 'true');
    }
  }

  //handle update profile logic code
  ngOnInit(): void {
    // Replace with actual logic to get userId
    this.userId = this.jwtService.getUserId();
    this.getUserData(this.userId);

    this.updateProfileForm = new FormGroup({
      userId: new FormControl(''),
      firstName: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(12),
      ]),
      lastName: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      dateOfBirth: new FormControl('', Validators.required),
      mobile: new FormControl('', [
        Validators.required,
        Validators.pattern(/^\d{10}$/),
      ]),
      address: new FormControl('', Validators.required),
      pinCode: new FormControl('', Validators.required),
    });
  }
  onUpdateProfile(): void {
    if (this.updateProfileForm.valid) {
      console.log('Form Submitted!', this.updateProfileForm.value);
      const payload = {
        UserId: this.userId, // or you can use this.updateProfileForm.value.userId if that's intended

        firstName: this.updateProfileForm.value.firstName,
        lastName: this.updateProfileForm.value.lastName,
        email: this.updateProfileForm.value.email,
        dateOfBirth: this.updateProfileForm.value.dateOfBirth,
        mobile: this.updateProfileForm.value.mobile,
        address: this.updateProfileForm.value.address,
        pinCode: this.updateProfileForm.value.pinCode,
      };

      console.log('profile payload is', payload);
      this.authService.updateUserProfile(payload).subscribe((response) => {
        if (response.status == 200) {
          this.toasterService.showSuccess('Profile updated successfully');
          const userId = this.jwtService.getUserId();
          this.getUserData(userId);
          this.onCloseupdateProfileModal();
        }
      });
    } else {
      this.toasterService.showError('Please fill out the form correctly.');
    }
  }

  openModal() {
    const modal = document.getElementById('changePasswordModal');
    if (modal) {
      modal.style.display = 'block';
      modal.classList.add('show');
      modal.setAttribute('aria-hidden', 'false');
    }
  }

  closeModal() {
    const modal = document.getElementById('changePasswordModal');
    if (modal) {
      modal.style.display = 'none';
      modal.classList.remove('show');
      modal.setAttribute('aria-hidden', 'true');
    }
  }

  changePasswordModal(product: any) {
    console.log('product', product);

    // this.productForm.get('PrName')?.setValue(product.pr)
    // this.productForm.get()
    const modal = document.getElementById('changePasswordModal');
    if (modal) {
      modal.style.display = 'block';
      modal.classList.add('show');
      modal.setAttribute('aria-hidden', 'false');
    }
  }

  passwordsMatchValidator(group: FormGroup): { [key: string]: boolean } | null {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;

    return password && confirmPassword && password !== confirmPassword
      ? { passwordMismatch: true }
      : null;
  }

  onSubmit(): void {
    if (this.changePasswordForm.invalid) {
      console.log('Form is invalid!');
      return;
    }
    const payload = {
      email: this.changePasswordForm.get('email')?.value,
      password: this.changePasswordForm.get('password')?.value,
    };
    console.log('payload is', payload);
    this.authService.changePassword(payload).subscribe(
      (response) => {
        if (response.status == 200) {
          this.router.navigateByUrl('auth/landing');
          this.toasterService.showSuccess('Password Change Successfully');
        } else {
          this.toasterService.showError('Invalid credentials');
        }
      },
      (error) => {
        this.toasterService.showError('unable to get response');
      }
    );
    const { email, password } = this.changePasswordForm.value;
    console.log(`Password for ${email} changed to ${password}`);
  }

  getUserData(userId: number): void {
    this.authService.getUserById(userId).subscribe((response: any) => {
      if (response.status === 200) {
        this.userData = response.data;
        this.StateName = response.state;
        this.countryName = response.country;
        this.specilization = response.specilizations;
        console.log(
          'User Data:',
          response.data.specialisation.specialisationName
        );
      }
    });
  }

  changeUserPassword() {
    this.authService.changePassword;
  }

  getUserType(userTypeId: number | undefined): string {
    if (userTypeId === 1) return 'Patient';
    if (userTypeId === 2) return 'Provider';
    return 'Unknown';
  }

  validateMobileLength(event: Event): void {
    const input = event.target as HTMLInputElement;
    const value = input.value;

    if (value.length > 10) {
      input.value = value.slice(0, 10);
    }
  }

  validateZipcodeLength(event: Event): void {
    const input = event.target as HTMLInputElement;
    const value = input.value;

    if (value.length > 6) {
      // Limit the input to 6 digits
      input.value = value.slice(0, 6);
    }
  }

  removeNumbers(event: KeyboardEvent) {
    const charCode = event.charCode;

    if (charCode >= 48 && charCode <= 57) {
      event.preventDefault();
    }
  }
}
