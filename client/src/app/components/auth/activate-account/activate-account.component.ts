import { AfterViewInit, Component, inject, OnInit } from '@angular/core';
import { ActivateAccount } from '../../../shared/models/auth/auth.models';
import { ActivatedRoute } from '@angular/router';
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
  private router = inject(ActivatedRoute);
  private authService = inject(AuthService);
  private toastrService = inject(ToastrService);

  activateParams= new ActivateAccount();

  ngAfterViewInit(): void {
    this.router.queryParams.subscribe((params) => {
      ((this.activateParams.email = params['email']),
        (this.activateParams.token = params['code']));
    });

    this.authService.activate(this.activateParams).subscribe({
      next: (result) => {
        this.toastrService.success('Your account has been activated successfuly', 'Activated');
        console.log(result);
      },
      error: (err) => {
        //this.toastrService.error(err.error.message, 'Error');
        console.log(err);
      },
    });
  }
}
