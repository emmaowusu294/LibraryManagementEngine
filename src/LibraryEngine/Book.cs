namespace LibraryEngine;

public class Book
{
    public Book(string isbn, string title, string author, int copies)
    {
        Isbn = Guard.NotBlank(isbn, "ISBN");
        Title = Guard.NotBlank(title, "Title");
        Author = Guard.NotBlank(author, "Author");

        if (copies < 1)
        {
            throw new LibraryException("A book needs at least one copy.");
        }

        TotalCopies = copies;
        AvailableCopies = copies;
    }

    public string Isbn { get; }
    public string Title { get; }
    public string Author { get; }
    public int TotalCopies { get; }
    public int AvailableCopies { get; private set; }
    public bool IsAvailable => AvailableCopies > 0;

    internal void TakeCopy()
    {
        if (!IsAvailable)
        {
            throw new LibraryException($"No copies of \"{Title}\" are available.");
        }

        AvailableCopies--;
    }

    internal void ReturnCopy()
    {
        if (AvailableCopies >= TotalCopies)
        {
            throw new LibraryException($"All copies of \"{Title}\" are already in the library.");
        }

        AvailableCopies++;
    }
}
