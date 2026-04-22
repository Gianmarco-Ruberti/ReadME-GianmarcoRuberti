using System;
using Microsoft.Maui.Controls;
using ReadMe.Models;
using ReadMe.ViewModels;

namespace ReadMe.Views
{
    public partial class TagsPage : ContentPage
    {
        private TagViewModel _viewModel;

        public TagsPage()
        {
            InitializeComponent();
            _viewModel = new TagViewModel();
            BindingContext = _viewModel;
        }

        private void OnSearchButtonPressed(object sender, EventArgs e)
        {
            // Logique de recherche de tags
        }

        private async void OnEditTagClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is Tag tag)
            {
                await Shell.Current.GoToAsync($"edittag?tagId={tag.Id}");
            }
        }
    }
}
