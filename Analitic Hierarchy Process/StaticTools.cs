using System;
using System.Text;

namespace Analitic_Hierarchy_Process
{
    public static class Analitic_Hierarchy_Process
    {
        internal static string ToString(this object toInsert, int len)
                    => string.Format(string.Format("{{0, {0}}}", len), toInsert.ToString());

        internal static string TableToString(this Array array)
        {
            if (array.Rank != 2)
                throw new NotSupportedException();
            int max = -1;
            foreach (var a in array)
                if (a.ToString().Length > max)
                    max = a.ToString().Length;
            max++;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    sb.Append(array.GetValue(i, j).ToString(max));
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }
    }
}
