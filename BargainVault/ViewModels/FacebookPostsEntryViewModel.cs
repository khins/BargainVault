using BargainVault.Commands;
using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace BargainVault.ViewModels
{
    public class FacebookPostsEntryViewModel : ViewModelBase
    {
        private readonly IFacebookPostsService _facebookService;
        private readonly IAcquisitionsService _acquisitionsService;
           

        public ObservableCollection<AcquisitionLookupDto> Acquisitions { get; }
            = new();

        public RelayCommand SaveCommand { get; }

        public FacebookPostsEntryViewModel(
            IFacebookPostsService facebookService,
            IAcquisitionsService acquisitionsService)
        {
            _facebookService = facebookService;
            _acquisitionsService = acquisitionsService;

            SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);

            PostDate = DateTime.Now;

            _ = LoadAcquisitionsAsync();
        }

        // Edit constructor
        public FacebookPostsEntryViewModel(
            IFacebookPostsService facebookService,
            IAcquisitionsService acquisitionsService,
            FacebookPostDto dto)
            : this(facebookService, acquisitionsService)
        {
            _postId = dto.PostId;

            SelectedAcqId = dto.AcqId;
            PostDate = dto.PostDate;
            PostTitle = dto.PostTitle;
            PostDescription = dto.PostDescription;
            AskingPrice = dto.AskingPrice;
            Boosted = dto.Boosted;
            MarkAsSold = dto.MarkAsSold;
            RenewDate = dto.RenewDate;

            IsDirty = false;
        }

        // ──────────────────────────────
        // Properties
        // ──────────────────────────────

        private int _selectedAcqId;
        public int SelectedAcqId
        {
            get => _selectedAcqId;
            set
            {
                if (SetProperty(ref _selectedAcqId, value))
                    MarkDirty();
            }
        }

        private int _postId;
        public int PostId
        {
            get => _postId;
            set
            {
                if (SetProperty(ref _postId, value))
                    MarkDirty();
            }
        }

        private int _selectedAcquisitionId;
        public int SelectedAcquisitionId
        {
            get => _selectedAcquisitionId;
            set
            {
                if (SetProperty(ref _selectedAcquisitionId, value))
                    MarkDirty();
            }
        }

        private DateTime? _postDate;
        public DateTime? PostDate
        {
            get => _postDate;
            set
            {
                if (SetProperty(ref _postDate, value))
                    MarkDirty();
            }
        }

        private string? _postTitle;
        public string? PostTitle
        {
            get => _postTitle;
            set
            {
                if (SetProperty(ref _postTitle, value))
                    MarkDirty();
            }
        }

        private string? _postDescription;
        public string? PostDescription
        {
            get => _postDescription;
            set
            {
                if (SetProperty(ref _postDescription, value))
                    MarkDirty();
            }
        }

        private decimal? _askingPrice;
        public decimal? AskingPrice
        {
            get => _askingPrice;
            set
            {
                if (SetProperty(ref _askingPrice, value))
                    MarkDirty();
            }
        }

        private bool _boosted;
        public bool Boosted
        {
            get => _boosted;
            set
            {
                if (SetProperty(ref _boosted, value))
                    MarkDirty();
            }
        }

        private bool _markAsSold;
        public bool MarkAsSold
        {
            get => _markAsSold;
            set
            {
                if (SetProperty(ref _markAsSold, value))
                    MarkDirty();
            }
        }

        private DateTime? _renewDate;
        public DateTime? RenewDate
        {
            get => _renewDate;
            set
            {
                if (SetProperty(ref _renewDate, value))
                    MarkDirty();
            }
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        // ──────────────────────────────
        // Dirty tracking
        // ──────────────────────────────

        private bool _isDirty;
        public bool IsDirty
        {
            get => _isDirty;
            private set
            {
                SetProperty(ref _isDirty, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private void MarkDirty()
        {
            IsDirty = true;
        }

        // ──────────────────────────────
        // Commands
        // ──────────────────────────────

        private bool CanSave()
            => IsDirty && SelectedAcqId > 0;

        private async Task SaveAsync()
        {
            var dto = new FacebookPostDto
            {
                PostId = _postId,
                AcqId = SelectedAcqId,
                PostDate = PostDate,
                PostTitle = PostTitle,
                PostDescription = PostDescription,
                AskingPrice = AskingPrice,
                Boosted = Boosted,
                MarkAsSold = MarkAsSold,
                RenewDate = RenewDate
            };

            if (_postId == null)
            {
                // INSERT
                _postId = await _facebookService.InsertFacebookPostAsync(
                    dto, Environment.UserName);

                MessageBox.Show(
                    $"Facebook Post saved successfully.\n\nPost ID: {_postId}",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                // UPDATE
                await _facebookService.UpdateFacebookPostAsync(
                    dto, Environment.UserName);
            }

            IsDirty = false;
        }

        // ──────────────────────────────
        // Lookup loading
        // ──────────────────────────────

        private async Task LoadAcquisitionsAsync()
        {
            Acquisitions.Clear();

            var results = await _acquisitionsService.GetAcquisitionLookupAsync();
            foreach (var row in results)
                Acquisitions.Add(row);
        }

        public async Task LoadAsync(int postId)
        {
            var dto = await _facebookService.GetPostByIdAsync(postId);

            PostId = dto.PostId;
            SelectedAcquisitionId = dto.AcqId;
            PostDate = dto.PostDate;
            PostTitle = dto.PostTitle;
            PostDescription = dto.PostDescription;
            AskingPrice = dto.AskingPrice;
            Boosted = dto.Boosted;
            MarkAsSold = dto.MarkAsSold;
            RenewDate = dto.RenewDate;

            IsEditMode = true;
        }

    }

}
