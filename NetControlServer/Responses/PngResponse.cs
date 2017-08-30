using NetControlCommon;
using NetControlCommon.Interfaces;

namespace NetControlServer.Responses
{
    class PngResponse:IRequestResponse
    {
        private readonly byte[] _pngBytes;

        public PngResponse(byte[] pngBytes)
        {
            _pngBytes = pngBytes;
        }

        public string ContentType => "image/png";
        public byte[] GetBytes()
        {
            return _pngBytes;
        }
    }
}
