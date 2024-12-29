import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';

bootstrapApplication(AppComponent, appConfig)
  .then(() => {
    // Remove the loader after the app is bootstrapped
    const loader = document.getElementById('global-loader');
    if (loader) {
      loader.style.display = 'none'; // Hide the loader
    }
  })
  .catch((err) => console.error(err));
