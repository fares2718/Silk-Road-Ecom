import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { City, Country, State } from '../shared/models/address.models';
import { CompleteProfile } from '../shared/models/complete-profile.model';

@Injectable({
  providedIn: 'root'
})
export class CompleteProfileService {
  private http = inject(HttpClient);

  private baseUrl = `${environment.baseUrl}/CompleteAccount`;

  completeProfile(data: CompleteProfile) {
    return this.http.post(`${this.baseUrl}/complete-account`, data);
  }
  
  getAllCountries(searchTerm: string ='') {
    return this.http.get<Country[]>(`${this.baseUrl}/all-countries?searchTerm=${searchTerm}`);
  }

  getCitiesByState(stateId: number, searchTerm: string ='') {
    return this.http.get<City[]>(`${this.baseUrl}/cities-by-state/${stateId}?searchTerm=${searchTerm}`);
  }

  getStatesByCountry(countryId: number, searchTerm: string ='') {
    return this.http.get<State[]>(`${this.baseUrl}/states-by-country/${countryId}?searchTerm=${searchTerm}`);
  }
}