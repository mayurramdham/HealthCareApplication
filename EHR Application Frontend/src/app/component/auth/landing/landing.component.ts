import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.css',
})
export class LandingComponent {
  constructor(private router: Router) {}

  navigateToProvider() {
    this.router.navigate(['auth/providerRegistration']);
  }

  navigateToPatient() {
    this.router.navigate(['auth/patientRegistration']);
  }
}
