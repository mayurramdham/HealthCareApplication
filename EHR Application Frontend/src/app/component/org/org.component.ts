import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { routes } from '../../app.routes';

@Component({
  selector: 'app-org',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './org.component.html',
  styleUrl: './org.component.css',
})
export class OrgComponent {}
