using BargainVault.Views.Items;
using global::BargainVault.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace BargainVault.UI
{
    public static class NavigationHelper
    {
        public static void OpenItem(int itemId)
        {
            var view = App.Services.GetRequiredService<ItemsEntryView>();
            view.LoadItem(itemId);
            Show(view);
        }

        public static void OpenAcquisition(int acqId)
        {
            var view = App.Services.GetRequiredService<AcquisitionsEntryView>();
            view.LoadAcquisition(acqId);
            Show(view);
        }

        public static void OpenFacebookPost(int postId)
        {
            var view = App.Services.GetRequiredService<FacebookPostsEntryView>();
            view.LoadPost(postId);
            Show(view);
        }

        public static void OpenSale(int postId)
        {
            //var view = App.Services.GetRequiredService<FacebookPostsEntryView>();
            //view.LoadPost(postId);
            //Show(view);
        }

        private static void Show(Window view)
        {
            view.Owner = Application.Current.MainWindow;
            view.ShowDialog();
        }
    }
}
