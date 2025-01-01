import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  inject,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../../../core/utility/common.service';
import { AppointmentService } from '../../../core/auth/appointment.service';
import { ToasterService } from '../../../core/utility/toaster.service';
import { JwtService } from '../../../core/utility/jwt.service';
import { AngularStripeService } from '@fireflysemantics/angular-stripe-service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-appointment',
  standalone: true,
  imports: [NavbarComponent, ReactiveFormsModule, CommonModule],
  templateUrl: './appointment.component.html',
  styleUrl: './appointment.component.css',
})
export class AppointmentComponent implements OnInit, AfterViewInit {
  @ViewChild('cardNumber', { static: false }) cardNumber!: ElementRef;
  @ViewChild('expiryInput', { static: false }) expiryInput!: ElementRef;
  @ViewChild('cvvInput', { static: false }) cvvInput!: ElementRef;
  specializationList?: any[];
  providersList?: any[];
  // private modalInstance: Modal | null = null;
  fee: number = 0;
  stripe: any;
  cardNumberElement: any;
  cardExpiryElement: any;
  cardCvcElement: any;
  stripeToken: any;
  error: any;
  errorbutton: boolean = false;
  isLoading: boolean = false;

  stripePaymentData:
    | { amount: any; customerName: string; customerEmail: string }
    | undefined;
  publisherKey =
    'pk_test_51QWiU5LbhFwVXMJv5qpYjm0FQV1VlQJjbZ02PpsBjgRM4yk9uTmQYrhndE6RPVH8Wa1VpKsAdN0tolfy6GIKJ5ER00ikws9sA7';

  filteredProviders = [];

