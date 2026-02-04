import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { IReport } from '../interfaces/report';
import { IResponseApi } from '../interfaces/response-api';

@Injectable({
  providedIn: 'root',
})
export class ReportService {
  url = environment.apiUrl;

  constructor(private http: HttpClient) {}

  created(report: IReport) {
    return this.http.post<IResponseApi<IReport>>(`${this.url}/report`, report);
  }

  findAll() {
    return this.http.get<IResponseApi<IReport[]>>(`${this.url}/report`);
  }
}
