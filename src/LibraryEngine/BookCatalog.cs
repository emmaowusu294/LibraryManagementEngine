namespace LibraryEngine;

public class BookCatalog
{
    private readonly Dictionary<string, Book> _books = new(StringComparer.OrdinalIgnoreCase);

    public Book Add(string isbn, string title, string author, int copies)
    {
        var book = new Book(isbn, title, author, copies);

        if (!_books.TryAdd(book.Isbn, book))
        {
            throw new LibraryException($"A book with ISBN {book.Isbn} already exists.");
        }

        return book;
    }

    public Book Get(string isbn)
    {
        var key = Guard.NotBlank(isbn, "ISBN");

        return _books.TryGetValue(key, out var book)
            ? book
            : throw new LibraryException($"No book with ISBN {key}.");
    }

    public IReadOnlyList<Book> All() => _books.Values.OrderBy(b => b.Title).ToList();

    public IReadOnlyList<Book> Search(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return All();
        }

        return All()
            .Where(b => b.Title.Contains(term, StringComparison.OrdinalIgnoreCase)
                     || b.Author.Contains(term, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
