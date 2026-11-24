using Xunit;

namespace LibraryEngine.Tests;

public class BookTests
{
    [Fact]
    public void Constructor_WithZeroCopies_Throws()
    {
        Assert.Throws<LibraryException>(() => new Book("111", "Title", "Author", 0));
    }

    [Fact]
    public void Constructor_WithBlankTitle_Throws()
    {
        Assert.Throws<LibraryException>(() => new Book("111", "  ", "Author", 1));
    }
}
