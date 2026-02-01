import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Snackbar } from '../../shared/components/snackbar/snackbar';
import { IResponseApi } from '../../shared/interfaces/response-api';
import { AuthService } from '../../shared/services/auth.service';
import { IamService } from '../../shared/services/iam.service';
import { LoadingService } from '../../shared/services/loading.service';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatCardModule,
    MatButtonModule,
    ReactiveFormsModule,
  ],
  templateUrl: './auth.html',
  styleUrl: './auth.scss',
})
export class Auth {
  private _snackBar = inject(MatSnackBar);
  private loadingService = inject(LoadingService);
  loading = false;

  form: FormGroup;

  constructor(
    private readonly fb: FormBuilder,
    private readonly iamService: IamService,
    private readonly authService: AuthService,
    private readonly router: Router,
  ) {
    this.form = this.fb.group({
      email: ['jtaccag@gmail.com', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  login() {
    if (this.form.invalid) {
      this._snackBar.openFromComponent(Snackbar, {
        duration: 3000,
        verticalPosition: 'top',
        horizontalPosition: 'right',
        data: {
          message: 'Complete los campos requeridos',
          type: 'warning',
        },
      });
      return;
    }
    const data = {
      email: this.form.get('email')?.value as string,
      password: this.form.get('password')?.value as string,
    };
    this.loadingService.show();
    this.loading = true;
    this.form.disable();
    this.iamService.login(data.email, data.password).subscribe({
      next: (res) => {
        this.authService.login(res.data.accessToken, res.data.user);
      },
      error: (err: HttpErrorResponse) => {
        const errorMessage = err.error ? (err.error as IResponseApi).message : err.message;
        this._snackBar.openFromComponent(Snackbar, {
          duration: 3000,
          verticalPosition: 'top',
          horizontalPosition: 'right',
          data: {
            message: errorMessage,
            type: 'error',
          },
        });
        //
        this.loading = false;
        this.form.enable();
        this.loadingService.hide();
      },
      complete: () => {
        this.loadingService.hide();
        // this.router.navigate(['/admin']);
        window.location.href = '/admin';
      },
    });
  }
}
