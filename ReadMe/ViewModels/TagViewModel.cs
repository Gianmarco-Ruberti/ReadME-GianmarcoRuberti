using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
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

            // Initialisation de la collection pour éviter des erreurs de Binding au démarrage
            Tags = new ObservableCollection<Tag>();

            // Liaison des commandes avec les méthodes asynchrones adaptées
            AddTagCommand = new Command(async () => await AddTagAsync());
            DeleteTagCommand = new Command<Tag>(async (tag) => await DeleteTagAsync(tag));
            UpdateTagCommand = new Command<Tag>(async (tag) => await UpdateTagAsync(tag));
            SearchTagsCommand = new Command<string>(async (text) => await SearchTagsAsync(text));
        }

        // Charge les données depuis le service de manière asynchrone
        public async Task LoadTagsAsync()
        {
            try
            {
                var tagList = await _tagService.GetTagsAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Tags.Clear();
                    foreach (var tag in tagList.OrderBy(t => t.Name))
                    {
                        Tags.Add(tag);
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Error LoadTags]: {ex.Message}");
            }
        }

        private async Task AddTagAsync()
        {
            if (string.IsNullOrWhiteSpace(NewTagName))
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", "Le nom du tag ne peut pas être vide", "OK");
                return;
            }

            var newTag = new Tag { Name = NewTagName, Color = NewTagColor };

            // Appel à l'API via le service
            bool success = await _tagService.CreateTagAsync(newTag);

            if (success)
            {
                // On recharge proprement la liste depuis la BDD pour avoir le bon ID généré par l'API
                await LoadTagsAsync();

                // Réinitialisation des champs du formulaire
                NewTagName = string.Empty;
                NewTagColor = "#512BD4";
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", "Impossible d'ajouter le tag sur le serveur", "OK");
            }
        }

        private async Task DeleteTagAsync(Tag tag)
        {
            if (tag == null) return;

            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmation", $"Supprimer le tag '{tag.Name}' ?", "Oui", "Non");
            if (!confirm) return;

            bool success = await _tagService.DeleteTagAsync(tag.Id);

            if (success)
            {
                Tags.Remove(tag);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", "Impossible de supprimer le tag sur le serveur", "OK");
            }
        }

        private async Task UpdateTagAsync(Tag tag)
        {
            if (tag == null) return;

            // TODO: Si tu as une méthode UpdateTagAsync dans ton TagService, appelle-la ici
            // bool success = await _tagService.UpdateTagAsync(tag);

            OnPropertyChanged(nameof(Tags));
        }

        private async Task SearchTagsAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                await LoadTagsAsync();
            }
            else
            {
                var tagList = await _tagService.GetTagsAsync();
                var filteredTags = tagList
                    .Where(t => t.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                Tags.Clear();
                foreach (var tag in filteredTags)
                {
                    Tags.Add(tag);
                }
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion
    }
}