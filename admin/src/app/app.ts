import { Component, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { APP_ROUTES, AppRoute } from './shared/constants/routes';
import { RoleEnum } from './shared/enums/role';
import { UserData } from './shared/interfaces/user-data';
import { AuthService } from './shared/services/auth.service';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    MatToolbarModule,
    MatButtonModule,
    RouterModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  protected readonly title = signal('admin');
  isAuthenticated = signal<boolean>(false);
  user = signal<UserData | undefined>(undefined);
  roleEnum = RoleEnum;
  routes: AppRoute[] = [];

  constructor(
    private authService: AuthService,
    private readonly router: Router,
  ) {
    this.authService.restoreSession();
  }

  ngOnInit(): void {
    this.isAuthenticated.set(this.authService.isAuthenticated);
    if (this.isAuthenticated()) {
      this.user.set(this.authService.user);
      this.routes = APP_ROUTES.filter((r) => r.roles.includes(this.user()!.role));
      console.log(this.user());
    }
  }

  logout() {
    this.authService.logout();
    this.isAuthenticated.set(false);
    this.user.set(undefined);
    this.router.navigate(['/']);
  }
}
