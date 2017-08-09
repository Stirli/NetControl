using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetControlCommon
{
    public class NotFoundResponse : IRequestResponse
    {
        public byte[] GetBytes()
        {
            return
            Encoding.UTF8.GetBytes(
                "<!DOCTYPE html>\r\n<html lang=\"ru\">\r\n    <head>\r\n        <title>Управление компьютером ученика</title>\r\n        <meta charset=\"UTF-8\">\r\n        <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n    </head>\r\n    <body>\r\n    <p style=\"color: red; font-size: 23vh; text-align:center\">ERROR 404</p>\r\n    </body>\r\n</html>");
        }

        public string ContentType
        {
            get { return "text/html; ; charset=utf-8"; }
        }
    }
}
