import { Injectable, PLATFORM_ID, Inject } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Observable, tap, map } from 'rxjs';
import { Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';
import { ApiResponseWithData, AuthResponseData } from '../../../core/models/api-response.model';

export interface AuthRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  email: string;
  role: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private apiService: ApiService,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  login(credentials: { email: string; password: string }): Observable<AuthResponse> {
    console.log('Login attempt with:', credentials);
    return this.apiService
      .post<ApiResponseWithData<AuthResponse>>('Auth', credentials)
      .pipe(
        tap(rawResponse => {
          console.log('Raw API response (before mapping):', rawResponse);
        }),
        map(response => {
          if (!response.data) {
            throw new Error('Invalid response format');
          }
          return response.data.data as AuthResponse;
        }),
        tap(data => {
          if (isPlatformBrowser(this.platformId)) {
            console.log('Storing auth data:', data);
            localStorage.setItem('token', data.token);
            localStorage.setItem('user', JSON.stringify(data));
          }
        })
      );
  }

  logout() {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
    }
    this.router.navigate(['/login']);
  }

  isAuthenticated(): boolean {
    return isPlatformBrowser(this.platformId) && !!localStorage.getItem('token');
  }

  getToken(): string | null {
    if (isPlatformBrowser(this.platformId)) {
      const token = localStorage.getItem('token');
      console.log('getToken called - current token:', token);
      return token;
    }
    return null;
  }
} 