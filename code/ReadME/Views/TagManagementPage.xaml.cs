using ReadMe_perso.ViewModels;

namespace ReadMe_perso.Views;

public partial class TagManagementPage : ContentPage
{
    public TagManagementPage()
    {
        InitializeComponent();
        BindingContext = new TagManagementViewModel();
    }
}
