using System.Text;
using NetControlCommon.Interfaces;

namespace NetControlCommon.StandartResponses
{
    public class NotFoundResponse : IRequestResponse
    {
        public byte[] GetBytes()
        {
            return
                Encoding.UTF8.GetBytes(
                    "<!DOCTYPE html>\r\n<html lang=\"ru\">\r\n    <head>\r\n        <title>Управление компьютером ученика</title>\r\n        <meta charset=\"UTF-8\">\r\n        <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n    </head>\r\n    <body>\r\n    <p style=\"color: red; font-size: 23vh; text-align:center\">ERROR 404</p>\r\n    </body>\r\n</html>");
        }

        public string ContentType => "text/html; ; charset=utf-8";
    }
}