namespace LibraryEngine;

public class LibraryService
{
    private readonly BookCatalog _books;
    private readonly MemberRegistry _members;
    private readonly IClock _clock;
    private readonly List<Loan> _loans = new();
    private int _nextLoanId = 1;

    public LibraryService(BookCatalog books, MemberRegistry members, IClock clock)
    {
        _books = books;
        _members = members;
        _clock = clock;
    }

    public Loan Borrow(int memberId, string isbn)
    {
        var member = _members.Get(memberId);
        var book = _books.Get(isbn);
        var today = _clock.Today;
        var active = ActiveLoansFor(memberId);

        if (active.Any(l => l.IsOverdue(today)))
        {
            throw new LibraryException($"{member.Name} has an overdue book and cannot borrow until it is returned.");
        }

        if (active.Count >= member.Policy.MaxActiveLoans)
        {
            throw new LibraryException(
                $"{member.Name} has reached the limit of {member.Policy.MaxActiveLoans} book(s) for a {member.Type} member.");
        }

        if (active.Any(l => l.Book == book))
        {
            throw new LibraryException($"{member.Name} already has \"{book.Title}\".");
        }

        book.TakeCopy();

        var loan = new Loan(_nextLoanId++, book, member, today);
        _loans.Add(loan);
        return loan;
    }

    public ReturnReceipt Return(int memberId, string isbn)
    {
        _members.Get(memberId);
        var book = _books.Get(isbn);

        var loan = ActiveLoansFor(memberId).FirstOrDefault(l => l.Book == book)
                   ?? throw new LibraryException($"Member {memberId} does not have \"{book.Title}\".");

        var today = _clock.Today;
        loan.MarkReturned(today);
        book.ReturnCopy();

        return new ReturnReceipt(loan, loan.DaysLate(today), loan.FineAsOf(today));
    }

    public IReadOnlyList<Loan> ActiveLoansFor(int memberId) =>
        _loans.Where(l => l.IsActive && l.Member.Id == memberId).ToList();

    public IReadOnlyList<Loan> OverdueLoans()
    {
        var today = _clock.Today;
        return _loans.Where(l => l.IsOverdue(today)).ToList();
    }
}
