import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors, } from '@angular/common/http';
import { authInterceptor } from './core/auth/auth.interceptor';
import { provideToastr } from 'ngx-toastr';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes), provideAnimations(), provideToastr({
    preventDuplicates: true,
    timeOut: 2500,
    positionClass: 'toast-top-right',
    closeButton: false,
    progressBar: false
  }), provideHttpClient(
    withInterceptors([authInterceptor]),
  )],
};
