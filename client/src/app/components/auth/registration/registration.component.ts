import { Component, inject } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { Register } from '../../../shared/models/auth/auth.models';
import { AuthService } from '../../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

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
      username: new FormControl('', {
        validators: [
          Validators.required,
          Validators.pattern(/^[a-zA-Z0-9_-]{4,50}/u),
        ],
      }),
      email: new FormControl('', {
        validators: [Validators.required, Validators.email],
      }),
      password: new FormControl('', {
        validators: [
          Validators.required,
          Validators.minLength(8),
          Validators.pattern(
            /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/u,
          ),
        ],
      }),

      confirmPassword: new FormControl('', {
        validators: [Validators.required],
      }),
    },
    { validators: [this.passwordMatchValidator] },
  );

  private authService = inject(AuthService);
  private toastrService = inject(ToastrService);
  private router = inject(Router);

  hidePassword = true;
  hideConfirmPassword = true;

  registerModel: Register;

  onSubmit() {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }
    this.registerModel = {
      email: this.registerForm.controls.email.value,
      password: this.registerForm.controls.password.value,
      userName: this.registerForm.controls.username.value,
      displayName: this.registerForm.controls.displayName.value,
      firstName: this.registerForm.controls.firstName.value,
      middleName: this.registerForm.controls.middleName.value,
      lastName: this.registerForm.controls.lastName.value,
    };

    this.authService.register(this.registerModel).subscribe({
      next: (result) => {
        this.toastrService.success('Please confirm your email', 'Registered');
        this.router.navigateByUrl('/login');
      },
      error: (err) => {
        this.toastrService.error(err.error.message, 'Error');
      },
    });
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password')?.value;
    const confirm = control.get('confirmPassword')?.value;

    if (!password || !confirm) return null;

    return password === confirm ? null : { passwordMismatch: true };
  }
}
