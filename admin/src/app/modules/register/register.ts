import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router, RouterLink } from '@angular/router';
import { Snackbar } from '../../shared/components/snackbar/snackbar';
import { IResponseApi } from '../../shared/interfaces/response-api';
import { AuthService } from '../../shared/services/auth.service';
import { IamService } from '../../shared/services/iam.service';
import { LoadingService } from '../../shared/services/loading.service';

@Component({
  selector: 'app-register',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    RouterLink,
  ],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register implements OnInit {
  private _snackBar = inject(MatSnackBar);
  private loadingService = inject(LoadingService);
  form: FormGroup;
  loading = false;

  constructor(
    private readonly fb: FormBuilder,
    private readonly iamService: IamService,
    private readonly authService: AuthService,
    private readonly router: Router,
  ) {
    this.form = this.fb.group(
      {
        email: ['', [Validators.required, Validators.email]],
        fullname: ['', [Validators.required, Validators.minLength(3)]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required],
      },
      {
        validators: this.passwordsMatchValidator,
      },
    );
  }

  ngOnInit(): void {
    if (this.authService.isAuthenticated) this.router.navigate(['/admin']);
  }

  passwordsMatchValidator(form: FormGroup) {
    const passwordControl = form.get('password');
    const confirmControl = form.get('confirmPassword');

    if (!passwordControl || !confirmControl) return null;

    const password = passwordControl.value;
    const confirmPassword = confirmControl.value;

    if (password !== confirmPassword) {
      confirmControl.setErrors({ passwordsNotMatch: true });
    } else {
      // limpiar solo este error, sin borrar otros
      if (confirmControl.hasError('passwordsNotMatch')) {
        confirmControl.setErrors(null);
      }
    }

    return null;
  }

  register() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
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
    this.loading = true;
    this.iamService.register(this.form.value).subscribe({
      next: (res) => {
        this.authService.login(res.data.accessToken, res.data.user);
      },
      error: (err: HttpErrorResponse) => {
        console.log(err);
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
