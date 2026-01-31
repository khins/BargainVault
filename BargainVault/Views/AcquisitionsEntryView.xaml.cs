using BargainVault.ViewModels.Acquisitions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BargainVault.Views
{
    /// <summary>
    /// Interaction logic for AcquisitionsEntryView.xaml
    /// </summary>
    public partial class AcquisitionsEntryView : Window
    {
        public AcquisitionsEntryView(AcquisitionsEntryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
