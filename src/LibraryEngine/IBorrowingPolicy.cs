namespace LibraryEngine;

///The borrowing rules that apply to a kind of member.
public interface IBorrowingPolicy
{
    int MaxActiveLoans { get; }
    int LoanPeriodDays { get; }
    decimal FinePerDay { get; }
}
