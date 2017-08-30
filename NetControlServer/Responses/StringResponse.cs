using System.Text;
using NetControlCommon;
using NetControlCommon.Interfaces;

namespace NetControlServer.Responses
{
    public class StringResponse:IRequestResponse
    {
        private string str;

        public StringResponse(string str)
        {
            this.str = str;
        }

        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public string ContentType => "text/plain; charset=utf-8";

        public static implicit operator StringResponse(string opstr)
        {
            return new StringResponse(opstr);
        }
    }
}
