import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ICategory } from '../interfaces/category';
import { IProduct } from '../interfaces/product';
import { IResponseApi } from '../interfaces/response-api';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  url = environment.apiUrl;

  constructor(private http: HttpClient) {}

  findAll() {
    return this.http.get<IResponseApi<ICategory[]>>(`${this.url}/category`);
  }

  created(product: IProduct) {
    return this.http.post<IResponseApi<ICategory>>(`${this.url}/category`, product);
  }

  updated(id: string, product: IProduct) {
    return this.http.patch<IResponseApi<ICategory>>(`${this.url}/category/${id}`, product);
  }
}
