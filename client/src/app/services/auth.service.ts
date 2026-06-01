import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ActivateAccount, Login, Register } from '../shared/models/auth/auth.models';
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

  login(login:Login){
    return this.http.post(`${this.baseUrl}/login`,login);
  }

  register(registerModel:Register){
    return this.http.post(`${this.baseUrl}/register`,registerModel);
  }
}
