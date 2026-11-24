using LibraryEngine;

namespace LibraryApp;

public static class SampleData
{
    public static void Load(BookCatalog books, MemberRegistry members)
    {
        books.Add("9780132350884", "Clean Code", "Robert C. Martin", 2);
        books.Add("9780135957059", "The Pragmatic Programmer", "Andrew Hunt", 1);
        books.Add("9780321125217", "Domain-Driven Design", "Eric Evans", 1);

        members.Register(MemberType.Student, "Ama Mensah", "ama@knust.edu.gh");
        members.Register(MemberType.Staff, "Dr Kofi Boateng", "kofi@knust.edu.gh");
        members.Register(MemberType.Guest, "Yaw Owusu", "yaw@example.com");
    }
}
