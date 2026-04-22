using System;
using Microsoft.Maui.Controls;
using ReadMe.Models;
using ReadMe.ViewModels;

namespace ReadMe.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new BookViewModel();
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
