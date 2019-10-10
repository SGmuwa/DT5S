using System;
using System.Collections.Generic;
using System.Text;

namespace Electra
{
    internal static class StaticTools
    {
        internal static string ToString(this object toInsert, int len)
                    => string.Format(string.Format("{{0, {0}}}", len), toInsert.ToString());
    }
}
