import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Member, CreateMember } from '../models/member.model';

@Injectable({ providedIn: 'root' })
export class MemberService {
  private url = '/api/members';

  constructor(private http: HttpClient) {}

  getAll(search?: string): Observable<Member[]> {
    let params = new HttpParams();
    if (search) params = params.set('search', search);
    return this.http.get<Member[]>(this.url, { params });
  }

  getById(id: number): Observable<Member> {
    return this.http.get<Member>(`${this.url}/${id}`);
  }

  create(member: CreateMember): Observable<Member> {
    return this.http.post<Member>(this.url, member);
  }

  update(id: number, member: CreateMember): Observable<void> {
    return this.http.put<void>(`${this.url}/${id}`, member);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
