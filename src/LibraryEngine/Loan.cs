namespace LibraryEngine;

public class Loan
{
    public Loan(int id, Book book, Member member, DateOnly borrowedOn)
    {
        Id = id;
        Book = book;
        Member = member;
        BorrowedOn = borrowedOn;
        DueDate = borrowedOn.AddDays(member.Policy.LoanPeriodDays);
    }

    public int Id { get; }
    public Book Book { get; }
    public Member Member { get; }
    public DateOnly BorrowedOn { get; }
    public DateOnly DueDate { get; }
    public DateOnly? ReturnedOn { get; private set; }
    public bool IsActive => ReturnedOn is null;

    public bool IsOverdue(DateOnly today) => IsActive && today > DueDate;

    public int DaysLate(DateOnly asOf)
    {
        var end = ReturnedOn ?? asOf;
        return Math.Max(0, end.DayNumber - DueDate.DayNumber);
    }

    public decimal FineAsOf(DateOnly asOf) => DaysLate(asOf) * Member.Policy.FinePerDay;

    internal void MarkReturned(DateOnly date)
    {
        if (!IsActive)
        {
            throw new LibraryException("This loan has already been returned.");
        }

        ReturnedOn = date;
    }
}
