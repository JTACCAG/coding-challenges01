import { Component, signal } from '@angular/core';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { APP_ROUTES, AppRoute } from './shared/constants/routes';
import { RoleEnum } from './shared/enums/role';
import { IReport } from './shared/interfaces/report';
import { UserData } from './shared/interfaces/user-data';
import { AuthService } from './shared/services/auth.service';
import { ReportService } from './shared/services/report.service';

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
    MatCardModule,
    MatBadgeModule,
    MatMenuModule,
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
  lengthReport = 0;
  reports = signal<IReport[]>([]);

  constructor(
    private readonly authService: AuthService,
    private readonly reportService: ReportService,
    private readonly router: Router,
  ) {
    this.authService.restoreSession();
  }

  ngOnInit(): void {
    this.isAuthenticated.set(this.authService.isAuthenticated);
    if (this.isAuthenticated()) {
      this.user.set(this.authService.user);
      this.routes = APP_ROUTES.filter((r) => r.roles.includes(this.user()!.role));
      if (this.user()?.role === this.roleEnum.Admin) this.getReports();
    }
  }

  getReports() {
    this.reportService.findAll().subscribe({
      next: (res) => {
        this.reports.set(res.data);
        this.lengthReport = res.data.length;
      },
      error: (err) => {},
      complete: () => {},
    });
  }

  logout() {
    this.authService.logout();
    this.isAuthenticated.set(false);
    this.user.set(undefined);
    this.router.navigate(['/']);
  }

  timeAgo(date: string | Date): string {
    const now = new Date();
    const past = new Date(date);
    const diffMs = now.getTime() - past.getTime();

    const seconds = Math.floor(diffMs / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);
    const months = Math.floor(days / 30);
    const years = Math.floor(days / 365);

    if (seconds < 60) {
      return `${seconds} segundo${seconds !== 1 ? 's' : ''}`;
    }

    if (minutes < 60) {
      return `${minutes} minuto${minutes !== 1 ? 's' : ''}`;
    }

    if (hours < 24) {
      return `${hours} hora${hours !== 1 ? 's' : ''}`;
    }

    if (days < 30) {
      return `${days} día${days !== 1 ? 's' : ''}`;
    }

    if (months < 12) {
      return `${months} mes${months !== 1 ? 'es' : ''}`;
    }

    return `${years} año${years !== 1 ? 's' : ''}`;
  }
}
