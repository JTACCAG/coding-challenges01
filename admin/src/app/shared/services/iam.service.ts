import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { IResponseApi } from '../interfaces/response-api';
import { UserData } from '../interfaces/user-data';

@Injectable({
  providedIn: 'root',
})
export class IamService {
  url = environment.apiUrl;

  constructor(private http: HttpClient) {}

  login(email: string, password: string) {
    return this.http.post<IResponseApi<{ accessToken: string; user: UserData }>>(
      `${this.url}/auth/login`,
      {
        email,
        password,
      },
    );
  }

  profile() {
    return this.http.get<IResponseApi<UserData>>(`${this.url}/iam/profile`);
  }
}