  appointmentForm!: FormGroup;
  specialities: any[] = [];
  providers: any[] = [];
  providersFees: any[] = [];
  userTypeData: any[] = [];
  appointmentFormValue = {};
  currentUserType: number = 2;
  minTime: string = '00:00';
  today = new Date().toISOString().split('T')[0];
  private commonService = inject(CommonService);
  private appointmentService = inject(AppointmentService);
  private toasterService = inject(ToasterService);
  private jwtService = inject(JwtService);

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private cd: ChangeDetectorRef,
    private stripeService: AngularStripeService
  ) {}

  ngAfterViewInit() {
    this.initializeStripe();
  }

  initializeStripe() {
    this.stripeService.setPublishableKey(this.publisherKey).then((stripe) => {
      this.stripe = stripe;
      const elements = stripe.elements();
      this.cardNumberElement = elements.create('cardNumber', {
        placeholder: '4242424242424242',
      });
      this.cardExpiryElement = elements.create('cardExpiry', {
        placeholder: 'MM/YY',
      });
      this.cardCvcElement = elements.create('cardCvc', {
        placeholder: 'CVV',
      });

      this.cardNumberElement.mount(this.cardNumber.nativeElement);
      this.cardExpiryElement.mount(this.expiryInput.nativeElement);
      this.cardCvcElement.mount(this.cvvInput.nativeElement);
      this.cardNumberElement.addEventListener(
        'change',
        this.onChange.bind(this)
      );
      this.cardExpiryElement.addEventListener(
        'change',
        this.onChange.bind(this)
      );
      this.cardCvcElement.addEventListener('change', this.onChange.bind(this));
    });
  }
  onChange({ error }: { error: Error }) {
    if (error) {
      this.error = error.message;
      this.errorbutton = false;
    } else {
      this.error = null;
      this.errorbutton = true;
    }
    this.cd.detectChanges();
  }

  onProviderChange(event: any) {
    const selectedCountryId = +event.target.value;
    console.log('onProviderChange', selectedCountryId);
    this.appointmentService.getProviderList(2).subscribe({
      next: (result: any) => {
        this.providersFees = result.data;
        console.log('onProviderChange2', this.providersFees);
        this.providersFees = result.data.filter(
          (provider: any) => provider.visitingCharge === selectedCountryId
        );
        console.log('onProviderChange2', this.providersFees);
      },
      error: (error: Error) => {
        console.log(error);
      },
    });
  }

  resetAppointmentForm() {
    this.appointmentForm = this.fb.group({
      specialisationId: [0, Validators.required],
      providerId: ['', [Validators.required]],
      appointmentDate: ['', [Validators.required]],
      appointmentTime: ['', [Validators.required]],
      chiefComplaint: ['', [Validators.required]],
    });
  }

  ngOnInit() {
    const currentDate = new Date();
    this.today = currentDate.toISOString().slice(0, 10);

    this.appointmentForm = this.fb.group({
      patientId: this.jwtService.getUserId(),
      specialisationId: [],
      providerId: ['', Validators.required],
      appointmentDate: ['', Validators.required],
      appointmentTime: ['', Validators.required],
      chiefComplaint: ['', Validators.required],
      fees: ['', Validators.required],
    });

    this.getSpecialities();
    this.getAllProvider(this.currentUserType);
    this.appointmentForm
      .get('appointmentDate')
      ?.valueChanges.subscribe((selectedDate) => {
        this.updateMinTime(selectedDate);
      });

    const initialDate =
      this.appointmentForm.get('appointmentDate')?.value || this.today;
    this.updateMinTime(initialDate);
  }

  // BookAppointment() {
  //   if (this.appointmentForm.invalid) {
  //     return;
  //   }

  //   const payload = {
  //     providerId: this.appointmentForm.get('providerId')?.value,
  //     specialisationId: 1,
  //     patientId: this.jwtService.getUserId(),
  //     appointmentDate: this.appointmentForm.get('appointmentDate')?.value,
  //     appointmentTime: this.appointmentForm.get('appointmentTime')?.value,
  //     chiefComplaint: this.appointmentForm.get('chiefComplaint')?.value,
  //     amount: this.appointmentForm.get('fees')?.value,
  //     customerEmail: this.jwtService.getEmail(),
  //     customerName: this.jwtService.getFirstName(),
  //     sourceToken: this.stripeToken.id.toString(),
  //   };
  //   this.isLoading = true;
  //   console.log('payload', payload);
  //   this.appointmentService.addPayment(payload).subscribe({
  //     next: (response) => {
  //       if (response) {
  //         this.toasterService.showSuccess('Payment Done Successfully');
  //         this.closeModal();
  //         this.isLoading = false;
  //         this.resetAppointmentForm();
  //       } else {
  //         this.isLoading = false;
  //         this.toasterService.showSuccess('error at payment');
  //       }
  //     },
  //     error: (err: Error) => {
  //       this.toasterService.showError('unable to book appointment');
  //       this.isLoading = false;
  //     },
  //   });
  // }
  BookAppointment() {
    if (this.appointmentForm.invalid) {
      return;
    }

    const payload = {
      providerId: this.appointmentForm.get('providerId')?.value,
      specialisationId: 1,
      patientId: this.jwtService.getUserId(),
      appointmentDate: this.appointmentForm.get('appointmentDate')?.value,
      appointmentTime: this.appointmentForm.get('appointmentTime')?.value,
      chiefComplaint: this.appointmentForm.get('chiefComplaint')?.value,
      amount: this.appointmentForm.get('fees')?.value,
      customerEmail: this.jwtService.getEmail(),
      customerName: this.jwtService.getFirstName(),
      sourceToken: this.stripeToken.id.toString(),
    };

    this.isLoading = true;
    console.log('payload', payload);

    this.appointmentService.addPayment(payload).subscribe({
      next: (response) => {
        if (response) {
          Swal.fire({
            icon: 'success',
            title: 'Success',
            text: 'Appointment Booked Successfully',
          }).then(() => {
            this.closeModal();
            this.resetAppointmentForm();
          });
          this.isLoading = false;
        } else {
          this.isLoading = false;
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Error at Booking Appointment',
          });
        }
      },
      error: (err: Error) => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'Unable to book appointment',
        });
        this.isLoading = false;
      },
    });
  }

  async onClickPayButton() {
    if (
      !this.cardNumberElement ||
      !this.cardExpiryElement ||
      !this.cardCvcElement
    ) {
      alert('Stripe Elements are not initialized correctly');
      return;
    }

    const { token, error } = await this.stripe.createToken(
      this.cardNumberElement
    );
    if (token != undefined) {
      this.stripeToken = token;
      this.BookAppointment();
    } else {
      alert(error.message);
    }
  }

  onChangeProvider(event: Event) {
    const providerId = Number((event.target as HTMLSelectElement).value);
    const provider = this.providers?.find((p) => p.id === providerId);

    if (provider) {
      this.fee = provider.visitingCharge;
      this.appointmentForm.get('fees')?.setValue(provider.visitingCharge);
    }
  }

  getAllProvider(currentUserType: any) {
    console.log('usertype is', currentUserType);
    this.appointmentService.getProviderList(currentUserType).subscribe({
      next: (response) => {
        if (response.status == 200) {
          this.providers = response.data;
        }
      },
    });
  }

  getSpecialities() {
    this.commonService.getAllSpecialisation().subscribe({
      next: (response) => {
        if (response.status == 200) {
          this.specialities = response.data;
          console.log('specialities data', this.specialities);
        } else {
          alert('response not handler properly');
        }
      },
    });
  }

  getProviders(event: any) {
    const selectedProviderId = +event.target.value;
    this.providers = [];
    console.log('selectedproviderId', selectedProviderId);
    this.commonService
      .getProviderBySpecialisation(selectedProviderId)
      .subscribe({
        next: (response) => {
          if (response.status == 200) {
            this.providers = response.providers;

            console.log('providers data', this.providers);
          }
        },
        error: (response) => {
          console.log('unable to get response at specialisation', response);
        },
      });
  }

  onSubmit() {
    console.log('outside of the submit');
    if (this.appointmentForm.valid) {
      const payload = {
        patientId: this.jwtService.getUserId(),
        specialisationId: 1,
        providerId: this.appointmentForm.get('providerId')?.value,
        appointmentDate: this.appointmentForm.get('appointmentDate')?.value,
        appointmentTime: this.appointmentForm.get('appointmentTime')?.value,
        chiefComplaint: this.appointmentForm.get('chiefComplaint')?.value,
        Fees: this.appointmentForm.get('fees')?.value,
      };

      console.log('inside of the payload', payload);
      this.appointmentService.addAppointment(payload).subscribe({
        next: (response) => {
          if (response.status == 200) {
            this.toasterService.showSuccess('Appointment Booked Successfully');
            this.resetForm();
          } else {
            this.toasterService.showError('response not handled properly');
          }
        },
        error: (error) => {
          this.toasterService.showError('unable to get response');
          console.log(error);
        },
      });
    }
  }

  resetForm() {
    this.appointmentForm.reset();
  }

  //time code for particular data and time
  updateMinTime(selectedDate: string): void {
    const currentDate = new Date();
    const selectedDateObj = new Date(selectedDate);

    if (
      selectedDateObj.getFullYear() === currentDate.getFullYear() &&
      selectedDateObj.getMonth() === currentDate.getMonth() &&
      selectedDateObj.getDate() === currentDate.getDate()
    ) {
      // If today is selected, calculate the minimum time as 1 hour from now
      const nextHour = new Date(currentDate.getTime() + 60 * 60 * 1000); // Add 1 hour
      this.minTime = nextHour.toISOString().slice(11, 16); // Format as "HH:MM"
    } else {
      // For future dates, no restrictions
      this.minTime = '00:00';
    }
  }
  openModal() {
    if (this.appointmentForm.invalid) {
      return;
    }

    // Use Bootstrap's data attributes to toggle the modal
    const modalElement = document.getElementById('exampleModal');
    if (modalElement) {
      modalElement.classList.add('show'); // Add Bootstrap's `show` class
      modalElement.style.display = 'block'; // Display the modal
      modalElement.setAttribute('aria-modal', 'true');
      modalElement.removeAttribute('aria-hidden');

      const backdrop = document.createElement('div');
      backdrop.className = 'modal-backdrop fade show';
      document.body.appendChild(backdrop);
    }
  }
  closeModal() {
    const modalElement = document.getElementById('exampleModal');
    const backdrop = document.querySelector('.modal-backdrop');

    if (modalElement) {
      modalElement.classList.remove('show'); // Remove Bootstrap's `show` class
      modalElement.style.display = 'none'; // Hide the modal
      modalElement.setAttribute('aria-hidden', 'true');
      modalElement.removeAttribute('aria-modal');
    }

    if (backdrop) {
      backdrop.remove(); // Remove the backdrop from the DOM
    }
  }
}
