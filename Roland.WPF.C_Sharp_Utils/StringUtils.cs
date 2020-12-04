using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roland.WPF.C_Sharp_Utils
{
    public static class StringUtils
    {

        /// <summary>
        ///     Retrieve all indexes of substring
        /// </summary>
        /// <param name="str"></param>
        /// <param name="subStr"></param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
        public static int[] AllIndexesOf(this string str, string subStr, StringComparison stringComparison)
        {
            if (string.IsNullOrEmpty(subStr))
            {
                throw new ArgumentException("Substring is not specified");
            }

            List<int> _indexes = new List<int>();
            int _index = 0;

            while(
                (_index =
                    str.IndexOf(
                        subStr,
                        _index,
                        stringComparison)) != -1)
            {
                _indexes.Add(_index++);
            }
            return _indexes.ToArray();
        }
    }
}
