using ReadMe_perso.ViewModels;

namespace ReadMe_perso.Views;

[QueryProperty(nameof(BookId), "bookId")]
public partial class ReaderPage : ContentPage
{
    private readonly ReaderViewModel _viewModel;
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

    public ReaderPage()
    {
        InitializeComponent();
        _viewModel = new ReaderViewModel();
        BindingContext = _viewModel;
    }
}
