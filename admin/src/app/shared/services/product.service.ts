import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { IProduct } from '../interfaces/product';
import { IResponseApi } from '../interfaces/response-api';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  url = environment.apiUrl;

  constructor(private http: HttpClient) {}

  findAll() {
    return this.http.get<IResponseApi<IProduct[]>>(`${this.url}/product`);
  }

  findOne(id: string) {
    return this.http.get<IResponseApi<IProduct>>(`${this.url}/product/${id}`);
  }

  created(product: IProduct) {
    return this.http.post<IResponseApi<IProduct>>(`${this.url}/product`, product);
  }

  updated(id: string, product: IProduct) {
    return this.http.patch<IResponseApi<IProduct>>(`${this.url}/product/${id}`, product);
  }

  deleted(id: string) {
    return this.http.delete<IResponseApi<IProduct>>(`${this.url}/product/${id}`);
  }

  getReport() {
    return this.http.get(`${this.url}/product/report`, {
      responseType: 'blob',
    });
  }
}
