namespace LibraryEngine;

public sealed record StudentPolicy : IBorrowingPolicy
{
    public int MaxActiveLoans => 3;
    public int LoanPeriodDays => 14;
    public decimal FinePerDay => 0.50m;
}

public sealed record StaffPolicy : IBorrowingPolicy
{
    public int MaxActiveLoans => 10;
    public int LoanPeriodDays => 30;
    public decimal FinePerDay => 0.25m;
}

public sealed record GuestPolicy : IBorrowingPolicy
{
    public int MaxActiveLoans => 1;
    public int LoanPeriodDays => 7;
    public decimal FinePerDay => 1.00m;
}
