using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDemo
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
            Console.WriteLine($"Книга: {Title}, {Year}");
        }
    }

    // ===== СЕРВІС =====
    class LibraryService
    {
        private List<LibraryItem> items = new List<LibraryItem>();
        private readonly object locker = new object();

        // ===== ASYNC-МЕТОД ДОДАВАННЯ =====
        public async Task AddItemsAsync(CancellationToken token)
        {
            for (int i = 1; i <= 10; i++)
            {
                token.ThrowIfCancellationRequested();

                await Task.Delay(300); // імітація довгої операції

                lock (locker)
                {
                    items.Add(new Book
                    {
                        Title = "Книга " + i,
                        Year = 2000 + i
                    });
                }

                Console.WriteLine("Додано книгу " + i);
            }
        }

        // ===== ASYNC-МЕТОД ВИВЕДЕННЯ =====
        public async Task ShowItemsAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(500);

                lock (locker)
                {
                    Console.WriteLine("Поточний список книг:");
                    foreach (var item in items)
                        item.ShowInfo();
                }

                Console.WriteLine();
            }
        }
    }

    // ===== ТОЧКА ВХОДУ =====
    class Program
    {
        static async Task Main(string[] args)
        {
            LibraryService library = new LibraryService();
            CancellationTokenSource cts = new CancellationTokenSource();

            // ===== СТВОРЕННЯ АСИНХРОННИХ ЗАВДАНЬ =====
            Task addTask = library.AddItemsAsync(cts.Token);
            Task showTask = library.ShowItemsAsync(cts.Token);

            // ===== ВІДСТЕЖЕННЯ СТАНУ =====
            Console.WriteLine("Стан addTask: " + addTask.Status);
            Console.WriteLine("Стан showTask: " + showTask.Status);

            // Даємо задачам трохи попрацювати
            await Task.Delay(2000);

            // ===== СКАСУВАННЯ =====
            Console.WriteLine("Скасування виконання...");
            cts.Cancel();

            try
            {
                await Task.WhenAll(addTask, showTask);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Асинхронні операції скасовано.");
            }

            // ===== КІНЦЕВИЙ СТАН =====
            Console.WriteLine("Кінцевий стан addTask: " + addTask.Status);
            Console.WriteLine("Кінцевий стан showTask: " + showTask.Status);
        }
    }
}
