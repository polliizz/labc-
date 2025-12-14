using System;
using System.Collections.Generic;

namespace EventsAndDelegates
{
    // ===== АБСТРАКТНИЙ БАЗОВИЙ КЛАС =====
    abstract class LibraryItem
    {
        public string Title { get; set; }
        public int Year { get; set; }

        public abstract void ShowInfo();
    }

    // ===== КЛАС-НАЩАДОК =====
    class Book : LibraryItem
    {
        public override void ShowInfo()
        {
            Console.WriteLine("Книга: " + Title + ", Рік: " + Year);
        }
    }

    // ===== ДЕЛЕГАТ ДЛЯ ПОДІЙ =====
    delegate void LibraryItemHandler(LibraryItem item);

    // ===== КЛАС, ЩО ГЕНЕРУЄ ПОДІЇ =====
    class LibraryService
    {
        private List<LibraryItem> items = new List<LibraryItem>();

        // Події
        public event LibraryItemHandler ItemAdded;
        public event LibraryItemHandler ItemRemoved;

        public void AddItem(LibraryItem item)
        {
            try
            {
                items.Add(item);
                ItemAdded?.Invoke(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка додавання: " + ex.Message);
            }
        }

        public void RemoveItem(LibraryItem item)
        {
            try
            {
                items.Remove(item);
                ItemRemoved?.Invoke(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка видалення: " + ex.Message);
            }
        }

        // Метод з делегатом (із попередньої роботи)
        public void ProcessItems(Action<LibraryItem> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }
    }

    // ===== КЛАС, ЯКИЙ РЕАГУЄ НА ПОДІЇ =====
    class LibraryLogger
    {
        public void OnItemAdded(LibraryItem item)
        {
            try
            {
                Console.WriteLine("Подія: додано -> " + item.Title);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка обробки події додавання: " + ex.Message);
            }
        }

        public void OnItemRemoved(LibraryItem item)
        {
            try
            {
                Console.WriteLine("Подія: видалено -> " + item.Title);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка обробки події видалення: " + ex.Message);
            }
        }
    }

    // ===== ТОЧКА ВХОДУ =====
    class Program
    {
        static void Main(string[] args)
        {
            LibraryService libraryService = new LibraryService();
            LibraryLogger logger = new LibraryLogger();

            // ПІДПИСКА НА ПОДІЇ
            libraryService.ItemAdded += logger.OnItemAdded;
            libraryService.ItemRemoved += logger.OnItemRemoved;

            Book book1 = new Book
            {
                Title = "Кобзар",
                Year = 1840
            };

            // Події
            libraryService.AddItem(book1);
            libraryService.RemoveItem(book1);

            Console.WriteLine();

            // ВЗАЄМОДІЯ З ДЕЛЕГАТАМИ
            Action<LibraryItem> showInfoAction = item => item.ShowInfo();
            libraryService.AddItem(book1);
            libraryService.ProcessItems(showInfoAction);
        }
    }
}
