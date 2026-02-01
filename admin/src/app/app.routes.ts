import { Routes } from '@angular/router';
import { Auth } from './modules/auth/auth';
import { AuthGuard } from './shared/guards/auth.guard';
import { Home } from './modules/home/home';
import { User } from './modules/user/user';
import { Inventory } from './modules/inventory/inventory';

export const routes: Routes = [
  {
    path: '',
    component: Auth,
  },
  // Rutas privadas (protegidas por guard)
  {
    path: 'admin',
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        component: Home,
      },
      {
        path: 'user',
        component: User,
      },
      {
        path: 'inventory',
        component: Inventory,
      },
    ],
  },
];
