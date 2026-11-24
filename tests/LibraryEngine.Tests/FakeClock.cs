using LibraryEngine;

namespace LibraryEngine.Tests;

public class FakeClock : IClock
{
    public FakeClock(DateOnly today) => Today = today;

    public DateOnly Today { get; set; }
}
