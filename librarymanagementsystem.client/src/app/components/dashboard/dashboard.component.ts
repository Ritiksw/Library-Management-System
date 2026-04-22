import { Component, OnInit } from '@angular/core';
import { LoanService } from '../../services/loan.service';
import { Dashboard } from '../../models/loan.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  dashboard: Dashboard | null = null;

  constructor(private loanService: LoanService) {}

  ngOnInit(): void {
    this.loanService.getDashboard().subscribe(data => this.dashboard = data);
  }
}
