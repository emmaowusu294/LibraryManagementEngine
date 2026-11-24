using Xunit;

namespace LibraryEngine.Tests;

public class MemberRegistryTests
{
    private readonly MemberRegistry _members = new();

    [Fact]
    public void Register_WithInvalidEmail_Throws()
    {
        Assert.Throws<LibraryException>(() => _members.Register(MemberType.Student, "Ama", "not-an-email"));
    }

    [Fact]
    public void Register_WithDuplicateEmail_Throws()
    {
        _members.Register(MemberType.Student, "Ama", "ama@example.com");

        Assert.Throws<LibraryException>(() => _members.Register(MemberType.Staff, "Ama Two", "AMA@example.com"));
    }

    [Theory]
    [InlineData(MemberType.Student, 3, 14)]
    [InlineData(MemberType.Staff, 10, 30)]
    [InlineData(MemberType.Guest, 1, 7)]
    public void Register_AssignsPolicyForMemberType(MemberType type, int maxLoans, int loanDays)
    {
        var member = _members.Register(type, "Test", "test@example.com");

        Assert.Equal(maxLoans, member.Policy.MaxActiveLoans);
        Assert.Equal(loanDays, member.Policy.LoanPeriodDays);
    }
}
