import { Component, OnInit } from '@angular/core';
import { LoanService } from '../../../services/loan.service';
import { LoanHistory } from '../../../models/loan.model';

@Component({
  selector: 'app-loan-history',
  templateUrl: './loan-history.component.html',
  styleUrls: ['./loan-history.component.css']
})
export class LoanHistoryComponent implements OnInit {
  history: LoanHistory[] = [];
  search = '';

  constructor(private loanService: LoanService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loanService.getHistory(this.search || undefined).subscribe(data => this.history = data);
  }

  onSearch(): void {
    this.load();
  }
}
