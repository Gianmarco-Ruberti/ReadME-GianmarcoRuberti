using System;
using Microsoft.Maui.Controls;
using ReadMe.Models;
using ReadMe.ViewModels;

namespace ReadMe.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly BookViewModel _viewModel;

        public HomePage()
        {
            InitializeComponent();

            _viewModel = MauiProgram.GetService<BookViewModel>();

            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is BookViewModel viewModel)
            {
                try
                {
                    viewModel.LoadData();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[Erreur chargement HomePage]: {ex.Message}");
                }
            }
        }

        private async void OnReadClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is Book book)
            {
                await Shell.Current.GoToAsync($"reader?bookId={book.Id}");
            }
        }
    }
}
