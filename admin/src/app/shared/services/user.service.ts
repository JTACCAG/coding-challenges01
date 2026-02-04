import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { IResponseApi } from '../interfaces/response-api';
import { IUser } from '../interfaces/user';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  url = environment.apiUrl;

  constructor(private http: HttpClient) {}

  findAll() {
    return this.http.get<IResponseApi<IUser[]>>(`${this.url}/user`);
  }
}
