using NetControlClient.Responses;
using NetControlCommon;

namespace NetControlClient.Controllers
{
    class TestController : IController
    {
        public IRequestResponse Echo(string mes)
        {
            return new StringResponse(mes);
        }
    }
}
