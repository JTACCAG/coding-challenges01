import { RoleEnum } from '../enums/role';

export interface IUser {
  id: string;
  fullname: string;
  email: string;
  role: RoleEnum;
  createdAt: Date;
}
