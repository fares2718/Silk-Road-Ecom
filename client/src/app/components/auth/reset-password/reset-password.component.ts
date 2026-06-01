import { Component, inject } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { ResetPassword } from '../../../shared/models/auth/auth.models';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
})
export class ResetPasswordComponent {
  resetPasswordForm = new FormGroup(
    {
      password: new FormControl('', {
        validators: [
          Validators.required,
          Validators.minLength(8),
          Validators.pattern(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$/u),
        ],
        nonNullable: true,
      }),

      confirmPassword: new FormControl('', {
        validators: [Validators.required],
        nonNullable: true,
      }),
    },
    {
      validators: [this.passwordMatchValidator],
    },
  );

  private authSrevice = inject(AuthService);
  private router = inject(ActivatedRoute);
  private toastrService = inject(ToastrService);

  resetPasswordModel: ResetPassword;

  hidePassword = true;
  hideConfirmPassword = true;

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password')?.value;

    const confirmPassword = control.get('confirmPassword')?.value;

    if (password !== confirmPassword) {
      return {
        passwordMismatch: true,
      };
    }

    return null;
  }

  get password() {
    return this.resetPasswordForm.controls.password;
  }

  get confirmPassword() {
    return this.resetPasswordForm.controls.confirmPassword;
  }

  onSubmit(): void {
    if (this.resetPasswordForm.invalid) {
      this.resetPasswordForm.markAllAsTouched();

      return;
    }
    this.router.queryParams.subscribe((params) => {
      this.resetPasswordModel = {
        email: params['email'],
        token: params['code'],
        password: this.password.value,
      };
    });

    this.authSrevice.resetPassword(this.resetPasswordModel).subscribe({
      next: (result) => {
        this.toastrService.success('Password has been reset', 'Success');
        console.log(result);
      },
      error: (err) => {
        this.toastrService.error(err.error.message, 'Error');
        console.log(err);
      },
    });
  }
}
