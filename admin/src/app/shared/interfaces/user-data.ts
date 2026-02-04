import { RoleEnum } from '../enums/role';

export interface UserData {
  id: string;
  email: string;
  role: RoleEnum;
}
