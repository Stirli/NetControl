using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetControlServer
{
    public class Client
    {
        public Client(IPAddress host)
        {
            Host = host;
        }

        public IPAddress Host { get;}

        public bool IsOnline
        {
            get
            {
                WebRequest wr = WebRequest.CreateHttp("http://" + Host + ":8080/test/echo?mes=message");
                WebResponse resp = null;
                try
                {
                    resp = wr.GetResponseAsync().Result;
                }
                catch (Exception ex)
                {
                    // ignored
                    var s = ex.Message;
                }
                var str = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                resp.Close();
                return str.Equals("message");
            }
        }
    }
}
