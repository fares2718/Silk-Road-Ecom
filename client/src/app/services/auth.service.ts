import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Register } from '../shared/models/auth/auth.models';
import { environment } from '../../environments/environment.development';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);

  private baseUrl: string = `${environment.baseUrl}/Auth`;

  register(registerModel:Register){
    return this.http.post(`${this.baseUrl}/register`,registerModel,{withCredentials:true});
  }
}
