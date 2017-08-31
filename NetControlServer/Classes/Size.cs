using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetControlCommon.Utils;

namespace NetControlServer.Classes
{
    public struct Size
    {
        public int w;
        public int h;

        public Size(int width, int height)
        {
            w = width;
            h = height;
        }

        public static bool TryParse(string str, out Size value)
        {
            var arr = str.Split(',');
            if (arr.Length == 2)
                if (int.TryParse(arr[0], out var width))
                    if (int.TryParse(arr[1], out var height))
                    {
                        value = new Size(width, height);
                        return true;
                    }
            value = default(Size);
            return false;
        }

        public static implicit operator System.Drawing.Size(Size size)
        {
            return new System.Drawing.Size(size.w, size.h);
        }

        public static implicit operator System.Windows.Size(Size size)
        {
            return new System.Windows.Size(size.w, size.h);
        }
    }
}
