using LibraryApp;
using LibraryEngine;

var clock = new OffsetClock();
var books = new BookCatalog();
var members = new MemberRegistry();
var library = new LibraryService(books, members, clock);

SampleData.Load(books, members);

new LibraryMenu(library, books, members, clock).Run();
