using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ReadMe.Models;
using ReadMe.Services;

namespace ReadMe.ViewModels
{
    public class TagViewModel : INotifyPropertyChanged
    {
        private readonly TagService _tagService;
        private Tag _tag;
        private Tag _currentTag;
        private string _newTagName = string.Empty;
        private string _newTagColor = "#512BD4";
        private ObservableCollection<Tag> _tags;

        public Tag Tag
        {
            get => _tag;
            set { _tag = value; OnPropertyChanged(); }
        }

        public Tag CurrentTag
        {
            get => _currentTag;
            set { _currentTag = value; OnPropertyChanged(); }
        }

        public string NewTagName
        {
            get => _newTagName;
            set { _newTagName = value; OnPropertyChanged(); }
        }

        private string newTagColor = "#512BD4"; // Couleur par d�faut
        public string NewTagColor
        {
            get => _newTagColor;
            set { _newTagColor = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Tag> Tags
        {
            get => _tags;
            set { _tags = value; OnPropertyChanged(); }
        }

        public ICommand AddTagCommand { get; }
        public ICommand DeleteTagCommand { get; }
        public ICommand UpdateTagCommand { get; }
        public ICommand SearchTagsCommand { get; }

        public TagViewModel()
        {
            _tagService = MauiProgram.GetService<TagService>();
            InitializeTags();

            AddTagCommand = new Command(AddTag);
            DeleteTagCommand = new Command<Tag>(DeleteTag);
            UpdateTagCommand = new Command<Tag>(UpdateTag);
            SearchTagsCommand = new Command<string>(SearchTags);
        }

        private void InitializeTags()
        {
            var tagList = _tagService.GetAllTags();
            Tags = new ObservableCollection<Tag>(tagList);
        }

        private void AddTag()
        {
            if (string.IsNullOrWhiteSpace(NewTagName))
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", "Le nom du tag ne peut pas être vide", "OK");
                });
                return;
            }

            var newTag = new Tag { Name = NewTagName, Color = NewTagColor };
            _tagService.AddTag(newTag);
            Tags.Add(newTag);

            NewTagName = string.Empty;
            NewTagColor = "#512BD4";
        }

        private void DeleteTag(Tag tag)
        {
            if (tag != null)
            {
                _tagService.DeleteTag(tag.Id);
                Tags.Remove(tag);
            }
        }

        private void UpdateTag(Tag tag)
        {
            if (tag != null)
            {
                _tagService.UpdateTag(tag);
                OnPropertyChanged(nameof(Tags));
            }
        }

        public async Task LoadTagsAsync()
        {
            await Task.Run(() => InitializeTags());
        }

        private void SearchTags(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                InitializeTags();
            }
            else
            {
                var filteredTags = _tagService.GetAllTags()
                    .Where(t => t.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                Tags = new ObservableCollection<Tag>(filteredTags);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

