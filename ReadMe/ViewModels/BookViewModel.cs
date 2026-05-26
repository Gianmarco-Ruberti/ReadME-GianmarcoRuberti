using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using ReadMe.Models;
using ReadMe.Services;

namespace ReadMe.ViewModels
{
    public class BookViewModel : INotifyPropertyChanged
    {
        private readonly BookService _bookService;
        private readonly TagService _tagService;
        private List<Book> _allBooks = new List<Book>();
        private Book _book;
        private Book _currentBook;
        private int _currentPageIndex;

        public ObservableCollection<Book> Books { get; set; } = new ObservableCollection<Book>();
        public ObservableCollection<Tag> Tags { get; set; } = new ObservableCollection<Tag>();
        public ObservableCollection<Tag> AvailableTags { get; set; } = new ObservableCollection<Tag>();

        public ICommand SortByDateCommand { get; }
        public ICommand FilterByTagsCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }

        public Book Book
        {
            get => _book;
            set { _book = value; OnPropertyChanged(); }
        }

        public Book CurrentBook
        {
            get => _currentBook;
            set
            {
                _currentBook = value;
                _currentPageIndex = 0;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayContent));
                OnPropertyChanged(nameof(PageIndicator));
            }
        }

        public string DisplayContent => CurrentBook?.Chapters?.ElementAtOrDefault(_currentPageIndex)?.Content ?? "Aucun contenu disponible.";

        public string PageIndicator => CurrentBook == null
            ? string.Empty
            : $"Chapitre {_currentPageIndex + 1} / {CurrentBook.Chapters?.Count ?? 0}";

        public BookViewModel()
        {
            _bookService = MauiProgram.GetService<BookService>() ?? throw new InvalidOperationException("BookService introuvable");
            _tagService = MauiProgram.GetService<TagService>() ?? throw new InvalidOperationException("TagService introuvable");

            SortByDateCommand = new Command(SortByDate);
            FilterByTagsCommand = new Command<object>(FilterByTags);
            PreviousPageCommand = new Command(GoToPreviousPage);
            NextPageCommand = new Command(GoToNextPage);
        }

        public void LoadData()
        {
            _allBooks = _bookService.GetAllBooks() ?? new List<Book>();
            RefreshBooks(_allBooks);
            RefreshTags(_tagService.GetAllTags());
        }

        public void LoadBook(int bookId)
        {
            Book = _bookService.GetBookById(bookId);
            if (Book == null)
            {
                return;
            }

            if (Book.Tags == null)
            {
                Book.Tags = new List<Tag>();
            }

            AvailableTags.Clear();
            foreach (var tag in _tagService.GetAllTags())
            {
                AvailableTags.Add(tag);
            }
        }

        public void LoadReaderBook(int bookId)
        {
            CurrentBook = _bookService.GetBookById(bookId);
            if (CurrentBook == null)
            {
                return;
            }

            if (CurrentBook.Chapters == null || !CurrentBook.Chapters.Any())
            {
                CurrentBook.Chapters = new List<Chapter>
                {
                    new Chapter
                    {
                        Number = 1,
                        Title = CurrentBook.Title,
                        Content = "Aucun contenu de lecture n'est disponible pour ce livre pour l'instant."
                    }
                };
            }

            _currentPageIndex = 0;
            OnPropertyChanged(nameof(DisplayContent));
            OnPropertyChanged(nameof(PageIndicator));
        }

        private void RefreshBooks(IEnumerable<Book> books)
        {
            Books.Clear();
            foreach (var book in books)
            {
                Books.Add(book);
            }
        }

        private void RefreshTags(IEnumerable<Tag> tags)
        {
            Tags.Clear();
            foreach (var tag in tags)
            {
                Tags.Add(tag);
            }
        }

        private void SortByDate()
        {
            RefreshBooks(_allBooks.OrderByDescending(b => b.DateAdded));
        }

        private void FilterByTags(object selectedItems)
        {
            if (selectedItems is not System.Collections.IList selection || selection.Count == 0)
            {
                RefreshBooks(_allBooks);
                return;
            }

            var selectedTagIds = selection.OfType<Tag>().Select(t => t.Id).ToList();
            if (!selectedTagIds.Any())
            {
                RefreshBooks(_allBooks);
                return;
            }

            var filtered = _allBooks.Where(b => b.Tags?.Any(t => selectedTagIds.Contains(t.Id)) == true).ToList();
            RefreshBooks(filtered);
        }

        private void GoToPreviousPage()
        {
            if (CurrentBook?.Chapters == null || _currentPageIndex <= 0) return;
            _currentPageIndex--;
            OnPropertyChanged(nameof(DisplayContent));
            OnPropertyChanged(nameof(PageIndicator));
        }

        private void GoToNextPage()
        {
            if (CurrentBook?.Chapters == null || _currentPageIndex >= CurrentBook.Chapters.Count - 1) return;
            _currentPageIndex++;
            OnPropertyChanged(nameof(DisplayContent));
            OnPropertyChanged(nameof(PageIndicator));
        }

        public void SaveCurrentBook()
        {
            if (Book == null) return;
            _bookService.UpdateBook(Book);
        }

        public void DeleteCurrentBook()
        {
            if (Book == null) return;
            _bookService.DeleteBook(Book.Id);
        }

        public void AddTagToBook(Tag tag)
        {
            if (Book == null || tag == null) return;

            if (Book.Tags == null)
            {
                Book.Tags = new List<Tag>();
            }

            if (Book.Tags.Any(t => t.Id == tag.Id))
            {
                return;
            }

            Book.Tags.Add(tag);
            OnPropertyChanged(nameof(Book));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
