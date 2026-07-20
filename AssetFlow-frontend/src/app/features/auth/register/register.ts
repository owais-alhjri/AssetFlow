import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Router, RouterLink } from '@angular/router';
import { Auth } from '../../../core/auth/auth';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { EMAIL_PATTERN } from '../../../shared/validators/email.validator';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatIconModule,
    RouterLink,
    MatSnackBarModule,
  ],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  private fb = inject(FormBuilder);
  private auth = inject(Auth);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  form = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.pattern(EMAIL_PATTERN)]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  isSubmitting = signal(false);
  errorMessage = signal('');
  successMessage = 'registration received — an admin will review your request';
  hidePassword = signal(true);

  get email() {
    return this.form.get('email')!;
  }
  get password() {
    return this.form.get('password')!;
  }

  onSubmit() {
    if (this.form.invalid) return;

      this.isSubmitting.set(true);
    this.errorMessage.set('');

    this.auth.register(this.form.getRawValue()).subscribe({
      next: () => {
          this.snackBar.open(this.successMessage, 'Close', {
            duration: 9000,
            horizontalPosition: 'center',
            verticalPosition: 'bottom',
            panelClass: ['success-snackbar'],
          });
          this.router.navigate(['/login']);
      },
      error: (err) => {
        this.isSubmitting.set(false);
        this.errorMessage.set(
          err?.status === 409
            ? 'An account with this email already exists.'
            : 'Something went wrong. Please try again.'
        );
      },
    });
  }
}
