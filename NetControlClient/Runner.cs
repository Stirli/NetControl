using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NetControlClient.Annotations;

namespace NetControlClient
{
    public static class Runner
    {
        public static bool IgnoreErr([NotNull]this Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool IgnoreErr<T>([NotNull]this Action<T> action, T param)
        {
            try
            {
                action.Invoke(param);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<T> IgnoreErrAsync<T>([NotNull]this Task<T> action)
        {
            try
            {
                return await action;
            }
            catch (Exception)
            {
                return default(T);
            }
        }
        public static void InMainDispatcher([NotNull] this Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}
