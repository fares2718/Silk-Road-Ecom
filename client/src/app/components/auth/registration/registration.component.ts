import { Component } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.scss',
})
export class RegistrationComponent {
  registerForm = new FormGroup(
    {
      firstName: new FormControl('', {
        validators: [
          Validators.required,
          Validators.maxLength(50),
          Validators.pattern(/^[\p{L}\s'-]+$/u),
        ],
      }),
      middleName: new FormControl('', {
        validators: [
          Validators.maxLength(50),
          Validators.pattern(/^[\p{L}\s'-]+$/u),
        ],
      }),
      lastName: new FormControl('', {
        validators: [
          Validators.required,
          Validators.maxLength(50),
          Validators.pattern(/^[\p{L}\s'-]+$/u),
        ],
      }),
      displayName: new FormControl('', {
        validators: [
          Validators.required,
          Validators.maxLength(50),
          Validators.pattern(/^[\p{L}\s'-]+$/u),
        ],
      }),
      username: new FormControl('', { validators: [Validators.required,Validators.pattern(/^[a-zA-Z0-9_-]{4,50}/u)]}),
      email: new FormControl('', {
        validators: [Validators.required, Validators.email],
      }),
      password: new FormControl('', {
        validators: [Validators.required, Validators.minLength(8),Validators.pattern(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/u)],
      }),

      confirmPassword: new FormControl('', {
        validators: [Validators.required],
      }),
    },
    { validators: [this.passwordMatchValidator] },
  );

  onSubmit() {}

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password')?.value;
    const confirm = control.get('confirmPassword')?.value;

    if (!password || !confirm) return null;

    return password === confirm ? null : { passwordMismatch: true };
  }
}
