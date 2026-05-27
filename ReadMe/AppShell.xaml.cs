using System;
using Microsoft.Maui.Controls;
using ReadMe.Views;

namespace ReadMe
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("edittag", typeof(EditTagPage));
            Routing.RegisterRoute("bookdetail", typeof(BookDetailPage));
            Routing.RegisterRoute("reader", typeof(ReaderPage));
        }

        private async void OnAccueilClicked(object sender, TappedEventArgs e)
        {
            Current.FlyoutIsPresented = false;
            await Shell.Current.GoToAsync("//homePage");
        }

        private async void OnBooksClicked(object sender, TappedEventArgs e)
        {
            Current.FlyoutIsPresented = false;
            await Shell.Current.GoToAsync("//homePage");
        }

        private async void OnTagsClicked(object sender, TappedEventArgs e)
        {
            Current.FlyoutIsPresented = false;
            await Shell.Current.GoToAsync("//tagsPage");
        }

        private async void OnSettingsClicked(object sender, TappedEventArgs e)
        {
            Current.FlyoutIsPresented = false;
            await Shell.Current.GoToAsync("//settingsPage");
        }
    }
}