using System;
using Microsoft.Maui.Controls;
using ReadMe.ViewModels;

namespace ReadMe.Views
{
    public partial class BookDetailPage : ContentPage
    {
        public BookDetailPage()
        {
            InitializeComponent();
            BindingContext = new BookViewModel();
        }
    }
}
