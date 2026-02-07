using BargainVault.ViewModels.Items;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BargainVault.Views.Items
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

