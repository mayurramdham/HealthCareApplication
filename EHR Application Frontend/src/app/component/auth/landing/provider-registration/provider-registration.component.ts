import { CommonModule } from '@angular/common';
import { Component, inject, runInInjectionContext } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonService } from '../../../../core/utility/common.service';
import { AuthService } from '../../../../core/auth/auth.service';
import { ToasterService } from '../../../../core/utility/toaster.service';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-provider-registration',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './provider-registration.component.html',
  styleUrl: './provider-registration.component.css',
})
export class ProviderRegistrationComponent {
  pateintRegisterForm: FormGroup;
  selectedFile: File | null = null;
  private commonService = inject(CommonService);
  private authService = inject(AuthService);
  private toasterService = inject(ToasterService);
  private router = inject(Router);
  isLoading: boolean = false;
  states: any[] = [];
  countries: any[] = [];
  cities: any[] = [];
  todayDate = new Date().toISOString().split('T')[0];
  ngOnInit(): void {
    this.getAllCountries();
  }
  constructor(private fb: FormBuilder) {
    this.pateintRegisterForm = this.fb.group({
      firstName: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(12),
        ],
      ],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      dateOfBirth: ['', Validators.required],
      mobile: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
      address: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(25),
        ],
      ],

      pincode: [
        '',
        [Validators.required, Validators.min(100000), Validators.max(999999)],
      ],
      bloodGroupId: ['', [Validators.required]],
      countryId: [''],
      stateId: [''],
      cityId: [''],
      gender: ['', [Validators.required]],
      qualification: ['', [Validators.required]],
      specialisationId: ['', [Validators.required]],
      registrationNumber: ['', [Validators.required]],
      visitingCharge: ['', [Validators.required]],
    });
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
      input.value = value.slice(0, 6);
    }
  }

  removeNumbers(event: KeyboardEvent) {
    const charCode = event.charCode;

    if (charCode >= 48 && charCode <= 57) {
      event.preventDefault();
    }
  }

  onFileSelect(event: Event): void {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      this.selectedFile = file; // Store the file for uploading
      console.log('file selected', this.selectedFile);
    }
  }
  onCountryChange(event: any) {
    const selectedCountryId = +event.target.value;
    console.log('selectedCountryId', selectedCountryId);
    this.commonService.getAllStateByCountryId(selectedCountryId).subscribe({
      next: (result: any) => {
        this.states = result.state;
      },
      error: (error: Error) => {
        console.log(error);
      },
    });
  }

  onStateChange(event: any) {
    const selectedStateId = +event.target.value;
    console.log('selectedStateIdForCity', selectedStateId);
    this.commonService.getAllCityByStateId(selectedStateId).subscribe({
      next: (response) => {
        if (response.status == 200) {
          this.cities = response.cityresponse;
          console.log('cityname', this.cities);
        }
      },
      error: (error: Error) => {
        console.log('error city dropdown', error);
      },
    });
  }
  onSubmit() {
    if (this.pateintRegisterForm.valid) {
      const formData = new FormData();
      Object.keys(this.pateintRegisterForm.value).forEach((key) => {
        formData.append(key, this.pateintRegisterForm.value[key]);
      });

      if (this.selectedFile) {
        formData.append(
          'profileImageUrl',
          this.selectedFile,
          this.selectedFile.name
        );
        console.log('Profile image:', this.selectedFile.name);
      }

      formData.append('userTypeId', '2');

      this.isLoading = true;
      this.authService.registerPatient(formData).subscribe({
        next: (response) => {
          if (response.status == 200) {
            this.isLoading = true;
            this.router.navigateByUrl('/auth/login');
            this.toasterService.showSuccess(response.message);
          } else {
            this.toasterService.showError(response.message);
          }
        },
        error: (error) => {
          this.isLoading = false;
          this.toasterService.showError(error);
        },
      });
    } else {
      this.toasterService.showError('Please fill all required fields.');
      return;
    }
  }

  getAllCountries() {
    this.commonService.getAllCoutries().subscribe({
      next: (result: any) => {
        this.countries = result.country;
        console.log('countries data', this.countries);
      },
      error: (error: Error) => {
        console.log(error);
      },
    });
  }
}
