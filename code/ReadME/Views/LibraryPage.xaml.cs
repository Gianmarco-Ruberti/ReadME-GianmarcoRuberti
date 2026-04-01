using ReadMe_perso.Models;
using ReadMe_perso.ViewModels;

namespace ReadMe_perso.Views;

public partial class LibraryPage : ContentPage
{
    public LibraryPage()
    {
        InitializeComponent();
        BindingContext = new LibraryViewModel();
    }

    private async void OnReadClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Book book)
        {
            await Shell.Current.GoToAsync($"reader?bookId={book.Id}");
        }
    }
}
