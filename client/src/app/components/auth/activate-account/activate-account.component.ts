import { AfterViewInit, Component, inject, OnInit } from '@angular/core';
import { ActivateAccount } from '../../../shared/models/auth/auth.models';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-activate-account',
  standalone: true,
  imports: [],
  templateUrl: './activate-account.component.html',
  styleUrl: './activate-account.component.scss',
})
export class ActivateAccountComponent implements AfterViewInit {
  private route = inject(ActivatedRoute);
  private authService = inject(AuthService);
  private toastrService = inject(ToastrService);
  private router = inject(Router);

  activateParams= new ActivateAccount();

  ngAfterViewInit(): void {
    this.route.queryParams.subscribe((params) => {
      ((this.activateParams.email = params['email']),
        (this.activateParams.token = params['code']));
    });

    this.authService.activate(this.activateParams).subscribe({
      next: (result) => {
        this.toastrService.success('Your account has been activated successfuly', 'Activated');
        this.router.navigate(['/login']);
        console.log(result);
      },
      error: (err) => {
        this.toastrService.error(err.error.message, 'Error');
        console.log(err);
      },
    });
  }
}
