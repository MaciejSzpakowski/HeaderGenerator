using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeaderGenerator
{
    public static class Util
    {
        public static bool StartsWith(this String str, string value)
        {
            return str.IndexOf(value) == 0;
        }

        public static string Pop(this string str, int n)
        {
            return str.Substring(0, str.Length - n);
        }

        public static int _IndexOf(this string str, string value, int startAt, int count)
        {
            try
            {
                return str.IndexOf(value, startAt, count);
            }
            catch (ArgumentOutOfRangeException)
            {
                return -1;
            }
        }
    }
}
