using System;

namespace PZ1
{
    // ===== КЛАС-МОДЕЛЬ =====
    class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int YearOfPublication { get; set; }
    }

    // ===== БАЗОВИЙ КЛАС (УСПАДКУВАННЯ) =====
    class LibraryServiceBase
    {
        protected int BooksCount;
    }

    // ===== КЛАС-СЕРВІС =====
    class LibraryService : LibraryServiceBase
    {
        private Book[] BooksStorage = new Book[10];

        public void AddBook(Book newBook)
        {
            BooksStorage[BooksCount] = newBook;
            BooksCount++;
        }

        public void ShowAllBooks()
        {
            for (int i = 0; i < BooksCount; i++)
            {
                Console.WriteLine(
                    BooksStorage[i].Title + " - " +
                    BooksStorage[i].Author + " (" +
                    BooksStorage[i].YearOfPublication + ")"
                );
            }
        }
    }

    // ===== ГОЛОВНИЙ КЛАС =====
    class Program
    {
        static void Main(string[] args)
        {
            Book firstBook = new Book
            {
                Title = "Кобзар",
                Author = "Тарас Шевченко",
                YearOfPublication = 1840
            };

            LibraryService libraryService = new LibraryService();
            libraryService.AddBook(firstBook);
            libraryService.ShowAllBooks();
        }
    }
}
