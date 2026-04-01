using ReadMe_perso.Models;
using ReadMe_perso.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ReadMe_perso.ViewModels
{
    public class ReaderViewModel : INotifyPropertyChanged
    {
        private readonly BookService _bookService;
        private Book _currentBook = new();
        private string _displayContent = string.Empty;
        private int _currentPageNumber = 1;

        public Book CurrentBook
        {
            get => _currentBook;
            set
            {
                if (_currentBook != value)
                {
                    _currentBook = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DisplayContent
        {
            get => _displayContent;
            set
            {
                if (_displayContent != value)
                {
                    _displayContent = value;
                    OnPropertyChanged();
                }
            }
        }

        public int CurrentPageNumber
        {
            get => _currentPageNumber;
            set
            {
                if (_currentPageNumber != value)
                {
                    _currentPageNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PageIndicator => $"Page {CurrentBook?.CurrentPage ?? 1}/{CurrentBook?.TotalPages ?? 0}";

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        public ReaderViewModel()
        {
            _bookService = new BookService();
            _currentBook = new Book();
            NextPageCommand = new Command(NextPage);
            PreviousPageCommand = new Command(PreviousPage);
        }

        public void LoadBook(int bookId)
        {
            var book = _bookService.GetBookById(bookId);
            CurrentBook = book;
            CurrentPageNumber = book.CurrentPage;
            DisplayContent = GeneratePageContent(book.CurrentPage, book.TotalPages, book.Content);
            OnPropertyChanged(nameof(PageIndicator));
        }

        private void NextPage()
        {
            if (CurrentBook != null && CurrentBook.CurrentPage < CurrentBook.TotalPages)
            {
                CurrentBook.CurrentPage++;
                CurrentPageNumber++;
                DisplayContent = GeneratePageContent(CurrentBook.CurrentPage, CurrentBook.TotalPages, CurrentBook.Content);
                _bookService.UpdateBook(CurrentBook);
                OnPropertyChanged(nameof(PageIndicator));
            }
        }

        private void PreviousPage()
        {
            if (CurrentBook != null && CurrentBook.CurrentPage > 1)
            {
                CurrentBook.CurrentPage--;
                CurrentPageNumber--;
                DisplayContent = GeneratePageContent(CurrentBook.CurrentPage, CurrentBook.TotalPages, CurrentBook.Content);
                _bookService.UpdateBook(CurrentBook);
                OnPropertyChanged(nameof(PageIndicator));
            }
        }

        private string GeneratePageContent(int pageNumber, int totalPages, string baseContent)
        {
            // Simulate page content based on page number
            var lines = baseContent.Split(' ');
            var contentPerPage = Math.Max(100, lines.Length / totalPages);
            var startIndex = (pageNumber - 1) * contentPerPage;
            var endIndex = Math.Min(lines.Length, startIndex + contentPerPage);

            if (startIndex >= lines.Length)
                return $"Fin du livre - Page {pageNumber}/{totalPages}";

            var pageContent = string.Join(" ", lines.Skip(startIndex).Take(contentPerPage));
            return pageContent.Length > 0 ? pageContent : $"Page {pageNumber}/{totalPages}";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
