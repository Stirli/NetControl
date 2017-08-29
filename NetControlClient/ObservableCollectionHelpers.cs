using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;

namespace NetControlClient
{
    public static class ObservableCollectionHelpers
    {
        public static Dispatcher GetDispatcher<T>(this ObservableCollection<T> collection)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(collection);
            return view.Dispatcher;
        }

        public static void Add_s<T>(this ObservableCollection<T> collection, T item)
        {
            collection.GetDispatcher().Invoke(() => collection.Add(item));
        }
        public static void Refresh(this ObservableCollection<Server> collection)
        {
            collection.GetDispatcher().Invoke(async () =>
            {
                foreach (var server in collection)
                {
                    await server.Refresh();
                }
            });
        }
    }
}
