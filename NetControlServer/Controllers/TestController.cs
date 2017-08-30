using NetControlCommon.Interfaces;
using NetControlServer.Responses;

namespace NetControlServer.Controllers
{
    internal class TestController : IController
    {
        public IRequestResponse Echo(string mes)
        {
            return new StringResponse(mes);
        }
    }
}