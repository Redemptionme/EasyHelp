#region FileInfo

// <summary>
// Author  hanhinhe
// Date    2026.01.20
// Desc
// </summary>

#endregion

using System;
using HHL.Common;

namespace IGG.Game.Game.HHL.Attr
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HHLFunVerAttr : Attribute
    {
        public Ver Version { get; private set; }

        public HHLFunVerAttr(Ver ver)
        {
            Version = ver;
        }
    }
}