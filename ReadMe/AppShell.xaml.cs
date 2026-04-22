namespace ReadMe
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private async void OnAccueilClicked(object sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync("home");
        }

        private async void OnBooksClicked(object sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync("home");
        }

        private async void OnTagsClicked(object sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync("tags");
        }

        private async void OnSettingsClicked(object sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync("settings");
        }
    }
}
