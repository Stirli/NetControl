using NetControlCommon;
using NetControlCommon.Interfaces;
using NetControlServer.Responses;

namespace NetControlServer.Controllers
{
    class TestController : IController
    {
        public IRequestResponse Echo(string mes)
        {
            return new StringResponse(mes);
        }
    }
}
