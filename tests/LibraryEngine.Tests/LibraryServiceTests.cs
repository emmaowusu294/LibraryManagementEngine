using Xunit;
namespace LibraryEngine.Tests;

public class LibraryServiceTests
{
    private readonly FakeClock _clock = new(new DateOnly(2026, 11, 1));
    private readonly BookCatalog _books = new();
    private readonly MemberRegistry _members = new();
    private readonly LibraryService _library;

    public LibraryServiceTests()
    {
        _library = new LibraryService(_books, _members, _clock);
    }

    [Fact]
    public void Borrow_SetsDueDateFromMemberPolicy()
    {
        var student = _members.Register(MemberType.Student, "Ama", "ama@example.com");
        _books.Add("1", "Clean Code", "Martin", 1);

        var loan = _library.Borrow(student.Id, "1");

        Assert.Equal(new DateOnly(2026, 11, 15), loan.DueDate);
    }

    [Fact]
    public void Borrow_ReducesAvailableCopies()
    {
        var student = _members.Register(MemberType.Student, "Ama", "ama@example.com");
        var book = _books.Add("1", "Clean Code", "Martin", 2);

        _library.Borrow(student.Id, "1");

        Assert.Equal(1, book.AvailableCopies);
    }

    [Fact]
    public void Borrow_WhenNoCopiesLeft_Throws()
    {
        var first = _members.Register(MemberType.Student, "Ama", "ama@example.com");
        var second = _members.Register(MemberType.Student, "Kofi", "kofi@example.com");
        _books.Add("1", "Clean Code", "Martin", 1);
        _library.Borrow(first.Id, "1");

        Assert.Throws<LibraryException>(() => _library.Borrow(second.Id, "1"));
    }

    [Fact]
    public void Borrow_BeyondMemberLimit_Throws()
    {
        var guest = _members.Register(MemberType.Guest, "Yaw", "yaw@example.com");
        _books.Add("1", "Clean Code", "Martin", 1);
        _books.Add("2", "Refactoring", "Fowler", 1);
        _library.Borrow(guest.Id, "1");

        Assert.Throws<LibraryException>(() => _library.Borrow(guest.Id, "2"));
    }

    [Fact]
    public void Borrow_SameBookTwice_Throws()
    {
        var student = _members.Register(MemberType.Student, "Ama", "ama@example.com");
        _books.Add("1", "Clean Code", "Martin", 2);
        _library.Borrow(student.Id, "1");

        Assert.Throws<LibraryException>(() => _library.Borrow(student.Id, "1"));
    }

    [Fact]
    public void Borrow_WithOverdueLoan_Throws()
    {
        var student = _members.Register(MemberType.Student, "Ama", "ama@example.com");
        _books.Add("1", "Clean Code", "Martin", 1);
        _books.Add("2", "Refactoring", "Fowler", 1);
        _library.Borrow(student.Id, "1");
        _clock.Today = new DateOnly(2026, 11, 20);

        Assert.Throws<LibraryException>(() => _library.Borrow(student.Id, "2"));
    }

    [Fact]
    public void Return_OnTime_HasNoFine()
    {
        var student = _members.Register(MemberType.Student, "Ama", "ama@example.com");
        _books.Add("1", "Clean Code", "Martin", 1);
        _library.Borrow(student.Id, "1");
        _clock.Today = new DateOnly(2026, 11, 15);

        var receipt = _library.Return(student.Id, "1");

        Assert.Equal(0, receipt.DaysLate);
        Assert.Equal(0m, receipt.Fine);
    }

    [Fact]
    public void Return_Late_ChargesFinePerDay()
    {
        var student = _members.Register(MemberType.Student, "Ama", "ama@example.com");
        _books.Add("1", "Clean Code", "Martin", 1);
        _library.Borrow(student.Id, "1");
        _clock.Today = new DateOnly(2026, 11, 18);

        var receipt = _library.Return(student.Id, "1");

        Assert.Equal(3, receipt.DaysLate);
        Assert.Equal(1.50m, receipt.Fine);
    }

    [Fact]
    public void Return_RestoresCopy()
    {
        var student = _members.Register(MemberType.Student, "Ama", "ama@example.com");
        var book = _books.Add("1", "Clean Code", "Martin", 1);
        _library.Borrow(student.Id, "1");

        _library.Return(student.Id, "1");

        Assert.Equal(book.TotalCopies, book.AvailableCopies);
    }

    [Fact]
    public void Return_WithoutLoan_Throws()
    {
        var student = _members.Register(MemberType.Student, "Ama", "ama@example.com");
        _books.Add("1", "Clean Code", "Martin", 1);

        Assert.Throws<LibraryException>(() => _library.Return(student.Id, "1"));
    }
}
