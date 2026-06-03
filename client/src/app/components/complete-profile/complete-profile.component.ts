import { Component, inject, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { NgSelectModule } from '@ng-select/ng-select';
import { CompleteProfileService } from '../../services/complete-profile.service';
import { City, Country, State } from '../../shared/models/address.models';
import { Router } from '@angular/router';

@Component({
  selector: 'app-complete-profile',
  standalone: true,
  imports: [NgSelectModule, ReactiveFormsModule],
  templateUrl: './complete-profile.component.html',
  styleUrls: ['./complete-profile.component.scss'],
})
export class CompleteProfileComponent implements OnInit {
  step = 1;

  countries: Country[] = [];
  states: State[] = [];
  cities: City[] = [];

  profileForm = new FormGroup({
    country: new FormControl<number | null>(null, Validators.required),

    state: new FormControl<number | null>(
      { value: null, disabled: true },
      Validators.required,
    ),

    city: new FormControl<number | null>(
      { value: null, disabled: true },
      Validators.required,
    ),

    street: new FormControl('', Validators.required),

    zipCode: new FormControl('', Validators.required),
  });

  private copleteProfileService = inject(CompleteProfileService);
  private router = inject(Router);

  ngOnInit(): void {
    this.loadCountries();

    this.country?.valueChanges.subscribe((countryId) => {
      this.state?.reset();
      this.city?.reset();

      this.city?.disable();

      if (!countryId) {
        this.states = [];
        this.state?.disable();

        return;
      }

      this.loadStates(countryId);

      this.state?.enable();
    });

    this.state?.valueChanges.subscribe((stateId) => {
      this.city?.reset();

      if (!stateId) {
        this.cities = [];
        this.city?.disable();

        return;
      }

      this.loadCities(stateId);

      this.city?.enable();
    });
  }

  get country() {
    return this.profileForm.controls.country;
  }

  get state() {
    return this.profileForm.controls.state;
  }

  get city() {
    return this.profileForm.controls.city;
  }

  nextStep(): void {
    this.step = 2;
  }

  previousStep(): void {
    this.step = 1;
  }

  saveProfile(): void {
    // TODO: call API
  }

  private loadCountries(): void {
    this.copleteProfileService.getAllCountries().subscribe({
      next: (countries) => {
        this.countries = countries;
      },
    });
  }

  private loadStates(countryId: number): void {
    this.copleteProfileService.getStatesByCountry(countryId).subscribe({
      next: (states) => {
        this.states = states;
      },
    });
  }

  private loadCities(stateId: number): void {
    this.copleteProfileService.getCitiesByState(stateId).subscribe({
      next: (cities) => {
        this.cities = cities;
      },
    });
  }

  onSearchCountries(searchTerm: string): void {
    this.copleteProfileService.getAllCountries(searchTerm).subscribe({
      next: (countries) => {
        this.countries = countries;
      },
    });
  }

  onSearchStates(searchTerm: string): void {
    const countryId = this.country?.value;

    if (!countryId) {
      return;
    }

    this.copleteProfileService
      .getStatesByCountry(countryId, searchTerm)
      .subscribe({
        next: (states) => {
          this.states = states;
        },
      });
  }

  onSearchCities(searchTerm: string): void {
    const stateId = this.state?.value;

    if (!stateId) {
      return;
    }

    this.copleteProfileService.getCitiesByState(stateId, searchTerm).subscribe({
      next: (cities) => {
        this.cities = cities;
      },
    });
  }
}
