using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReadMe.Models;

namespace ReadMe.ViewModels
{
    public class ChapterViewModel : INotifyPropertyChanged
    {
        private Chapter _chapter;
        public Chapter Chapter { get => _chapter; set { _chapter = value; OnPropertyChanged(); } }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
