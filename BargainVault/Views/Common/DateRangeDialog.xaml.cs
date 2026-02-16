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

namespace BargainVault.Views.Common
{
    /// <summary>
    /// Interaction logic for DateRangeDialog.xaml
    /// </summary>
    public partial class DateRangeDialog : Window
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateRangeDialog()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
