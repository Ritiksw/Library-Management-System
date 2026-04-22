export interface Loan {
  id: number;
  bookId: number;
  bookTitle: string;
  memberId: number;
  memberName: string;
  borrowDate: string;
  dueDate: string;
  returnDate: string | null;
  isReturned: boolean;
  isOverdue: boolean;
}

export interface CreateLoan {
  bookId: number;
  memberId: number;
  loanDays: number;
}

export interface Dashboard {
  totalBooks: number;
  totalMembers: number;
  activeLoans: number;
  overdueLoans: number;
  recentLoans: Loan[];
}
