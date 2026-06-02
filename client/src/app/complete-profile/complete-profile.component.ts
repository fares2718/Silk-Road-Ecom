import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { NgSelectModule } from '@ng-select/ng-select';

interface LookupItem {
  id: number;
  name: string;
}

@Component({
  selector: 'app-complete-profile',
  standalone: true,
  imports: [NgSelectModule, ReactiveFormsModule],
  templateUrl: './complete-profile.component.html',
  styleUrls: ['./complete-profile.component.scss'],
})
export class CompleteProfileComponent implements OnInit {
  step = 1;

  countries: LookupItem[] = [];
  states: LookupItem[] = [];
  cities: LookupItem[] = [];

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

  private loadCountries(): void {}

  private loadStates(countryId: number): void {
    // Replace with API later
  }

  private loadCities(stateId: number): void {
    // Replace with API later
  }
}
