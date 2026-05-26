using System;
using Microsoft.Maui.Controls;
using ReadMe.ViewModels;

namespace ReadMe.Views
{
    [QueryProperty(nameof(BookId), "bookId")]
    public partial class ReaderPage : ContentPage
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
                    _viewModel.LoadReaderBook(_bookId);
                }
            }
        }

        public ReaderPage()
        {
            InitializeComponent();
            _viewModel = new BookViewModel();
            BindingContext = _viewModel;
        }
    }
}
