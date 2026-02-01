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
    public class SalesEntryViewModel : ViewModelBase
    {
        private readonly ISalesService _service;
        private int? _saleId;
        private readonly ISalesService _salesService;
        private readonly IItemsService _itemsService;
       
        public SalesEntryViewModel(ISalesService service)
        {
            _service = service;

            DateSold = DateTime.Today;
            QtySold = 1;

            SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
        }

        public SalesEntryViewModel(
            ISalesService salesService,
            IItemsService itemsService)
        {
            _salesService = salesService;
            _itemsService = itemsService;

            Items = new ObservableCollection<ItemDto>();

            DateSold = DateTime.Today;
            QtySold = 1;

            SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);

            _ = LoadItemsAsync();
        }

        private async Task LoadItemsAsync()
        {
            var items = await _itemsService.GetItemsAsync();

            foreach (var item in items.OrderBy(i => i.Title))
                Items.Add(item);
        }

        public SalesEntryViewModel(
            ISalesService salesService,
            IItemsService itemsService,
            SaleDto dto)
            : this(salesService, itemsService)
        {
            _saleId = dto.SaleId;

            SelectedItemId = dto.ItemId;
            DateSold = dto.DateSold;
            QtySold = dto.QtySold;
            ChannelType = dto.ChannelType;
            BoothId = dto.BoothId;
            UnitSalePrice = dto.UnitSalePrice;
            DiscountedRate = dto.DiscountedRate;
        }

        public ObservableCollection<ItemDto> Items { get; }

        private int _selectedItemId;
        public int SelectedItemId
        {
            get => _selectedItemId;
            set
            {
                if (SetProperty(ref _selectedItemId, value))
                {
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public int ItemId { get; set; }
        public DateTime DateSold { get; set; }
        private int _qtySold;
        public int QtySold
        {
            get => _qtySold;
            set
            {
                if (SetProperty(ref _qtySold, value))
                {
                    SaveCommand?.RaiseCanExecuteChanged();
                }
            }
        }
        public string? ChannelType { get; set; }
        public int? BoothId { get; set; }
        private decimal _unitSalePrice;
        public decimal UnitSalePrice
        {
            get => _unitSalePrice;
            set
            {
                if (SetProperty(ref _unitSalePrice, value))
                {
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public decimal? DiscountedRate { get; set; }

        public RelayCommand SaveCommand { get; }

        private bool CanSave()
            => SelectedItemId > 0
               && UnitSalePrice > 0
               && QtySold > 0;


        private async Task SaveAsync()
        {
            var dto = new SaleDto
            {
                SaleId = _saleId ?? 0,
                ItemId = ItemId,
                DateSold = DateSold,
                QtySold = QtySold,
                ChannelType = ChannelType,
                BoothId = BoothId,
                UnitSalePrice = UnitSalePrice,
                DiscountedRate = DiscountedRate
            };

            if (_saleId == null)
            {
                var newId = await _service.InsertSaleAsync(dto, Environment.UserName);
                _saleId = newId;
                MessageBox.Show($"Sale #{newId} saved.", "Saved");
            }
            else
            {
                await _service.UpdateSaleAsync(dto, Environment.UserName);
                MessageBox.Show("Sale updated.", "Saved");
            }
        }
    }

}
