# API Reference

Base URL: `https://localhost:7206`

All endpoints return JSON. Error responses follow the format:

```json
{
  "message": "Description of the error."
}
```

---

## Books

### List all books

```
GET /api/books?search={query}
```

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `search` | string | No | Filter by title, author, ISBN, or genre |

**Response** `200 OK`

```json
[
  {
    "id": 1,
    "title": "The Great Gatsby",
    "author": "F. Scott Fitzgerald",
    "isbn": "978-0743273565",
    "genre": "Fiction",
    "publishedYear": 1925,
    "totalCopies": 3,
    "availableCopies": 3
  }
]
```

### Get a book by ID

```
GET /api/books/{id}
```

**Response** `200 OK` — Book object (same shape as above)

**Response** `404 Not Found` — Book does not exist

### Create a book

```
POST /api/books
```

**Request body**

```json
{
  "title": "Clean Code",
  "author": "Robert C. Martin",
  "isbn": "978-0132350884",
  "genre": "Technology",
  "publishedYear": 2008,
  "totalCopies": 2
}
```

| Field | Type | Required | Constraints |
|-------|------|----------|-------------|
| `title` | string | Yes | Max 200 characters |
| `author` | string | Yes | Max 150 characters |
| `isbn` | string | Yes | Max 20 characters, must be unique |
| `genre` | string | No | Max 100 characters |
| `publishedYear` | int | No | |
| `totalCopies` | int | No | 1–100, defaults to 1 |

**Response** `201 Created` — Created book object

**Response** `409 Conflict` — ISBN already exists

### Update a book

```
PUT /api/books/{id}
```

**Request body** — Same as create.

**Response** `204 No Content` — Updated successfully

**Response** `404 Not Found` — Book does not exist

**Response** `409 Conflict` — ISBN already exists (on another book)

**Response** `400 Bad Request` — Cannot reduce copies below currently loaned count

### Delete a book

```
DELETE /api/books/{id}
```

**Response** `204 No Content` — Deleted successfully

**Response** `404 Not Found` — Book does not exist

**Response** `400 Bad Request` — Book has active loans

---

## Members

### List all members

```
GET /api/members?search={query}
```

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `search` | string | No | Filter by name or email |

**Response** `200 OK`

```json
[
  {
    "id": 1,
    "fullName": "Alice Johnson",
    "email": "alice@example.com",
    "phone": "555-0101",
    "membershipDate": "2024-01-15T00:00:00Z",
    "isActive": true,
    "activeLoans": 1
  }
]
```

### Get a member by ID

```
GET /api/members/{id}
```

**Response** `200 OK` — Member object (same shape as above)

**Response** `404 Not Found` — Member does not exist

### Create a member

```
POST /api/members
```

**Request body**

```json
{
  "fullName": "Alice Johnson",
  "email": "alice@example.com",
  "phone": "555-0101"
}
```

| Field | Type | Required | Constraints |
|-------|------|----------|-------------|
| `fullName` | string | Yes | Max 100 characters |
| `email` | string | Yes | Max 200 characters, valid email, must be unique |
| `phone` | string | No | Max 20 characters |

**Response** `201 Created` — Created member object

**Response** `409 Conflict` — Email already exists

### Update a member

```
PUT /api/members/{id}
```

**Request body** — Same as create.

**Response** `204 No Content` — Updated successfully

**Response** `404 Not Found` — Member does not exist

**Response** `409 Conflict` — Email already exists (on another member)

### Delete a member

```
DELETE /api/members/{id}
```

**Response** `204 No Content` — Deleted successfully

**Response** `404 Not Found` — Member does not exist

**Response** `400 Bad Request` — Member has active loans

---

## Loans

### List all loans

```
GET /api/loans?activeOnly={true|false}
```

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `activeOnly` | bool | No | If `true`, only returns loans that haven't been returned |

**Response** `200 OK`

```json
[
  {
    "id": 1,
    "bookId": 3,
    "bookTitle": "1984",
    "memberId": 1,
    "memberName": "Alice Johnson",
    "borrowDate": "2026-04-20T10:00:00Z",
    "dueDate": "2026-05-04T10:00:00Z",
    "returnDate": null,
    "isReturned": false,
    "isOverdue": false
  }
]
```

### Checkout a book (create a loan)

```
POST /api/loans
```

**Request body**

```json
{
  "bookId": 3,
  "memberId": 1,
  "loanDays": 14
}
```

| Field | Type | Required | Constraints |
|-------|------|----------|-------------|
| `bookId` | int | Yes | Must reference an existing book |
| `memberId` | int | Yes | Must reference an existing, active member |
| `loanDays` | int | No | 1–90, defaults to 14 |

**Response** `200 OK` — Created loan object

**Response** `404 Not Found` — Book or member does not exist

**Response** `400 Bad Request` — Possible reasons:
- Member account is not active
- No copies available for this book
- Member already has an active loan for this book

### Return a book

```
POST /api/loans/{id}/return
```

**Response** `200 OK`

```json
{
  "message": "Book returned successfully."
}
```

**Response** `404 Not Found` — Loan does not exist

**Response** `400 Bad Request` — Loan has already been returned

### Dashboard

```
GET /api/loans/dashboard
```

**Response** `200 OK`

```json
{
  "totalBooks": 23,
  "totalMembers": 4,
  "activeLoans": 2,
  "overdueLoans": 0,
  "recentLoans": [
    {
      "id": 1,
      "bookId": 3,
      "bookTitle": "1984",
      "memberId": 1,
      "memberName": "Alice Johnson",
      "borrowDate": "2026-04-20T10:00:00Z",
      "dueDate": "2026-05-04T10:00:00Z",
      "returnDate": null,
      "isReturned": false,
      "isOverdue": false
    }
  ]
}
```

---

## OpenAPI

When running in development mode, the OpenAPI spec is available at:

```
GET /openapi/v1.json
```
