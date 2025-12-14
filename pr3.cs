using System;
using System.Collections.Generic;

namespace DelegatesDemo
{
    // ===== АБСТРАКТНИЙ БАЗОВИЙ КЛАС =====
    abstract class LibraryItem
    {
        public string Title { get; set; }
        public int Year { get; set; }

        public abstract void ShowInfo();

        public virtual int GetValue()
        {
            return 1; // базове значення
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

        public override int GetValue()
        {
            return 2;
        }
    }

    // ===== КЛАС-НАЩАДОК =====
    class Magazine : LibraryItem
    {
        public override void ShowInfo()
        {
            Console.WriteLine("Журнал: " + Title + ", Рік: " + Year);
        }

        public override int GetValue()
        {
            return 1;
        }
    }

    // ===== ДЕЛЕГАТИ =====
    delegate void ItemAction(LibraryItem item);
    delegate bool ItemCondition(LibraryItem item);
    delegate int ItemCounter(LibraryItem item);

    // ===== СЕРВІС ДЛЯ РОБОТИ З КОЛЕКЦІЄЮ =====
    class LibraryService
    {
        public void ProcessItems(
            List<LibraryItem> items,
            ItemAction action,
            ItemCondition condition,
            ItemCounter counter)
        {
            int total = 0;

            foreach (LibraryItem item in items)
            {
                if (condition(item))
                {
                    action(item);
                    total += counter(item);
                }
            }

            Console.WriteLine("Загальне значення: " + total);
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
                    Year = 2023
                }
            };

            LibraryService service = new LibraryService();

            // Делегат для виведення інформації
            ItemAction showInfoAction = item => item.ShowInfo();

            // Делегат для перевірки умови
            ItemCondition yearCondition = item => item.Year > 1900;

            // Делегат для підрахунку
            ItemCounter countValue = item => item.GetValue();

            service.ProcessItems(
                items,
                showInfoAction,
                yearCondition,
                countValue
            );

            Console.WriteLine();

            // ДРУГИЙ НАБІР ДЕЛЕГАТІВ
            ItemCondition allItemsCondition = item => true;
            ItemCounter oneForEach = item => 1;

            service.ProcessItems(
                items,
                showInfoAction,
                allItemsCondition,
                oneForEach
            );
        }
    }
}
