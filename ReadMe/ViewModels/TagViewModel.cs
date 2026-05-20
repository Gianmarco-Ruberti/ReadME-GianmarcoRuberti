using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ReadMe.Models;

namespace ReadMe.ViewModels
{
    public class TagViewModel : INotifyPropertyChanged
    {
        // Liste observable connectée à la CollectionView du XAML
        public ObservableCollection<Tag> Tags { get; set; } = new ObservableCollection<Tag>();

        private Tag _currentTag;
        public Tag CurrentTag
        {
            get => _currentTag;
            set
            {
                _currentTag = value;
                OnPropertyChanged(); // Notifie le XAML du changement
            }
        }

        private string _newTagName;
        public string NewTagName
        {
            get => _newTagName;
            set { _newTagName = value; OnPropertyChanged(); }
        }

        private string _newTagColor = "#512BD4"; // Couleur par défaut
        public string NewTagColor
        {
            get => _newTagColor;
            set { _newTagColor = value; OnPropertyChanged(); }
        }

        // Commandes utilisées par le XAML
        public ICommand AddTagCommand { get; }
        public ICommand DeleteTagCommand { get; }
        public ICommand SearchTagsCommand { get; }

        public TagViewModel()
        {
            AddTagCommand = new Command(async () => await AddTagAsync());
            DeleteTagCommand = new Command<Tag>(async (tag) => await DeleteTagAsync(tag));
            SearchTagsCommand = new Command<string>(async (query) => await SearchTagsAsync(query));
        }

        //Appel API : Charger la liste des tags
        public async Task LoadTagsAsync()
        {
            try
            {
                // TODO: Remplace par ton appel API réel (HttpClient)
                // var tagsFromApi = await _apiService.GetTagsAsync();

                Tags.Clear();
                // Simulation de données pour tester ton XAML immédiatement :
                Tags.Add(new Tag { Id = 1, Name = "Roman", Color = "#FF5733" });
                Tags.Add(new Tag { Id = 2, Name = "Sci-Fi", Color = "#33FF57" });
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erreur", "Impossible de charger les tags", "OK");
            }
        }

        private async Task AddTagAsync()
        {
            if (string.IsNullOrWhiteSpace(NewTagName)) return;

            var newTag = new Tag { Name = NewTagName, Color = NewTagColor };

            // TODO: Envoi POST à ton API
            // await _apiService.CreateTagAsync(newTag);

            Tags.Add(newTag); // Ajout visuel direct

            // Réinitialisation du champ de saisie
            NewTagName = string.Empty;
        }

        // Appel API : Supprimer un tag
        private async Task DeleteTagAsync(Tag tag)
        {
            if (tag == null) return;

            bool confirm = await App.Current.MainPage.DisplayAlert("Confirmation", $"Supprimer le tag '{tag.Name}' ?", "Oui", "Non");
            if (!confirm) return;

            // TODO: Envoi DELETE à ton API
            // await _apiService.DeleteTagAsync(tag.Id);

            Tags.Remove(tag);
        }

        // Appel API : Filtrer les tags locaux ou via l'API
        private async Task SearchTagsAsync(string query)
        {
            // Logique de filtrage (soit en rappelant l'API, soit en filtrant la liste locale)
        }

        #region MVVM PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}