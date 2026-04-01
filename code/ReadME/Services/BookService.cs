using ReadMe_perso.Models;
using System.Text.Json;
using System.Diagnostics;

namespace ReadMe_perso.Services
{
    public class BookService
    {
        private List<Book> _books = new();
        private const string BooksFileName = "books.json";

        public BookService()
        {
            LoadBooks();
        }

        public List<Book> GetAllBooks()
        {
            return _books;
        }

        public List<Book> GetBooksSortedByDate(bool ascending = false)
        {
            return ascending
                ? _books.OrderBy(b => b.DateAdded).ToList()
                : _books.OrderByDescending(b => b.DateAdded).ToList();
        }

        public List<Book> GetBooksByTags(List<int> tagIds)
        {
            if (tagIds.Count == 0)
                return _books;

            return _books.Where(b => b.TagIds.Any(tid => tagIds.Contains(tid))).ToList();
        }

        public Book GetBookById(int id)
        {
            return _books.FirstOrDefault(b => b.Id == id) ?? new Book();
        }

        public void AddBook(Book book)
        {
            book.Id = _books.Count > 0 ? _books.Max(b => b.Id) + 1 : 1;
            book.DateAdded = DateTime.Now;
            _books.Add(book);
            SaveBooks();
        }

        public void UpdateBook(Book book)
        {
            var existing = _books.FirstOrDefault(b => b.Id == book.Id);
            if (existing != null)
            {
                existing.Title = book.Title;
                existing.Author = book.Author;
                existing.CoverImagePath = book.CoverImagePath;
                existing.CurrentPage = book.CurrentPage;
                existing.TotalPages = book.TotalPages;
                existing.TagIds = book.TagIds;
                existing.Content = book.Content;
                SaveBooks();
            }
        }

        public void DeleteBook(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                _books.Remove(book);
                SaveBooks();
            }
        }

        private async void SaveBooks()
        {
            try
            {
                var appDataPath = FileSystem.AppDataDirectory;
                var filePath = Path.Combine(appDataPath, BooksFileName);
                var json = JsonSerializer.Serialize(_books);
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving books: {ex.Message}");
            }
        }

        private void LoadBooks()
        {
            try
            {
                var appDataPath = FileSystem.AppDataDirectory;
                var filePath = Path.Combine(appDataPath, BooksFileName);

                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    var loadedBooks = JsonSerializer.Deserialize<List<Book>>(json);
                    _books = loadedBooks ?? new();
                }
                else
                {
                    // Create sample books
                    _books = new()
                    {
                        new Book
                        {
                            Id = 1,
                            Title = "Le Seigneur des Anneaux",
                            Author = "J.R.R. Tolkien",
                            CoverImagePath = "book1.jpg",
                            DateAdded = DateTime.Now.AddDays(-30),
                            CurrentPage = 150,
                            TotalPages = 1000,
                            TagIds = new() { 1, 2 },
                            Content = "En une époque lointaine, dans la Terre du Milieu..."
                        },
                        new Book
                        {
                            Id = 2,
                            Title = "Fondation",
                            Author = "Isaac Asimov",
                            CoverImagePath = "book2.jpg",
                            DateAdded = DateTime.Now.AddDays(-20),
                            CurrentPage = 80,
                            TotalPages = 500,
                            TagIds = new() { 2 },
                            Content = "Sur Terminius, une petite planète de la Périphérie..."
                        },
                        new Book
                        {
                            Id = 3,
                            Title = "Mystère en Seine",
                            Author = "Agatha Christie",
                            CoverImagePath = "book3.jpg",
                            DateAdded = DateTime.Now.AddDays(-10),
                            CurrentPage = 250,
                            TotalPages = 350,
                            TagIds = new() { 4, 5 },
                            Content = "Un crime parfait semblait inévitable..."
                        },
                        new Book
                        {
                            Id = 4,
                            Title = "Biographie: Steve Jobs",
                            Author = "Walter Isaacson",
                            CoverImagePath = "book4.jpg",
                            DateAdded = DateTime.Now.AddDays(-5),
                            CurrentPage = 120,
                            TotalPages = 650,
                            TagIds = new() { 3 },
                            Content = "Steven Paul Jobs est né le 24 février 1955..."
                        },
                        new Book
                        {
                            Id = 5,
                            Title = "1984",
                            Author = "George Orwell",
                            CoverImagePath = "book5.jpg",
                            DateAdded = DateTime.Now,
                            CurrentPage = 1,
                            TotalPages = 350,
                            TagIds = new() { 1, 5 },
                            Content = "C'était une journée froide et lumineuse en avril..."
                        }
                    };
                    SaveBooks();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading books: {ex.Message}");
                _books = new();
            }
        }
    }
}
