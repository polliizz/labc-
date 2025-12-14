using System;
using System.Collections.Generic;

namespace PZ2_Extended
{
    // ===== АБСТРАКТНИЙ БАЗОВИЙ КЛАС =====
    abstract class LibraryItem
    {
        public string Title { get; set; }
        public int Year { get; set; }

        // Абстрактний метод
        public abstract void ShowInfo();

        // Віртуальний метод (базова поведінка)
        public virtual void Borrow()
        {
            Console.WriteLine("Елемент бібліотеки видано.");
        }

        // ДОДАТКОВИЙ ВІРТУАЛЬНИЙ МЕТОД
        public virtual void ReturnItem()
        {
            Console.WriteLine("Елемент бібліотеки повернено.");
        }
    }

    // ===== КЛАС-НАЩАДОК =====
    class Book : LibraryItem
    {
        public string Author { get; set; }

        public override void ShowInfo()
        {
            Console.WriteLine("Книга: " + Title + ", Автор: " + Author + ", Рік: " + Year);
        }

        public override void Borrow()
        {
            Console.WriteLine("Книгу \"" + Title + "\" видано читачу.");
        }
    }

    // ===== КЛАС-НАЩАДОК =====
    class Magazine : LibraryItem
    {
        public int IssueNumber { get; set; }

        public override void ShowInfo()
        {
            Console.WriteLine("Журнал: " + Title + ", Номер: " + IssueNumber + ", Рік: " + Year);
        }

        public override void Borrow()
        {
            Console.WriteLine("Журнал \"" + Title + "\" можна читати тільки в залі.");
        }
    }

    // ===== НОВИЙ КЛАС-НАЩАДОК =====
    class EBook : LibraryItem
    {
        public double FileSizeMb { get; set; }

        public override void ShowInfo()
        {
            Console.WriteLine("Електронна книга: " + Title + ", Рік: " + Year +
                              ", Розмір: " + FileSizeMb + " MB");
        }

        // ПЕРЕВИЗНАЧЕННЯ З ВИКЛЮЧЕННЯМ
        public override void Borrow()
        {
            try
            {
                if (FileSizeMb <= 0)
                {
                    throw new Exception("Некоректний розмір файлу.");
                }

                Console.WriteLine("Електронну книгу \"" + Title + "\" завантажено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при завантаженні: " + ex.Message);
            }
        }

        public override void ReturnItem()
        {
            Console.WriteLine("Електронну книгу \"" + Title + "\" видалено з пристрою.");
        }
    }

    // ===== ТОЧКА ВХОДУ =====
    class Program
    {
        static void Main(string[] args)
        {
            List<LibraryItem> libraryItems = new List<LibraryItem>();

            libraryItems.Add(new Book
            {
                Title = "Кобзар",
                Author = "Тарас Шевченко",
                Year = 1840
            });

            libraryItems.Add(new Magazine
            {
                Title = "Наука і життя",
                IssueNumber = 5,
                Year = 2023
            });

            libraryItems.Add(new EBook
            {
                Title = "C# для початківців",
                Year = 2024,
                FileSizeMb = -10 // помилкові дані для демонстрації виключення
            });

            foreach (LibraryItem item in libraryItems)
            {
                item.ShowInfo();
                item.Borrow();
                item.ReturnItem();
                Console.WriteLine();
            }
        }
    }
}
