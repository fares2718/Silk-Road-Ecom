import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { City, Country, State } from '../shared/models/lookup-item.model';

@Injectable({
  providedIn: 'root'
})
export class CompleteProfileService {
  private http = inject(HttpClient);

  private baseUrls = `${environment.baseUrl}/CompleteAccount`;
  
  getAllCountries(searchTerm: string ='') {
    return this.http.get<Country[]>(`${this.baseUrls}/all-countries?searchTerm=${searchTerm}`);
  }

  getCitiesByState(stateId: number, searchTerm: string ='') {
    return this.http.get<City[]>(`${this.baseUrls}/cities-by-state/${stateId}?searchTerm=${searchTerm}`);
  }

  getStatesByCountry(countryId: number, searchTerm: string ='') {
    return this.http.get<State[]>(`${this.baseUrls}/states-by-country/${countryId}?searchTerm=${searchTerm}`);
  }
}