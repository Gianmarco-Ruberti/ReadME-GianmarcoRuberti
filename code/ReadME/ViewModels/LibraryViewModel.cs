using ReadMe_perso.Models;
using ReadMe_perso.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ReadMe_perso.ViewModels
{
    public class LibraryViewModel : INotifyPropertyChanged
    {
        private readonly BookService _bookService;
        private readonly TagService _tagService;
        private ObservableCollection<Book> _books = new();
        private ObservableCollection<Tag> _tags = new();
        private List<int> _selectedTagIds = new();
        private bool _sortAscending = false;

        public ObservableCollection<Book> Books
        {
            get => _books;
            set
            {
                if (_books != value)
                {
                    _books = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Tag> Tags
        {
            get => _tags;
            set
            {
                if (_tags != value)
                {
                    _tags = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SortByDateCommand { get; }
        public ICommand FilterByTagsCommand { get; }
        public ICommand RemoveTagFilterCommand { get; }

    public LibraryViewModel()
    {
        _bookService = new BookService();
        _tagService = new TagService();

        LoadBooks();
        LoadTags();

        SortByDateCommand = new Command(SortByDate);
        FilterByTagsCommand = new Command<Tag>(FilterByTag);
        RemoveTagFilterCommand = new Command<Tag>(RemoveTagFilter);
    }

        private void LoadBooks()
        {
            var books = _bookService.GetAllBooks();
            Books = new ObservableCollection<Book>(books);
        }

        private void LoadTags()
        {
            var tags = _tagService.GetAllTags();
            Tags = new ObservableCollection<Tag>(tags);
        }

        private void SortByDate()
        {
            _sortAscending = !_sortAscending;
            var sortedBooks = _bookService.GetBooksSortedByDate(_sortAscending);
            Books = new ObservableCollection<Book>(sortedBooks);
        }

        private void FilterByTag(Tag tag)
        {
            if (!_selectedTagIds.Contains(tag.Id))
            {
                _selectedTagIds.Add(tag.Id);
            }
            else
            {
                _selectedTagIds.Remove(tag.Id);
            }
            ApplyFilter();
        }

        private void RemoveTagFilter(Tag tag)
        {
            _selectedTagIds.Remove(tag.Id);
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var filteredBooks = _bookService.GetBooksByTags(_selectedTagIds);
            Books = new ObservableCollection<Book>(filteredBooks);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
