import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book, CreateBook } from '../models/book.model';

@Injectable({ providedIn: 'root' })
export class BookService {
  private url = '/api/books';

  constructor(private http: HttpClient) {}

  getAll(search?: string): Observable<Book[]> {
    let params = new HttpParams();
    if (search) params = params.set('search', search);
    return this.http.get<Book[]>(this.url, { params });
  }

  getById(id: number): Observable<Book> {
    return this.http.get<Book>(`${this.url}/${id}`);
  }

  create(book: CreateBook): Observable<Book> {
    return this.http.post<Book>(this.url, book);
  }

  update(id: number, book: CreateBook): Observable<void> {
    return this.http.put<void>(`${this.url}/${id}`, book);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
