using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReadMe.Models;

namespace ReadMe.ViewModels
{
    public class BookViewModel : INotifyPropertyChanged
    {
        private Book _book;
        public Book Book { get => _book; set { _book = value; OnPropertyChanged(); } }
        public ObservableCollection<Tag> AvailableTags { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
