using System.Windows;
using NetControlCommon;
using NetControlServer.Classes;
using NetControlServer.Responses;

namespace NetControlServer.Controllers
{
    public class ApiController : IController
    {
        public IRequestResponse Version()
        {
            return new StringResponse("1.0");
        }

        public IRequestResponse PrtSc()
        {
            return new PngResponse(ScreenCapturer.Take());
        }
        public IRequestResponse PrtSc(string size)
        {
            var s = Size.Parse(size);
            return new PngResponse(ScreenCapturer.Take(s));
        }
    }
}
