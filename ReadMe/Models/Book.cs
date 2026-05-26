using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ReadMe.Models
{
    public class Book : INotifyPropertyChanged
    {
        private int _id;
        private string _title;
        private string _author;
        private DateTime _dateAdded;
        private List<Tag> _tags;
        private List<Chapter> _chapters;
        private string _coverImagePath;
        private string _filePath;
        private int _currentPage;
        private int _totalPages;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string Author
        {
            get => _author;
            set { _author = value; OnPropertyChanged(); }
        }

        public DateTime DateAdded
        {
            get => _dateAdded;
            set { _dateAdded = value; OnPropertyChanged(); }
        }

        public List<Tag> Tags
        {
            get => _tags;
            set { _tags = value; OnPropertyChanged(); }
        }

        public List<Chapter> Chapters
        {
            get => _chapters;
            set { _chapters = value; OnPropertyChanged(); }
        }

        public string CoverImagePath
        {
            get => _coverImagePath;
            set { _coverImagePath = value; OnPropertyChanged(); }
        }

        public string FilePath
        {
            get => _filePath;
            set { _filePath = value; OnPropertyChanged(); }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set { _currentPage = value; OnPropertyChanged(); OnPropertyChanged(nameof(Progress)); }
        }

        public int TotalPages
        {
            get => _totalPages;
            set { _totalPages = value; OnPropertyChanged(); OnPropertyChanged(nameof(Progress)); }
        }

        public double Progress => TotalPages > 0 ? (double)CurrentPage / TotalPages : 0;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
