using System;

namespace Lab1
{
    // ===== БАЗОВИЙ КЛАС-МОДЕЛЬ =====
    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    // ===== НАЩАДОК КЛАСУ-МОДЕЛІ =====
    class Author : Person
    {
        public int BirthYear { get; set; }
    }

    // ===== КЛАС-МОДЕЛЬ =====
    class Book
    {
        public string Title { get; set; }
        public Author BookAuthor { get; set; }
        public int PublicationYear { get; set; }
    }

    // ===== БАЗОВИЙ КЛАС-СЕРВІС =====
    class BaseService
    {
        protected int ItemsCount;
    }

    // ===== КЛАС-СЕРВІС =====
    class LibraryService : BaseService
    {
        private Book[] Books = new Book[10];

        public void AddBook(Book book)
        {
            Books[ItemsCount] = book;
            ItemsCount++;
        }

        public void ShowBooks()
        {
            for (int i = 0; i < ItemsCount; i++)
            {
                Console.WriteLine(
                    Books[i].Title + " — " +
                    Books[i].BookAuthor.FirstName + " " +
                    Books[i].BookAuthor.LastName + ", " +
                    Books[i].PublicationYear
                );
            }
        }
    }

    // ===== ТОЧКА ВХОДУ =====
    class Program
    {
        static void Main(string[] args)
        {
            Author author = new Author
            {
                FirstName = "Тарас",
                LastName = "Шевченко",
                BirthYear = 1814
            };

            Book book = new Book
            {
                Title = "Кобзар",
                BookAuthor = author,
                PublicationYear = 1840
            };

            LibraryService libraryService = new LibraryService();
            libraryService.AddBook(book);
            libraryService.ShowBooks();
        }
    }
}
