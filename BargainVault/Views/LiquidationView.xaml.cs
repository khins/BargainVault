using BargainVault.ViewModels;
using System.Windows;

namespace BargainVault.Views
{
    /// <summary>
    /// Interaction logic for LiquidationView.xaml
    /// </summary>
    public partial class LiquidationView : Window
    {
        private readonly LiquidationViewModel _viewModel;

        public LiquidationView(LiquidationViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;

            Loaded += async (_, _) => await _viewModel.LoadAsync();
        }

        private async void SetDisregard_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedCandidate == null)
                return;

            var confirm = MessageBox.Show(
                $"Mark '{_viewModel.SelectedCandidate.Title}' as disregard and remove it from this list?",
                "Confirm Disregard",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes)
                return;

            await _viewModel.MarkSelectedAsDisregardAsync();
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }
    }
}
