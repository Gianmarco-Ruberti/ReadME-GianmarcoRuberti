using ReadMe_perso.Models;
using ReadMe_perso.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ReadMe_perso.ViewModels
{
    public class TagManagementViewModel : INotifyPropertyChanged
    {
        private readonly TagService _tagService;
        private ObservableCollection<Tag> _tags = new();
        private string _newTagName = string.Empty;
        private string _selectedColor = "#512BD4";

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

        public string NewTagName
        {
            get => _newTagName;
            set
            {
                if (_newTagName != value)
                {
                    _newTagName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (_selectedColor != value)
                {
                    _selectedColor = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddTagCommand { get; }
        public ICommand DeleteTagCommand { get; }
        public ICommand UpdateTagCommand { get; }

        public TagManagementViewModel()
        {
            _tagService = new TagService();
            LoadTags();

            AddTagCommand = new Command(AddTag);
            DeleteTagCommand = new Command<Tag>(DeleteTag);
            UpdateTagCommand = new Command<Tag>(UpdateTag);
        }

        private void LoadTags()
        {
            var tags = _tagService.GetAllTags();
            Tags = new ObservableCollection<Tag>(tags);
        }

        private void AddTag()
        {
            if (!string.IsNullOrWhiteSpace(NewTagName))
            {
                _tagService.AddTag(NewTagName, SelectedColor);
                LoadTags();
                NewTagName = string.Empty;
                SelectedColor = "#512BD4";
            }
        }

        private void DeleteTag(Tag tag)
        {
            if (tag != null)
            {
                _tagService.DeleteTag(tag.Id);
                LoadTags();
            }
        }

        private void UpdateTag(Tag tag)
        {
            if (tag != null)
            {
                _tagService.UpdateTag(tag.Id, tag.Name, tag.Color);
                LoadTags();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
