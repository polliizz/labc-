using System;
using System.Collections.Generic;

namespace PZ2
{
    // ===== АБСТРАКТНИЙ БАЗОВИЙ КЛАС =====
    abstract class LibraryItem
    {
        public string Title { get; set; }
        public int Year { get; set; }

        // Абстрактний метод
        public abstract void ShowInfo();

        // Віртуальний метод
        public virtual void GetItemType()
        {
            Console.WriteLine("Це елемент бібліотеки");
        }
    }

    // ===== КЛАС-НАЩАДОК =====
    class Book : LibraryItem
    {
        public string Author { get; set; }

        public override void ShowInfo()
        {
            Console.WriteLine(
                "Книга: " + Title +
                ", Автор: " + Author +
                ", Рік: " + Year
            );
        }

        public override void GetItemType()
        {
            Console.WriteLine("Тип: Книга");
        }
    }

    // ===== КЛАС-НАЩАДОК =====
    class Magazine : LibraryItem
    {
        public int IssueNumber { get; set; }

        public override void ShowInfo()
        {
            Console.WriteLine(
                "Журнал: " + Title +
                ", Номер: " + IssueNumber +
                ", Рік: " + Year
            );
        }

        public override void GetItemType()
        {
            Console.WriteLine("Тип: Журнал");
        }
    }

    // ===== ГОЛОВНИЙ КЛАС =====
    class Program
    {
        static void Main(string[] args)
        {
            List<LibraryItem> libraryItems = new List<LibraryItem>();

            Book book = new Book
            {
                Title = "Кобзар",
                Author = "Тарас Шевченко",
                Year = 1840
            };

            Magazine magazine = new Magazine
            {
                Title = "Наука і життя",
                IssueNumber = 5,
                Year = 2023
            };

            libraryItems.Add(book);
            libraryItems.Add(magazine);

            foreach (LibraryItem item in libraryItems)
            {
                item.GetItemType();
                item.ShowInfo();
                Console.WriteLine();
            }
        }
    }
}
