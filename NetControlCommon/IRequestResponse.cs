using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetControlCommon
{
    public interface IRequestResponse
    {
        byte[] GetBytes();
        string ContentType { get; }
    }
}
