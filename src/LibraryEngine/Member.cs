namespace LibraryEngine;

public abstract class Member
{
    protected Member(int id, string name, string email)
    {
        Id = id;
        Name = Guard.NotBlank(name, "Name");
        Email = Guard.Email(email);
    }

    public int Id { get; }
    public string Name { get; }
    public string Email { get; }
    public abstract MemberType Type { get; }
    public abstract IBorrowingPolicy Policy { get; }
}

public class StudentMember : Member
{
    private static readonly IBorrowingPolicy Rules = new StudentPolicy();

    public StudentMember(int id, string name, string email) : base(id, name, email)
    {
    }

    public override MemberType Type => MemberType.Student;
    public override IBorrowingPolicy Policy => Rules;
}

public class StaffMember : Member
{
    private static readonly IBorrowingPolicy Rules = new StaffPolicy();

    public StaffMember(int id, string name, string email) : base(id, name, email)
    {
    }

    public override MemberType Type => MemberType.Staff;
    public override IBorrowingPolicy Policy => Rules;
}

public class GuestMember : Member
{
    private static readonly IBorrowingPolicy Rules = new GuestPolicy();

    public GuestMember(int id, string name, string email) : base(id, name, email)
    {
    }

    public override MemberType Type => MemberType.Guest;
    public override IBorrowingPolicy Policy => Rules;
}
