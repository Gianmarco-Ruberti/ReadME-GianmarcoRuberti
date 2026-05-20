namespace ReadMe
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("homePage", typeof(Views.HomePage));
            Routing.RegisterRoute("tagsPage", typeof(Views.TagsPage));
            Routing.RegisterRoute("settingsPage", typeof(Views.SettingsPage));
            Routing.RegisterRoute("readerPage", typeof(Views.ReaderPage));
            Routing.RegisterRoute("EditTagPage", typeof (Views.EditTagPage));
            Routing.RegisterRoute("BookDetailPage", typeof(Views.BookDetailPage));
        }

        private async void OnAccueilClicked(object sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync("homePage");
        }

        private async void OnBooksClicked(object sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync("homePage");
        }

        private async void OnTagsClicked(object sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync("tagsPage");
        }

        private async void OnSettingsClicked(object sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync("settingsPage");
        }
    }
}