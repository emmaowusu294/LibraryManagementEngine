using LibraryEngine;

namespace LibraryApp;

/// <summary>Today's date plus a number of skipped days. Used to demo overdue books.</summary>
public class OffsetClock : IClock
{
    private int _skippedDays;

    public DateOnly Today => DateOnly.FromDateTime(DateTime.Today).AddDays(_skippedDays);

    public void Advance(int days)
    {
        if (days < 1)
        {
            throw new LibraryException("Days must be 1 or more.");
        }

        _skippedDays += days;
    }
}
