using NetControlCommon.Interfaces;
using NetControlCommon.StandartResponses;
using NetControlCommon.Utils;
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
            if (Size.TryParse(size, out var s))
                return new PngResponse(ScreenCapturer.Take(s.w, s.h));
            return new PngResponse(ScreenCapturer.Take());
        }
    }
}