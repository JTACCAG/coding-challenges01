import { ICategory } from './category';

export interface IProduct {
  id: string;
  name: string;
  description: string;
  price: number;
  stockQuantity: number;
  categories: ICategory[];
  updatedAt: string;
  deletedAt: string;
}
