import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ActivateAccount, Login, Register, ResetPassword } from '../shared/models/auth/auth.models';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);

  private baseUrl: string = `${environment.baseUrl}/Auth`;

  activate(params:ActivateAccount){
    return this.http.post(`${this.baseUrl}/activate-account`,params);
  }

  forgetPassword(email:string){
    return this.http.post(`${this.baseUrl}/send-forget-password-email?email=${email}`,{});
  }

  login(login:Login){
    return this.http.post(`${this.baseUrl}/login`,login);
  }

  register(registerModel:Register){
    return this.http.post(`${this.baseUrl}/register`,registerModel);
  }

  resetPassword(resetPassword:ResetPassword){
    return this.http.post(`${this.baseUrl}/reset-password`,resetPassword);
  }
}
