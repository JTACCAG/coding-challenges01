import { Routes } from '@angular/router';
import { Auth } from './modules/auth/auth';
import { Home } from './modules/home/home';
import { Inventory } from './modules/inventory/inventory';
import { Register } from './modules/register/register';
import { User } from './modules/user/user';
import { RoleEnum } from './shared/enums/role';
import { AuthGuard } from './shared/guards/auth.guard';
import { RoleGuard } from './shared/guards/role.guard';

export const routes: Routes = [
  {
    path: '',
    component: Auth,
  },
  {
    path: 'register',
    component: Register,
  },
  // Rutas privadas (protegidas por guard)
  {
    path: 'admin',
    canActivate: [AuthGuard, RoleGuard],
    children: [
      {
        path: '',
        component: Home,
        data: {
          roles: [RoleEnum.Admin, RoleEnum.Regular],
        },
      },
      {
        path: 'user',
        component: User,
        data: {
          roles: [RoleEnum.Admin],
        },
      },
      {
        path: 'inventory',
        component: Inventory,
        data: {
          roles: [RoleEnum.Admin, RoleEnum.Regular],
        },
      },
    ],
  },
];
