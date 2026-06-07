import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import {
  ActivateAccount,
  Login,
  Register,
  ResetPassword,
} from '../shared/models/auth/auth.models';
import { environment } from '../../environments/environment.development';
import { BehaviorSubject, catchError, of, tap, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);

  private baseUrl: string = `${environment.baseUrl}/Auth`;

  private authState$ = new BehaviorSubject<boolean | null>(null);
  isLoggedIn$ = this.authState$.asObservable();

  checkAuthenticationStatus() {
    return this.http
      .get<boolean>(`${this.baseUrl}/is-authenticated`, {
        withCredentials: true,
      })
      .pipe(
        tap((isValid) => this.authState$.next(isValid)),
        catchError(() => {
          this.authState$.next(false);
          return of(false);
        }),
      );
  }

  activate(params: ActivateAccount) {
    return this.http.post(`${this.baseUrl}/activate-account`, params);
  }

  forgetPassword(email: string) {
    return this.http.post(
      `${this.baseUrl}/send-forget-password-email?email=${email}`,
      {},
    );
  }

  login(login: Login) {
    return this.http.post(`${this.baseUrl}/login`, login, {
      withCredentials: true,
    });
  }

  logout() {
    return this.http
      .post(`${this.baseUrl}/logout`, {}, { withCredentials: true })
      .pipe(
        tap(() => this.clearSession()),
        catchError((err) => {
          this.clearSession();
          return throwError(() => err);
        }),
      );
  }

  clearSession(): void {
    this.authState$.next(false);
  }

  refresh() {
    return this.http
      .post(`${this.baseUrl}/refresh`, {}, { withCredentials: true })
      .pipe(tap(() => this.authState$.next(true)));
  }

  register(registerModel: Register) {
    return this.http.post(`${this.baseUrl}/register`, registerModel);
  }

  resetPassword(resetPassword: ResetPassword) {
    return this.http.post(`${this.baseUrl}/reset-password`, resetPassword);
  }
}
