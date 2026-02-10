using BargainVault.Commands;
using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BargainVault.ViewModels.Acquisitions
{
    public class AcquisitionsEntryViewModel : ViewModelBase
    {
        private readonly IAcquisitionsService _acquisitionsService;
        private readonly IItemsService _itemsService;
        private readonly ILookupsService _lookupsService;


        public AcquisitionsEntryViewModel(
            IAcquisitionsService acquisitionsService,
            IItemsService itemsService,
            ILookupsService lookupsService)
        {
            _acquisitionsService = acquisitionsService;
            _itemsService = itemsService;
            _lookupsService = lookupsService;

            Items = new ObservableCollection<ItemDto>();
            AuctionSites = new ObservableCollection<LookupDto>();
            Statuses = new ObservableCollection<LookupDto>();

            SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);

            DateAcquired = DateTime.Today;

            _ = LoadLookupsAsync();
            _lookupsService = lookupsService;
        }

        public AcquisitionsEntryViewModel(
            IAcquisitionsService acquisitionsService,
            IItemsService itemsService,
            ILookupsService lookupsService,
            AcquisitionDto dto)
            : this(acquisitionsService, itemsService, lookupsService)
        {
            _acqId = dto.AcqId;

            SelectedItemId = dto.ItemId;
            SelectedAuctionSiteId = dto.AuctionSiteId;
            SelectedStatusId = dto.StatusId;

            DateAcquired = dto.DateAcquired ?? DateTime.Today;
            QtyAcquired = dto.QtyAcquired;

            UnitHammerPrice = dto.UnitHammerPrice;
            BuyerPremium = dto.BuyerPremium;
            TaxRate = dto.TaxRate;

            SalesTaxPaid = dto.SalesTaxPaid;
            TotalSettlement = dto.TotalSettlement;

            IsPersonal = dto.Personal;
            IsBusinessExpense = dto.BusinessExpense;

            SaveCommand.RaiseCanExecuteChanged();
        }


        // -------------------------
        // Lookups
        // -------------------------

        public ObservableCollection<ItemDto> Items { get; }
        public ObservableCollection<LookupDto> AuctionSites { get; }
        public ObservableCollection<LookupDto> Statuses { get; }

        private int? _selectedItemId;
        public int? SelectedItemId
        {
            get => _selectedItemId;
            set
            {
                SetProperty(ref _selectedItemId, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private int? _acqId;
        public int? AcqId
        {
            get => _acqId;
            set
            {
                SetProperty(ref _acqId, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }        

        private int? _selectedAuctionSiteId;
        public int? SelectedAuctionSiteId
        {
            get => _selectedAuctionSiteId;
            set => SetProperty(ref _selectedAuctionSiteId, value);
        }

        private int? _selectedStatusId;
        public int? SelectedStatusId
        {
            get => _selectedStatusId;
            set => SetProperty(ref _selectedStatusId, value);
        }

        // -------------------------
        // Entry Fields
        // -------------------------

        private DateTime? _dateAcquired;
        public DateTime? DateAcquired
        {
            get => _dateAcquired;
            set => SetProperty(ref _dateAcquired, value);
        }

        private int _qtyAcquired = 1;
        public int QtyAcquired
        {
            get => _qtyAcquired;
            set => SetProperty(ref _qtyAcquired, value);
        }

        private decimal? _unitHammerPrice;
        public decimal? UnitHammerPrice
        {
            get => _unitHammerPrice;
            set
            {
                SetProperty(ref _unitHammerPrice, value);
                RecalculateTotals();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private decimal? _buyerPremium;
        public decimal? BuyerPremium
        {
            get => _buyerPremium;
            set
            {
                SetProperty(ref _buyerPremium, value);
                RecalculateTotals();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private decimal? _taxRate;
        public decimal? TaxRate
        {
            get => _taxRate;
            set
            {
                SetProperty(ref _taxRate, value);
                RecalculateTotals();
            }
        }

        private decimal? _salesTaxPaid;
        public decimal? SalesTaxPaid
        {
            get => _salesTaxPaid;
            private set => SetProperty(ref _salesTaxPaid, value);
        }

        private decimal? _totalSettlement;
        public decimal? TotalSettlement
        {
            get => _totalSettlement;
            private set => SetProperty(ref _totalSettlement, value);
        }

        private bool _isPersonal;
        public bool IsPersonal
        {
            get => _isPersonal;
            set => SetProperty(ref _isPersonal, value);
        }

        private bool _isBusinessExpense;
        public bool IsBusinessExpense
        {
            get => _isBusinessExpense;
            set => SetProperty(ref _isBusinessExpense, value);
        }

        
        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

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


        // -------------------------
        // Commands
        // -------------------------

        public RelayCommand SaveCommand { get; }

        private bool CanSave()
        {
            return SelectedItemId.HasValue
                && UnitHammerPrice.HasValue;
        }

        private AcquisitionDto BuildDto()
        {
            return new AcquisitionDto
            {
                AcqId = _acqId ?? 0,   // used for UPDATE, ignored for INSERT

                ItemId = SelectedItemId.Value,
                SourceType = "Auction",

                AuctionSiteId = SelectedAuctionSiteId,
                DateAcquired = DateAcquired,

                QtyAcquired = QtyAcquired,

                UnitHammerPrice = UnitHammerPrice,
                BuyerPremium = BuyerPremium,
                TaxRate = TaxRate,
                SalesTaxPaid = SalesTaxPaid,
                TotalSettlement = TotalSettlement,

                StatusId = SelectedStatusId,

                Personal = IsPersonal,
                BusinessExpense = IsBusinessExpense
            };
        }

        private async Task SaveAsync()
        {
            if (IsEditMode)
            {
                await _acquisitionsService.UpdateAcquisitionAsync(BuildDto());
                MessageBox.Show("Acquisition updated successfully.", "Updated");
            }
            else
            {
                int newId = await _acquisitionsService.InsertAcquisitionAsync(
                    itemId: SelectedItemId.Value,
                    sourceType: "Auction",
                    auctionSiteId: SelectedAuctionSiteId,
                    dateAcquired: DateAcquired,
                    qtyAcquired: QtyAcquired,
                    unitHammerPrice: UnitHammerPrice,
                    buyerPremium: BuyerPremium,
                    taxRate: TaxRate,
                    salesTaxPaid: SalesTaxPaid,
                    totalSettlement: TotalSettlement,
                    statusId: SelectedStatusId,
                    personal: IsPersonal,
                    businessExpense: IsBusinessExpense,
                    enteredBy: Environment.UserName
                );
                // ✅ Confirmation
                MessageBox.Show(
                    $"Acquisition saved successfully.\n\nAcquisition ID: {newId}",
                    "Saved",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // ✅ Auto-reset for next entry
                ResetForm();
            }
        }

        private void ResetForm()
        {
            SelectedItemId = null;
            SelectedAuctionSiteId = null;
            SelectedStatusId = null;

            DateAcquired = DateTime.Today;
            QtyAcquired = 1;

            UnitHammerPrice = null;
            BuyerPremium = null;
            TaxRate = null;

            SalesTaxPaid = null;
            TotalSettlement = null;

            IsPersonal = false;
            IsBusinessExpense = false;

            // If using command CanExecute
            SaveCommand.RaiseCanExecuteChanged();
        }


        // -------------------------
        // Helpers
        // -------------------------

        private void RecalculateTotals()
        {
            var baseAmount =
                (UnitHammerPrice ?? 0m) +
                (BuyerPremium ?? 0m);

            SalesTaxPaid = TaxRate.HasValue
                ? Math.Round(baseAmount * TaxRate.Value, 2)
                : 0m;

            TotalSettlement = baseAmount + (SalesTaxPaid ?? 0m);
        }

        private async Task LoadLookupsAsync()
        {
            AuctionSites.Clear();
            Statuses.Clear();

            var items = await _lookupsService.GetItemsAsync();
            foreach (var item in items)
                Items.Add(item);

            foreach (var site in await _lookupsService.GetAuctionSitesAsync())
                AuctionSites.Add(site);

            foreach (var status in await _lookupsService.GetAcquisitionStatusesAsync())
                Statuses.Add(status);
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public async Task LoadAsync(int acqId)
        {
            // 1️⃣ Load lookup data first
            await LoadLookupsAsync();

            // 2️⃣ Load acquisition
            var dto = await _acquisitionsService.GetAcquisitionByIdAsync(acqId);

            AcqId = dto.AcqId;
            SelectedItemId = dto.ItemId;
            DateAcquired = dto.DateAcquired;
            UnitHammerPrice = dto.UnitHammerPrice;
            BuyerPremium = dto.BuyerPremium;
            SalesTaxPaid = dto.SalesTaxPaid;
            TotalSettlement = dto.TotalSettlement;
            IsPersonal = dto.Personal;
            IsBusinessExpense = dto.BusinessExpense;

            IsEditMode = true;
        }

        public async Task LoadForEditAsync(int acqId)
        {
            await LoadLookupsAsync();
            await LoadAcquisitionAsync(acqId);
        }

        private async Task LoadAcquisitionAsync(int acqId)
        {
            var dto = await _acquisitionsService.GetAcquisitionByIdAsync(acqId);
            if (dto == null)
                return;

            _acqId = dto.AcqId;

            SelectedItemId = dto.ItemId;
            SelectedAuctionSiteId = dto.AuctionSiteId;
            SelectedStatusId = dto.StatusId;

            DateAcquired = dto.DateAcquired;
            QtyAcquired = dto.QtyAcquired;
            UnitHammerPrice = dto.UnitHammerPrice;
            BuyerPremium = dto.BuyerPremium;
            SalesTaxPaid = dto.SalesTaxPaid;
            TotalSettlement = dto.TotalSettlement;

            IsPersonal = dto.Personal;
            IsBusinessExpense = dto.BusinessExpense;

            IsDirty = false;
        }


    }
}
