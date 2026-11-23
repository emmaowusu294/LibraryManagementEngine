namespace LibraryEngine;

internal static class MemberFactory
{
    public static Member Create(MemberType type, int id, string name, string email) => type switch
    {
        MemberType.Student => new StudentMember(id, name, email),
        MemberType.Staff => new StaffMember(id, name, email),
        MemberType.Guest => new GuestMember(id, name, email),
        _ => throw new LibraryException("Unknown member type.")
    };
}
