import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Loan, CreateLoan, Dashboard } from '../models/loan.model';

@Injectable({ providedIn: 'root' })
export class LoanService {
  private url = '/api/loans';

  constructor(private http: HttpClient) {}

  getAll(activeOnly?: boolean): Observable<Loan[]> {
    let params = new HttpParams();
    if (activeOnly !== undefined) params = params.set('activeOnly', activeOnly);
    return this.http.get<Loan[]>(this.url, { params });
  }

  checkout(loan: CreateLoan): Observable<Loan> {
    return this.http.post<Loan>(this.url, loan);
  }

  return(id: number): Observable<any> {
    return this.http.post(`${this.url}/${id}/return`, {});
  }

  getDashboard(): Observable<Dashboard> {
    return this.http.get<Dashboard>(`${this.url}/dashboard`);
  }
}
