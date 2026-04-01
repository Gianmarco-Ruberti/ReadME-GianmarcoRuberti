using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReadMe.Models;

namespace ReadMe.ViewModels
{
    public class TagViewModel : INotifyPropertyChanged
    {
        private Tag _tag;
        public Tag Tag { get => _tag; set { _tag = value; OnPropertyChanged(); } }
        public ObservableCollection<Tag> Tags { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
