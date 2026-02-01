import { Routes } from '@angular/router';
import { Auth } from './modules/auth/auth';
import { Home } from './modules/home/home';
import { Inventory } from './modules/inventory/inventory';
import { NotFound } from './modules/not-found/not-found';
import { User } from './modules/user/user';
import { AuthGuard } from './shared/guards/auth.guard';

export const routes: Routes = [
  // Rutas públicas
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
  { path: '**', component: NotFound },
];
