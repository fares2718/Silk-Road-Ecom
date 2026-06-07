import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { BehaviorSubject, catchError, filter, switchMap, take, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import { Router } from '@angular/router';


let isRefreshing = false;
const refreshTokenSubject: BehaviorSubject<boolean | null> = new BehaviorSubject<boolean | null>(null);
export const refreshTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // 1. Force withCredentials on every outbound call so HttpOnly cookies are attached
  let clonedRequest = req.clone({ withCredentials: true });

  return next(clonedRequest).pipe(
    catchError((error: HttpErrorResponse) => {
      
      // 2. Catch expired access tokens (401), but bypass the login path to avoid loops on bad inputs
      if (error.status === 401 && !req.url.includes('/login')) {
        
        // 3. If a refresh request is ALREADY in mid-flight, pause and queue this request
        if (isRefreshing) {
          return refreshTokenSubject.pipe(
            filter(result => result !== null), // Wait until the value changes from null to true/false
            take(1),                           // Unsubscribe instantly after getting the resolution
            switchMap(() => next(req.clone({ withCredentials: true }))) // Retry original call with new cookies
          );
        }

        // 4. If this is the FIRST request to hit 401, initiate the refresh lock
        isRefreshing = true;
        refreshTokenSubject.next(null); // Clear the queue's signal state

        // 5. Fire off the background refresh network payload
        return authService.refresh().pipe(
          switchMap(() => {
            isRefreshing = false;
            refreshTokenSubject.next(true); // Signal to all waiting queued calls that cookies are updated
            return next(req.clone({ withCredentials: true })); // Retry the original request
          }),
          catchError((refreshError) => {
            // 6. Complete failure: The refresh token itself is dead or revoked
            isRefreshing = false;
            refreshTokenSubject.next(false); // Clear lock
            authService.clearSession();      // Set internal app authentication status to false
            router.navigate(['/login']);    // Route the user back to the screen
            return throwError(() => refreshError);
          })
        );
      }

      // Pass along other error statuses (400, 403, 500, etc.) immediately
      return throwError(() => error);
    })
  );
};
