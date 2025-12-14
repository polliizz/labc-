using System;
using System.Collections.Generic;
using System.Linq;

namespace IteratorsAndLinq
{
    // ===== БАЗОВИЙ КЛАС =====
    abstract class LibraryItem : IComparable<LibraryItem>, ICloneable
    {
        public string Title { get; set; }
        public int Year { get; set; }

        public abstract void ShowInfo();

        public int CompareTo(LibraryItem other)
        {
            return Year.CompareTo(other.Year);
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
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
    }

    // ===== КЛАС-НАЩАДОК =====
    class Magazine : LibraryItem
    {
        public override void ShowInfo()
        {
            Console.WriteLine("Журнал: " + Title +
                              ", Рік: " + Year);
        }
    }

    // ===== СЕРВІС З ВЛАСНИМ ІТЕРАТОРОМ =====
    class LibraryService
    {
        private List<LibraryItem> items = new List<LibraryItem>();

        public void AddItem(LibraryItem item)
        {
            items.Add(item);
        }

        // ВЛАСНИЙ ІТЕРАТОР З yield
        public IEnumerable<LibraryItem> GetItemsAfterYear(int year)
        {
            foreach (var item in items)
            {
                if (item.Year > year)
                {
                    yield return item;
                }
            }
        }

        public List<LibraryItem> GetAllItems()
        {
            return items;
        }
    }

    // ===== ТОЧКА ВХОДУ =====
    class Program
    {
        static void Main(string[] args)
        {
            LibraryService library = new LibraryService();

            library.AddItem(new Book
            {
                Title = "Кобзар",
                Author = "Тарас Шевченко",
                Year = 1840
            });

            library.AddItem(new Book
            {
                Title = "C# для початківців",
                Author = "Іван Іванов",
                Year = 2020
            });

            library.AddItem(new Magazine
            {
                Title = "Наука",
                Year = 2023
            });

            Console.WriteLine("=== ІТЕРАТОР (після 2000 року) ===");
            foreach (var item in library.GetItemsAfterYear(2000))
            {
                item.ShowInfo();
            }

            var allItems = library.GetAllItems();

            // ===== LINQ: ФІЛЬТРАЦІЯ =====
            Console.WriteLine("\n=== LINQ: тільки книги ===");
            var books = allItems.OfType<Book>();
            foreach (var book in books)
                book.ShowInfo();

            // ===== LINQ: ВИБІРКА =====
            Console.WriteLine("\n=== LINQ: назви елементів ===");
            var titles = allItems.Select(i => i.Title);
            foreach (var title in titles)
                Console.WriteLine(title);

            // ===== LINQ: АГРЕГУВАННЯ =====
            int countAfter2000 = allItems.Count(i => i.Year > 2000);
            Console.WriteLine("\nКількість елементів після 2000 року: " + countAfter2000);

            int maxYear = allItems.Max(i => i.Year);
            Console.WriteLine("Найновіший рік: " + maxYear);
        }
    }
}
