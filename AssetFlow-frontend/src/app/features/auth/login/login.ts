import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Router } from '@angular/router';
import { Auth } from '../../../core/auth/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule,
    MatFormFieldModule, MatInputModule,
    MatButtonModule, MatCardModule,
    MatProgressSpinnerModule, MatIconModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {

  private fb = inject(FormBuilder);
  private auth = inject(Auth)
  private router = inject(Router);

  form = this.fb.nonNullable.group({
    email: ['',[Validators.required, Validators.email]],
    password:['',[Validators.required, Validators.minLength(6)]]
  });

  isSubmitting = false;
  errorMessage = '';
  hidePassword = true;

  get email(){return this.form.get('email')!;}
  get password(){return this.form.get('password')!;}

  onSubmit(){
    if(this.form.invalid) return;

    this.isSubmitting = true;
    this.errorMessage = '';
    const {email, password} = this.form.getRawValue();

    this.auth.login(email, password).subscribe({
      next: () => this.router.navigate(['/']),
      error:(err)=>{
        this.isSubmitting = false;
        this.errorMessage = err?.status === 401
        ? 'Invalid email or password'
        : 'Something went wrong. Please try again.'; 
      }
    })
  }

}
