using System;
using Microsoft.Maui.Controls;
using ReadMe.Models;
using ReadMe.ViewModels;

namespace ReadMe.Views
{
    [QueryProperty(nameof(BookId), "bookId")]
    public partial class BookDetailPage : ContentPage
    {
        private readonly BookViewModel _viewModel;
        private int _bookId;

        public int BookId
        {
            get => _bookId;
            set
            {
                _bookId = value;
                if (_bookId > 0)
                {
                    _viewModel.LoadBook(_bookId);
                }
            }
        }

        public BookDetailPage()
        {
            InitializeComponent();
            _viewModel = new BookViewModel();
            BindingContext = _viewModel;
        }

        private void OnAvailableTagTapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is Tag tag)
            {
                _viewModel.AddTagToBook(tag);
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            _viewModel.SaveCurrentBook();
            await DisplayAlert("Succès", "Livre enregistré avec succès.", "OK");
            await Shell.Current.GoToAsync("homePage");
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Confirmation", "Supprimer ce livre ?", "Oui", "Non"))
            {
                _viewModel.DeleteCurrentBook();
                await Shell.Current.GoToAsync("homePage");
            }
        }
    }
}
