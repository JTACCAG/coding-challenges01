import { RoleEnum } from '../enums/role';

export interface AppRoute {
  icon: string;
  path: string;
  label: string;
  roles: RoleEnum[];
}

export const APP_ROUTES: AppRoute[] = [
  {
    icon: 'home',
    path: '/admin/',
    label: 'Home',
    roles: [RoleEnum.Admin, RoleEnum.Regular],
  },
  {
    icon: 'person',
    path: '/admin/user',
    label: 'User',
    roles: [RoleEnum.Admin],
  },
  {
    icon: 'inventory_2',
    path: '/admin/inventory',
    label: 'Inventory',
    roles: [RoleEnum.Admin, RoleEnum.Regular],
  },
  {
    icon: 'account_circle',
    path: '/admin/account',
    label: 'Account',
    roles: [RoleEnum.Regular],
  },
];
