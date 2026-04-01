using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ReadMe.Models;

namespace ReadMe.Services
{
    public class BookService
    {
        private List<Book> _books = new();
        private readonly string _filePath;
        private int _nextId = 1;

        public BookService(string appDataDirectory)
        {
            _filePath = Path.Combine(appDataDirectory, "books.json");
            LoadBooks();
        }

        private void LoadBooks()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _books = JsonSerializer.Deserialize<List<Book>>(json) ?? new();
                if (_books.Count > 0)
                    _nextId = _books.Max(b => b.Id) + 1;
            }
        }

        private void SaveBooks()
        {
            var json = JsonSerializer.Serialize(_books);
            File.WriteAllText(_filePath, json);
        }

        public List<Book> GetAllBooks() => _books;
        public Book GetBookById(int id) => _books.FirstOrDefault(b => b.Id == id);
        public List<Book> GetBooksByTags(List<int> tagIds) => _books.Where(b => b.Tags.Any(t => tagIds.Contains(t.Id))).ToList();
        public List<Book> GetBooksSortedByDate() => _books.OrderByDescending(b => b.DateAdded).ToList();

        public void AddBook(Book book)
        {
            book.Id = _nextId++;
            _books.Add(book);
            SaveBooks();
        }

        public void UpdateBook(Book book)
        {
            var index = _books.FindIndex(b => b.Id == book.Id);
            if (index >= 0)
            {
                _books[index] = book;
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
    }
}
