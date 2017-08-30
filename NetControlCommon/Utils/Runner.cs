using System;
using System.Threading.Tasks;
using NetControlCommon.Properties;

namespace NetControlCommon.Utils
{
    public static class Runner
    {
        public static bool CatchWithMessage([NotNull]this Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception e)
            {
                DefaultCatch(e.ToString());
                return false;
            }
        }

        public static Action<string> DefaultCatch;

        public static bool CatchWithMessage<T>([NotNull]this Action<T> action, T param)
        {
            try
            {
                action.Invoke(param);
                return true;
            }
            catch (Exception e)
            {
                DefaultCatch(e.ToString());
                return false;
            }
        }

        public static async Task CatchWithMessageAsync([NotNull] this Task action)
        {
            try
            {
                await action;
            }
            catch (Exception e)
            {
                DefaultCatch(e.ToString());
            }
        }

        public static async Task<T> CatchWithMessageAsync<T>([NotNull]this Task<T> action)
        {
            try
            {
                return await action;
            }
            catch (Exception e)
            {
                DefaultCatch(e.ToString());
                return default(T);
            }
        }
        public static async Task<T> CatchAsync<T>([NotNull]this Task<T> action)
        {
            try
            {
                return await action;
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

    }
}
