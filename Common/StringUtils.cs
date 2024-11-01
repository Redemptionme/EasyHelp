#region FileInfo

// <summary>
// Author  hanhinhe
// Date    2024.10.24
// Desc
// </summary>

#endregion

using System;

namespace HHL.Common
{
    public static class StringUtils
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}