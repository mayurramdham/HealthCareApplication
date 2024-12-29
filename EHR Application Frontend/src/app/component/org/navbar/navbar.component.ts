import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { JwtService } from '../../../core/utility/jwt.service';
import { AuthService } from '../../../core/auth/auth.service';
import { ToasterService } from '../../../core/utility/toaster.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {
  userId?: number;
  usersData: any = {};
  userName: string = '';
  isAdmin = false;
  currentRole: string = '';
  cartItemCount: number = 0;
  cartCaunt: number = 0;

  constructor(private router: Router) {
    this.currentRole = this.jwtService.getRole();
    this.userName = this.jwtService.getFirstName();
    this.userId = this.jwtService.getUserId();
  }
  ngOnInit(): void {
    if (this.userId) this.getUserData(this.userId);
    console.log('currentRole', this.currentRole);
  }
  private jwtService = inject(JwtService);
  private authService = inject(AuthService);
  private toaster = inject(ToasterService);

  logout() {
    Swal.fire({
      title: 'Are You Sure',
      text: 'Are You Sure You want to logout from the application?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, Logout from application',
      cancelButtonText: 'No, keep it log in',
    }).then((result) => {
      if (result.isConfirmed) {
        localStorage.clear();
        localStorage.removeItem('accessToken');
        localStorage.removeItem('userId');
        localStorage.removeItem('userName');
        this.router.navigate(['auth/landing']); // Redirect to login page
      }
    });
  }
  profile() {
    this.router.navigate(['/org/profile']);
  }
  goToCart() {
    this.router.navigate(['/product/cart']);
  }

  getUserData(userId: number) {
    console.log('id', userId);
    this.authService.getUserById(userId).subscribe({
      next: (response) => {
        if (response.status == 200) {
          this.usersData = response.data;
        } else {
          console.log('invalid response');
        }
      },
      error: (response) => {
        console.error(response);
      },
    });
  }
}
