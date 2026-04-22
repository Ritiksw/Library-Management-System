import { Component, OnInit } from '@angular/core';
import { LoanService } from '../../../services/loan.service';
import { Loan } from '../../../models/loan.model';

@Component({
  selector: 'app-loan-list',
  templateUrl: './loan-list.component.html',
  styleUrls: ['./loan-list.component.css']
})
export class LoanListComponent implements OnInit {
  loans: Loan[] = [];
  activeOnly = false;
  search = '';

  constructor(private loanService: LoanService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loanService.getAll(this.activeOnly || undefined, this.search || undefined).subscribe(data => this.loans = data);
  }

  onSearch(): void {
    this.load();
  }

  toggleFilter(): void {
    this.activeOnly = !this.activeOnly;
    this.load();
  }

  returnBook(loanId: number): void {
    if (confirm('Confirm return of this book?')) {
      this.loanService.return(loanId).subscribe({
        next: () => this.load(),
        error: (err) => alert(err.error?.message || 'Error returning book.')
      });
    }
  }
}
