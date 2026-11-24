using LibraryEngine;

namespace LibraryApp;

public static class ConsoleInput
{
    public static string ReadText(string prompt)
    {
        Console.Write($"{prompt}: ");
        return Console.ReadLine() ?? "";
    }

    public static int ReadInt(string prompt)
    {
        while (true)
        {
            if (int.TryParse(ReadText(prompt), out var value))
            {
                return value;
            }

            Console.WriteLine("Please enter a whole number.");
        }
    }

    public static MemberType ReadMemberType()
    {
        while (true)
        {
            var text = ReadText("Type (student/staff/guest)");

            if (Enum.TryParse<MemberType>(text, true, out var type) && Enum.IsDefined(type))
            {
                return type;
            }

            Console.WriteLine("Please enter student, staff or guest.");
        }
    }
}
