namespace LibraryEngine;

public class SystemClock : IClock
{
    public DateOnly Today => DateOnly.FromDateTime(DateTime.Today);
}
