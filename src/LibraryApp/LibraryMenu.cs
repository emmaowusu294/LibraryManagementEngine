using LibraryEngine;
using static LibraryApp.ConsoleInput;

namespace LibraryApp;

public class LibraryMenu
{
    private readonly LibraryService _library;
    private readonly BookCatalog _books;
    private readonly MemberRegistry _members;
    private readonly OffsetClock _clock;

    public LibraryMenu(LibraryService library, BookCatalog books, MemberRegistry members, OffsetClock clock)
    {
        _library = library;
        _books = books;
        _members = members;
        _clock = clock;
    }

    public void Run()
    {
        Console.WriteLine("Library Management Engine");

        while (true)
        {
            ShowOptions();
            var choice = Console.ReadLine()?.Trim();

            if (choice is null || choice == "0")
            {
                return;
            }

            try
            {
                Handle(choice);
            }
            catch (LibraryException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private static void ShowOptions()
    {
        Console.WriteLine();
        Console.WriteLine("1  Add book");
        Console.WriteLine("2  List books");
        Console.WriteLine("3  Search books");
        Console.WriteLine("4  Register member");
        Console.WriteLine("5  List members");
        Console.WriteLine("6  Borrow book");
        Console.WriteLine("7  Return book");
        Console.WriteLine("8  Show member's loans");
        Console.WriteLine("9  Show overdue loans");
        Console.WriteLine("10 Skip days forward (demo)");
        Console.WriteLine("0  Exit");
        Console.Write("Choose: ");
    }

    private void Handle(string choice)
    {
        switch (choice)
        {
            case "1": AddBook(); break;
            case "2": PrintBooks(_books.All()); break;
            case "3": PrintBooks(_books.Search(ReadText("Title or author"))); break;
            case "4": RegisterMember(); break;
            case "5": ListMembers(); break;
            case "6": Borrow(); break;
            case "7": Return(); break;
            case "8": PrintLoans(_library.ActiveLoansFor(ReadInt("Member ID"))); break;
            case "9": PrintLoans(_library.OverdueLoans()); break;
            case "10": SkipDays(); break;
            default: Console.WriteLine("Unknown option."); break;
        }
    }

    private void AddBook()
    {
        var book = _books.Add(ReadText("ISBN"), ReadText("Title"), ReadText("Author"), ReadInt("Copies"));
        Console.WriteLine($"Added \"{book.Title}\".");
    }

    private void RegisterMember()
    {
        var member = _members.Register(ReadMemberType(), ReadText("Name"), ReadText("Email"));
        Console.WriteLine($"Registered {member.Name} with ID {member.Id}.");
    }

    private void ListMembers()
    {
        foreach (var m in _members.All())
        {
            Console.WriteLine($"{m.Id} | {m.Name} | {m.Type} | {m.Email}");
        }
    }

    private void Borrow()
    {
        var loan = _library.Borrow(ReadInt("Member ID"), ReadText("ISBN"));
        Console.WriteLine($"{loan.Member.Name} borrowed \"{loan.Book.Title}\". Due {loan.DueDate:yyyy-MM-dd}.");
    }

    private void Return()
    {
        var receipt = _library.Return(ReadInt("Member ID"), ReadText("ISBN"));
        Console.WriteLine($"Returned \"{receipt.Loan.Book.Title}\".");

        if (receipt.DaysLate > 0)
        {
            Console.WriteLine($"{receipt.DaysLate} day(s) late. Fine: {receipt.Fine:0.00}");
        }
    }

    private void SkipDays()
    {
        _clock.Advance(ReadInt("Days to skip"));
        Console.WriteLine($"Today is now {_clock.Today:yyyy-MM-dd}.");
    }

    private static void PrintBooks(IReadOnlyList<Book> books)
    {
        if (books.Count == 0)
        {
            Console.WriteLine("No books found.");
            return;
        }

        foreach (var b in books)
        {
            Console.WriteLine($"{b.Isbn} | {b.Title} by {b.Author} | {b.AvailableCopies}/{b.TotalCopies} available");
        }
    }

    private static void PrintLoans(IReadOnlyList<Loan> loans)
    {
        if (loans.Count == 0)
        {
            Console.WriteLine("No loans.");
            return;
        }

        foreach (var l in loans)
        {
            Console.WriteLine($"Loan {l.Id} | {l.Book.Title} | {l.Member.Name} | due {l.DueDate:yyyy-MM-dd}");
        }
    }
}
