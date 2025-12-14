using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingDemo
{
    // ===== БАЗОВИЙ КЛАС =====
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
            Console.WriteLine(
                $"[Потік {Thread.CurrentThread.ManagedThreadId}] Книга: {Title}, {Year}"
            );
        }
    }

    // ===== СЕРВІС З СИНХРОНІЗАЦІЄЮ =====
    class LibraryService
    {
        private List<LibraryItem> items = new List<LibraryItem>();
        private readonly object locker = new object(); // для Monitor / lock

        public void AddItem(LibraryItem item)
        {
            lock (locker)
            {
                items.Add(item);
                Console.WriteLine(
                    $"[Потік {Thread.CurrentThread.ManagedThreadId}] Додано: {item.Title}"
                );
            }
        }

        public void ShowAllItems()
        {
            lock (locker)
            {
                foreach (var item in items)
                {
                    item.ShowInfo();
                }
            }
        }
    }

    // ===== ТОЧКА ВХОДУ =====
    class Program
    {
        static void Main(string[] args)
        {
            LibraryService library = new LibraryService();

            // ===== ЗАДАЧА 1: ДОДАВАННЯ КНИГ =====
            Task addTask = Task.Run(() =>
            {
                for (int i = 1; i <= 5; i++)
                {
                    library.AddItem(new Book
                    {
                        Title = "Книга " + i,
                        Year = 2000 + i
                    });

                    Thread.Sleep(200); // імітація роботи
                }
            });

            // ===== ЗАДАЧА 2: ВИВЕДЕННЯ КНИГ =====
            Task showTask = Task.Run(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    Thread.Sleep(300);
                    library.ShowAllItems();
                }
            });

            Task.WaitAll(addTask, showTask);

            Console.WriteLine("\nРобота завершена.");
        }
    }
}
