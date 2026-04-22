import { Component, OnInit } from '@angular/core';
import { BookService } from '../../../services/book.service';
import { Book } from '../../../models/book.model';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent implements OnInit {
  books: Book[] = [];
  search = '';

  constructor(private bookService: BookService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.bookService.getAll(this.search || undefined).subscribe(data => this.books = data);
  }

  onSearch(): void {
    this.load();
  }

  deleteBook(id: number): void {
    if (confirm('Are you sure you want to delete this book?')) {
      this.bookService.delete(id).subscribe({
        next: () => this.load(),
        error: (err) => alert(err.error?.message || 'Cannot delete this book.')
      });
    }
  }
}
