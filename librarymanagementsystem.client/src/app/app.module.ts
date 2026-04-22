import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './components/shared/navbar/navbar.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { BookListComponent } from './components/books/book-list/book-list.component';
import { BookFormComponent } from './components/books/book-form/book-form.component';
import { MemberListComponent } from './components/members/member-list/member-list.component';
import { MemberFormComponent } from './components/members/member-form/member-form.component';
import { LoanListComponent } from './components/loans/loan-list/loan-list.component';
import { LoanCheckoutComponent } from './components/loans/loan-checkout/loan-checkout.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    DashboardComponent,
    BookListComponent,
    BookFormComponent,
    MemberListComponent,
    MemberFormComponent,
    LoanListComponent,
    LoanCheckoutComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
