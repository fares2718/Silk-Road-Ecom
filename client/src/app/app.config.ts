import {
  ApplicationConfig,
  importProvidersFrom,
  provideZoneChangeDetection,
} from '@angular/core';
import {
  provideRouter,
  withComponentInputBinding,
  withRouterConfig,
} from '@angular/router';

import { routes } from './app.routes';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { loaderInterceptor } from './interceptors/loader.interceptor';
import { BrowserAnimationsModule, provideAnimations } from '@angular/platform-browser/animations';
import { NgxSpinnerModule } from 'ngx-spinner';
import { provideToastr } from 'ngx-toastr';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideClientHydration(),
    provideHttpClient(withInterceptors([loaderInterceptor])),
    provideRouter(
      routes,
      withComponentInputBinding(),
      withRouterConfig({
        paramsInheritanceStrategy: 'always',
      }),
    ),
    //provideAnimations(),
    importProvidersFrom(
      BrowserModule,
      BrowserAnimationsModule,
      NgxSpinnerModule.forRoot({ type: 'square-jelly-box' }),
    ),
    provideToastr({
      closeButton:true,
      positionClass:'toast-top-right',
      timeOut: 2000,
      countDuplicates:true,
      progressBar:true
    })
  ],
};
