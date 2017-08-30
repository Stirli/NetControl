using NetControlCommon.Interfaces;
using NetControlCommon.StandartResponses;
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
            int width;
            int height;
            var arr = size.Split(',');
            if (arr.Length != 2) return new NotFoundResponse();
            int.TryParse(arr[0], out width);
            int.TryParse(arr[1], out height);
            return new PngResponse(ScreenCapturer.Take(width, height));
        }
    }
}