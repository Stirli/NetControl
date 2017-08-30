using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;
using NetControlClient.Classes;

namespace NetControlClient.Utils
{
    public static class ObservableCollectionHelpers
    {
        public static Dispatcher GetDispatcher<T>(this ObservableCollection<T> collection)
        {
            var view = (CollectionView) CollectionViewSource.GetDefaultView(collection);
            return view.Dispatcher;
        }

        public static void Add_s<T>(this ObservableCollection<T> collection, T item)
        {
            collection.GetDispatcher().Invoke(() => collection.Add(item));
        }

        public static async Task RefreshAsync(this ObservableCollection<Server> collection)
        {
            await Task.WhenAll(collection.Select(item => item.Refresh()));
        }
    }
}