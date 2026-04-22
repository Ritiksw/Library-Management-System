import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoanService } from '../../../services/loan.service';
import { BookService } from '../../../services/book.service';
import { MemberService } from '../../../services/member.service';
import { Book } from '../../../models/book.model';
import { Member } from '../../../models/member.model';
import { CreateLoan } from '../../../models/loan.model';

@Component({
  selector: 'app-loan-checkout',
  templateUrl: './loan-checkout.component.html',
  styleUrls: ['./loan-checkout.component.css']
})
export class LoanCheckoutComponent implements OnInit {
  books: Book[] = [];
  members: Member[] = [];
  errorMessage = '';

  model: CreateLoan = {
    bookId: 0,
    memberId: 0,
    loanDays: 14
  };

  constructor(
    private loanService: LoanService,
    private bookService: BookService,
    private memberService: MemberService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.bookService.getAll().subscribe(data => this.books = data.filter(b => b.availableCopies > 0));
    this.memberService.getAll().subscribe(data => this.members = data.filter(m => m.isActive));
  }

  checkout(): void {
    this.errorMessage = '';
    this.loanService.checkout(this.model).subscribe({
      next: () => this.router.navigate(['/loans']),
      error: (err) => this.errorMessage = err.error?.message || 'An error occurred.'
    });
  }
}
