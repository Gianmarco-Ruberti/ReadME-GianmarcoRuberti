using System;
using Microsoft.Maui.Controls;
using ReadMe.Models;
using ReadMe.ViewModels;

namespace ReadMe.Views
{
    [QueryProperty(nameof(TagId), "tagId")]
    public partial class EditTagPage : ContentPage
    {
        private TagViewModel _viewModel;
        private int _tagId;

        public int TagId
        {
            get => _tagId;
            set
            {
                _tagId = value;
                if (_tagId > 0)
                {
                    LoadTag();
                }
            }
        }

        public EditTagPage()
        {
            InitializeComponent();
            _viewModel = new TagViewModel();
            BindingContext = _viewModel;
        }

        private void LoadTag()
        {
            var tagService = MauiProgram.GetService<ReadMe.Services.TagService>();
            var tag = tagService.GetTagById(_tagId);
            if (tag != null)
            {
                _viewModel.CurrentTag = new Tag { Id = tag.Id, Name = tag.Name, Color = tag.Color };
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_viewModel.CurrentTag?.Name))
            {
                await DisplayAlert("Erreur", "Le nom du tag ne peut pas être vide", "OK");
                return;
            }

            var tagService = MauiProgram.GetService<ReadMe.Services.TagService>();
            tagService.UpdateTag(_viewModel.CurrentTag);

            await DisplayAlert("Succès", "Tag mis à jour avec succès", "OK");
            await Shell.Current.GoToAsync("tags");
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("tags");
        }

        private void OnColorTapped(object sender, TappedEventArgs e)
        {
            if (sender is BoxView boxView && boxView.Color != null)
            {
                _viewModel.CurrentTag.Color = boxView.Color.ToHex();
            }
        }
    }
}
