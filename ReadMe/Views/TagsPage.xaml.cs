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

        // Déclenché à chaque fois que la page s'affiche
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_viewModel != null)
            {
                await _viewModel.LoadTagsAsync();
            }
        }

        private void OnSearchButtonPressed(object sender, EventArgs e)
        {
            if (sender is SearchBar searchBar)
            {
                // transmet le texte de recherche au ViewModel
                _viewModel.SearchTagsCommand.Execute(searchBar.Text);
            }
        }

        private async void OnEditTagClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is Tag selectedTag)
            {
                await Shell.Current.GoToAsync($"///edittag?tagId={selectedTag.Id}");
            }
        }
    }
}