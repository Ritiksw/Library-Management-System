import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { BookService } from '../../../services/book.service';
import { CreateBook } from '../../../models/book.model';

@Component({
  selector: 'app-book-form',
  templateUrl: './book-form.component.html',
  styleUrls: ['./book-form.component.css']
})
export class BookFormComponent implements OnInit {
  isEdit = false;
  bookId = 0;
  errorMessage = '';

  model: CreateBook = {
    title: '',
    author: '',
    isbn: '',
    genre: '',
    publishedYear: new Date().getFullYear(),
    totalCopies: 1
  };

  constructor(
    private bookService: BookService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.bookId = +id;
      this.bookService.getById(this.bookId).subscribe(book => {
        this.model = {
          title: book.title,
          author: book.author,
          isbn: book.isbn,
          genre: book.genre,
          publishedYear: book.publishedYear,
          totalCopies: book.totalCopies
        };
      });
    }
  }

  save(): void {
    this.errorMessage = '';
    const action: Observable<any> = this.isEdit
      ? this.bookService.update(this.bookId, this.model)
      : this.bookService.create(this.model);

    action.subscribe({
      next: () => this.router.navigate(['/books']),
      error: (err: any) => this.errorMessage = err.error?.message || 'An error occurred.'
    });
  }
}
