import { inject, Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  RegisterResponse,
} from '../../shared/models/auth.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class Auth {
  private http = inject(HttpClient);
  private router = inject(Router);
  private readonly tokenKey = 'assetflow_token';

  register(request: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${environment.apiUrl}/auth/register`, request);
  }

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${environment.apiUrl}/auth/login`, request)
      .pipe(tap((res) => localStorage.setItem(this.tokenKey, res.token)));
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isLoggedIn(): boolean {
    const payload = this.decodeToken();
    if (!payload?.exp) return false;
    return payload.exp * 1000 > Date.now();
  }

  getRole(): string | null {
    const payload = this.decodeToken();
    if (!payload) return null;
    return (
      payload.role ??
      payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ??
      null
    );
  }

  private decodeToken(): any | null {
    const token = this.getToken();
    if (!token) return null;
    try {
      let b64 = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
      b64 += '='.repeat((4 - (b64.length % 4)) % 4); // restore base64 padding
      return JSON.parse(atob(b64));
    } catch {
      return null;
    }
  }
}
