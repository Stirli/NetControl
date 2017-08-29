using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetControlClient
{
    public static class RequestHelpers
    {
        public static async Task<WebResponse> GetResponseAsyncUnsafe(this WebRequest req)
        {
            try
            {
                return await req.GetResponseAsync();
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }
    }
}
