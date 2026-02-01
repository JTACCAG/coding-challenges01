import { isPlatformBrowser } from '@angular/common';
import { inject, Injectable, PLATFORM_ID, signal } from '@angular/core';
import { UserData } from '../interfaces/user-data';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private platformId = inject(PLATFORM_ID);
  private _isAuthenticated = signal<boolean>(false);
  authState = this._isAuthenticated.asReadonly();

  private get isBrowser(): boolean {
    return isPlatformBrowser(this.platformId);
  }

  get isAuthenticated(): boolean {
    return this._isAuthenticated();
  }

  get token(): string {
    return localStorage.getItem('access_token') || '';
  }

  get user(): UserData {
    return JSON.parse(localStorage.getItem('user') || '{}');
  }

  login(token: string, user: UserData): boolean {
    if (token) {
      this._isAuthenticated.set(true);
      if (this.isBrowser) {
        localStorage.setItem('access_token', token);
        localStorage.setItem('user', JSON.stringify(user));
      }
      return true;
    }
    return false;
  }

  logout(): void {
    this._isAuthenticated.set(false);
    if (this.isBrowser) {
      localStorage.removeItem('access_token');
      localStorage.removeItem('user');
    }
  }

  restoreSession(): void {
    if (!this.isBrowser) return;
    const token = localStorage.getItem('access_token');
    console.log(token);
    if (token) {
      this._isAuthenticated.set(true);
    }
  }
}
