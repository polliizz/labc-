using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinalProject
{
    // ===== СТАНИ (STATE MACHINE) =====
    enum ItemState
    {
        Available,
        Borrowed,
        Processing
    }

    // ===== ДЕЛЕГАТ ДЛЯ ПОДІЙ =====
    delegate void StateChangedHandler(LibraryItem item, ItemState oldState, ItemState newState);

    // ===== БАЗОВИЙ КЛАС =====
    abstract class LibraryItem : ICloneable, IComparable<LibraryItem>
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public ItemState State { get; private set; } = ItemState.Available;

        public event StateChangedHandler StateChanged;

        public int CompareTo(LibraryItem other)
        {
            return Year.CompareTo(other.Year);
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        protected void ChangeState(ItemState newState)
        {
            ItemState oldState = State;
            State = newState;
            StateChanged?.Invoke(this, oldState, newState);
        }

        // ===== ASYNC ПЕРЕХІД СТАНУ =====
        public async Task BorrowAsync()
        {
            if (State != ItemState.Available)
                return;

            ChangeState(ItemState.Processing);
            await Task.Delay(500);
            ChangeState(ItemState.Borrowed);
        }

        public async Task ReturnAsync()
        {
            if (State != ItemState.Borrowed)
                return;

            ChangeState(ItemState.Processing);
            await Task.Delay(500);
            ChangeState(ItemState.Available);
        }

        public abstract void ShowInfo();
    }

    // ===== КЛАС-НАЩАДОК =====
    class Book : LibraryItem
    {
        public string Author { get; set; }

        public override void ShowInfo()
        {
            Console.WriteLine($"{Title} ({Author}, {Year}) — Стан: {State}");
        }

        public override object Clone()
        {
            return new Book
            {
                Title = Title,
                Author = Author,
                Year = Year
            };
        }
    }

    // ===== СЕРВІС =====
    class LibraryService
    {
        private List<LibraryItem> items = new List<LibraryItem>();
        private readonly object locker = new object();

        public event Action<LibraryItem> ItemAdded;

        public void AddItem(LibraryItem item)
        {
            lock (locker)
            {
                items.Add(item);
                ItemAdded?.Invoke(item);
            }
        }

        public IEnumerable<LibraryItem> GetItemsAfterYear(int year)
        {
            foreach (var item in items)
                if (item.Year > year)
                    yield return item;
        }

        public List<LibraryItem> GetAll()
        {
            return items;
        }
    }

    // ===== ЛОГЕР (ПІДПИСКА НА ПОДІЇ) =====
    class LibraryLogger
    {
        public void OnStateChanged(LibraryItem item, ItemState oldState, ItemState newState)
        {
            Console.WriteLine(
                $"[STATE] {item.Title}: {oldState} → {newState}"
            );
        }

        public void OnItemAdded(LibraryItem item)
        {
            Console.WriteLine($"[EVENT] Додано: {item.Title}");
        }
    }

    // ===== ТОЧКА ВХОДУ =====
    class Program
    {
        static async Task Main(string[] args)
        {
            LibraryService library = new LibraryService();
            LibraryLogger logger = new LibraryLogger();

            Book book1 = new Book
            {
                Title = "Кобзар",
                Author = "Тарас Шевченко",
                Year = 1840
            };

            Book book2 = new Book
            {
                Title = "C# для початківців",
                Author = "Іван Іванов",
                Year = 2022
            };

            // ===== ПОДІЇ =====
            library.ItemAdded += logger.OnItemAdded;
            book1.StateChanged += logger.OnStateChanged;
            book2.StateChanged += logger.OnStateChanged;

            library.AddItem(book1);
            library.AddItem(book2);

            Console.WriteLine("\n=== LINQ (після 1900) ===");
            var modern = library.GetAll().Where(i => i.Year > 1900);
            foreach (var item in modern)
                item.ShowInfo();

            Console.WriteLine("\n=== STATE MACHINE ===");
            await book2.BorrowAsync();
            await Task.Delay(300);
            await book2.ReturnAsync();

            Console.WriteLine("\n=== КЛОНУВАННЯ ===");
            Book clone = (Book)book2.Clone();
            clone.Title = "Клон книги";
            clone.ShowInfo();

            Console.WriteLine("\n=== СОРТУВАННЯ ===");
            var all = library.GetAll();
            all.Sort();
            foreach (var item in all)
                item.ShowInfo();
        }
    }
}
