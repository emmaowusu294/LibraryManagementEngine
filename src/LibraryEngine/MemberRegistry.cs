namespace LibraryEngine;

public class MemberRegistry
{
    private readonly Dictionary<int, Member> _members = new();
    private int _nextId = 1;

    public Member Register(MemberType type, string name, string email)
    {
        var member = MemberFactory.Create(type, _nextId, name, email);

        if (_members.Values.Any(m => m.Email.Equals(member.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new LibraryException($"A member with email {member.Email} already exists.");
        }

        _members.Add(member.Id, member);
        _nextId++;
        return member;
    }

    public Member Get(int id) =>
        _members.TryGetValue(id, out var member)
            ? member
            : throw new LibraryException($"No member with ID {id}.");

    public IReadOnlyList<Member> All() => _members.Values.OrderBy(m => m.Id).ToList();
}
