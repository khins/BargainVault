using BargainVault.ViewModels.Items;
using System.Windows;

namespace BargainVault.Views
{
    public partial class ItemsEntryView : Window
    {
        public ItemsEntryView(ItemsEntryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public void LoadItem(int itemId)
        {
            if (DataContext is ItemsEntryViewModel vm)
            {
                _ = vm.LoadAsync(itemId);
            }
        }
    }
}
