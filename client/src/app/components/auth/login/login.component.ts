import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Login } from '../../../shared/models/auth/auth.models';
import { AuthService } from '../../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  loginForm = new FormGroup({
    email: new FormControl('', {
      validators: [Validators.required, Validators.email],
      nonNullable: true,
    }),

    password: new FormControl('', {
      validators: [Validators.required, Validators.minLength(8)],
      nonNullable: true,
    }),
  });

  private authService = inject(AuthService);
  private toastrService = inject(ToastrService);
  private router = inject(Router);

  login: Login;

  hidePassword:boolean = false;
  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.login = {
      email:this.loginForm.controls.email.value,
      password:this.loginForm.controls.password.value,
    }

    this.authService.login(this.login).subscribe({
      next: (result) => {
        this.toastrService.success('you are loged in successfuly', 'Success');
        this.router.navigate(['/home'])
        console.log(result);
      },
      error: (err) => {
        this.toastrService.error(err.error.message, 'Error');
        console.log(err);
      },
    });
  }
}
