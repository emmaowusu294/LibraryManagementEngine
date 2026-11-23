namespace LibraryEngine;

///Gives the current date. Lets tests and demos control time.
public interface IClock
{
    DateOnly Today { get; }
}
