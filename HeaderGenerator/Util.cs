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

        /// <summary>
        /// IndexOf will throw exception if trying to match a string near the end so I need this wrapper
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <param name="startAt"></param>
        /// <param name="count"></param>
        /// <returns></returns>
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
