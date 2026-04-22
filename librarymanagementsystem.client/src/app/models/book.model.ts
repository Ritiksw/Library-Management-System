export interface Book {
  id: number;
  title: string;
  author: string;
  isbn: string;
  genre: string;
  publishedYear: number;
  totalCopies: number;
  availableCopies: number;
}

export interface CreateBook {
  title: string;
  author: string;
  isbn: string;
  genre: string;
  publishedYear: number;
  totalCopies: number;
}
