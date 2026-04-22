import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { BookListComponent } from './components/books/book-list/book-list.component';
import { BookFormComponent } from './components/books/book-form/book-form.component';
import { MemberListComponent } from './components/members/member-list/member-list.component';
import { MemberFormComponent } from './components/members/member-form/member-form.component';
import { LoanListComponent } from './components/loans/loan-list/loan-list.component';
import { LoanCheckoutComponent } from './components/loans/loan-checkout/loan-checkout.component';
import { LoanHistoryComponent } from './components/loans/loan-history/loan-history.component';

const routes: Routes = [
  { path: '', component: DashboardComponent },
  { path: 'books', component: BookListComponent },
  { path: 'books/new', component: BookFormComponent },
  { path: 'books/edit/:id', component: BookFormComponent },
  { path: 'members', component: MemberListComponent },
  { path: 'members/new', component: MemberFormComponent },
  { path: 'members/edit/:id', component: MemberFormComponent },
  { path: 'loans', component: LoanListComponent },
  { path: 'loans/history', component: LoanHistoryComponent },
  { path: 'loans/checkout', component: LoanCheckoutComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
