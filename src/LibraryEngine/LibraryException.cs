namespace LibraryEngine;

///Raised when input is invalid or a library rule is broken.
public class LibraryException : Exception
{
    public LibraryException(string message) : base(message)
    {
    }
}
