using BargainVault.Commands;
using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;

namespace BargainVault.ViewModels
{
    public class FacebookPostsListViewModel : ViewModelBase
    {
        private readonly IFacebookPostsService _service;

        public ObservableCollection<FacebookPostListDto> Posts { get; }
            = new();

        public ICollectionView PostsView { get; private set; }

        public RelayCommand RefreshCommand { get; }

        public FacebookPostsListViewModel(IFacebookPostsService service)
        {
            _service = service;
            RefreshCommand = new RelayCommand(async () => await LoadAsync());
            _ = LoadAsync();
        }

        private FacebookPostListDto? _selectedPost;
        public FacebookPostListDto? SelectedPost
        {
            get => _selectedPost;
            set
            {
                SetProperty(ref _selectedPost, value);
                OnPropertyChanged(nameof(HasSelection));
            }
        }

        public bool HasSelection => SelectedPost != null;

        // 🔍 Search
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                PostsView?.Refresh();
            }
        }

        public async Task LoadAsync()
        {
            Posts.Clear();

            var results = await _service.GetFacebookPostsAsync();
            foreach (var p in results)
                Posts.Add(p);

            PostsView = CollectionViewSource.GetDefaultView(Posts);
            PostsView.Filter = FilterPosts;

            OnPropertyChanged(nameof(PostsView));
        }

        private bool FilterPosts(object obj)
        {
            if (obj is not FacebookPostListDto p)
                return false;

            if (string.IsNullOrWhiteSpace(SearchText))
                return true;

            return p.ItemTitle.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }

        public async Task DeleteSelectedAsync()
        {
            if (SelectedPost == null)
                return;

            await _service.DeleteFacebookPostAsync(
                SelectedPost.PostId,
                Environment.UserName);

            await LoadAsync();
        }
    }

}
