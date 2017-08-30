using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetControlClient.Classes
{
    public partial class Server
    {
        public async Task Refresh()
        {
            try
            {
                var isOn = await CheckOnline();
                //OnPropertyChanged(nameof(IsOnline));
                if (isOn)
                    await UpdateBackBuffer();
                IsOnline = isOn;
            }
            catch (WebException e)
            {
                var ee = e;
            }

        }

        public async Task Shutdown()
        {

        }
    }
}
