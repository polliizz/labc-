using System;
using System.Collections.Generic;

namespace InterfacesDemo
{
    // ===== АБСТРАКТНИЙ БАЗОВИЙ КЛАС =====
    abstract class LibraryItem : ICloneable, IComparable<LibraryItem>
    {
        public string Title { get; set; }
        public int Year { get; set; }

        public abstract void ShowInfo();

        // ===== IComparable =====
        public int CompareTo(LibraryItem other)
        {
            if (other == null)
                return 1;

            return Year.CompareTo(other.Year);
        }

        // ===== ICloneable =====
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    // ===== КЛАС-НАЩАДОК =====
    class Book : LibraryItem
    {
        public string Author { get; set; }

        public override void ShowInfo()
        {
            Console.WriteLine("Книга: " + Title +
                              ", Автор: " + Author +
                              ", Рік: " + Year);
        }

        public override object Clone()
        {
            return new Book
            {
                Title = this.Title,
                Author = this.Author,
                Year = this.Year
            };
        }
    }

    // ===== ЩЕ ОДИН КЛАС =====
    class Magazine : LibraryItem
    {
        public int IssueNumber { get; set; }

        public override void ShowInfo()
        {
            Console.WriteLine("Журнал: " + Title +
                              ", Номер: " + IssueNumber +
                              ", Рік: " + Year);
        }
    }

    // ===== ТОЧКА ВХОДУ =====
    class Program
    {
        static void Main(string[] args)
        {
            List<LibraryItem> items = new List<LibraryItem>
            {
                new Book
                {
                    Title = "Кобзар",
                    Author = "Тарас Шевченко",
                    Year = 1840
                },
                new Magazine
                {
                    Title = "Наука",
                    IssueNumber = 5,
                    Year = 2023
                },
                new Book
                {
                    Title = "C# для початківців",
                    Author = "Іван Іванов",
                    Year = 2020
                }
            };

            Console.WriteLine("=== ДО СОРТУВАННЯ ===");
            foreach (var item in items)
                item.ShowInfo();

            // ===== СОРТУВАННЯ =====
            items.Sort();

            Console.WriteLine("\n=== ПІСЛЯ СОРТУВАННЯ (за роком) ===");
            foreach (var item in items)
                item.ShowInfo();

            // ===== КЛОНУВАННЯ =====
            Console.WriteLine("\n=== КЛОНУВАННЯ ===");
            Book originalBook = new Book
            {
                Title = "Оригінал",
                Author = "Автор",
                Year = 2024
            };

            Book clonedBook = (Book)originalBook.Clone();
            clonedBook.Title = "Клон";

            originalBook.ShowInfo();
            clonedBook.ShowInfo();
        }
    }
}
