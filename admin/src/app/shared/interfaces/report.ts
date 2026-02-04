import { IProduct } from './product';
import { IUser } from './user';

export interface IReport {
  id?: string;
  userId: string;
  user?: IUser;
  productId: string;
  product?: IProduct;
  reason: string;
  createdAt?: string;
}
