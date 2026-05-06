using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ReadMe.Models;

namespace ReadMe.ViewModels
{
    public class TagViewModel : INotifyPropertyChanged
    {
        // --- Propriétés ---
        private Tag _currentTag;
        public Tag CurrentTag
        {
            get => _currentTag;
            set { _currentTag = value; OnPropertyChanged(); }
        }

        private string _newTagName;
        public string NewTagName
        {
            get => _newTagName;
            set { _newTagName = value; OnPropertyChanged(); }
        }

        private string _newTagColor = "#512BD4";
        public string NewTagColor
        {
            get => _newTagColor;
            set { _newTagColor = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Tag> Tags { get; set; } = new();

        // --- Commandes ---
        public ICommand AddTagCommand { get; }
        public ICommand DeleteTagCommand { get; }

        public TagViewModel()
        {
            AddTagCommand = new Command(AddTag);
            DeleteTagCommand = new Command<Tag>(DeleteTag);
        }

        private void AddTag()
        {
            if (string.IsNullOrWhiteSpace(NewTagName)) return;

            Tags.Add(new Tag
            {
                Id = Tags.Count+1,
                Name = NewTagName,
                Color = NewTagColor
            });

            NewTagName = string.Empty;
            NewTagColor = "#512BD4";
        }

        private void DeleteTag(Tag tag)
        {
            if (tag != null)
                Tags.Remove(tag);
        }

        // --- INotifyPropertyChanged ---
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}