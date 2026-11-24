# Library Management Engine

A console library system in C#. It handles books, members, borrowing, returns, due dates, fines, different borrowing rules per member type, and input validation.

## Setup

Requires the .NET 10 SDK.

```
git clone <repository-url>
cd LibraryManagementEngine
dotnet build
```

## Run

```
dotnet run --project src/LibraryApp
```

The app starts with sample books and members. Use option 10 to skip days forward and see overdue rules and fines.

## Test

```
dotnet test
```

## Projects

| Project | Purpose |
| --- | --- |
| `src/LibraryEngine` | All library rules. No console code. |
| `src/LibraryApp` | Console menu that calls the engine. |
| `tests/LibraryEngine.Tests` | xUnit tests for the engine. |

## Borrowing rules

| Member type | Max books | Loan period | Fine per late day |
| --- | --- | --- | --- |
| Student | 3 | 14 days | 0.50 |
| Staff | 10 | 30 days | 0.25 |
| Guest | 1 | 7 days | 1.00 |

A member cannot borrow if they have an overdue book, are at their limit, already hold the same book, or no copy is available.

## Class design

Each class has one job.

| Class | Responsibility |
| --- | --- |
| `Book` | Holds book details and its copy count. Keeps its own counts valid. |
| `Member` (abstract) | Common member data. Each subclass supplies its policy. |
| `StudentMember`, `StaffMember`, `GuestMember` | Inherit `Member` and return their own `IBorrowingPolicy`. |
| `IBorrowingPolicy` | Interface for the three rules: max loans, loan days, fine per day. |
| `StudentPolicy`, `StaffPolicy`, `GuestPolicy` | Records that implement the policy interface. |
| `MemberType` | Enum used to choose which member to create. |
| `MemberFactory` | Creates the right `Member` subclass from a `MemberType`. |
| `Loan` | One borrowing. Knows its due date, whether it is overdue, and the fine. |
| `ReturnReceipt` | Record returned when a book comes back (days late and fine). |
| `BookCatalog` | Stores and searches books. |
| `MemberRegistry` | Registers and finds members. Rejects duplicate emails. |
| `LibraryService` | Applies the borrowing and return rules using the classes above. |
| `IClock`, `SystemClock` | Supply today's date so time can be controlled in tests. |
| `Guard`, `LibraryException` | Shared validation and the single error type for broken rules. |

OOP ideas used:

- **Encapsulation:** `Book.AvailableCopies` can only change through `TakeCopy` and `ReturnCopy`. `Loan.ReturnedOn` can only be set by `MarkReturned`.
- **Abstraction:** `LibraryService` works with `Member` and `IBorrowingPolicy`, not with specific member types.
- **Inheritance:** three member types extend `Member`.
- **Polymorphism:** `member.Policy` gives different rules depending on the member type, with no `if` on type in the service.
- **Interfaces, enums, records:** `IBorrowingPolicy` and `IClock`, `MemberType`, and the policy and receipt records.
- **Nullability:** nullable reference types are enabled. Input is checked in `Guard` before it is stored.

Adding a new member type means adding one policy, one member subclass, and one line in `MemberFactory`. `LibraryService` does not change.

## Assumptions

- Dates are whole days (`DateOnly`). A book returned on its due date is not late.
- Fines are calculated at return time and are not stored as a balance.
- ISBN is the unique ID for a book. Email is unique per member.
- A member must return overdue books before borrowing again.

## Known limitations

- Data is in memory only. Everything resets when the app closes.
- No fine payment tracking.
- No renewals or reservations.
- No removal or editing of books and members.
