#region FileInfo

// <summary>
// Author  hanhinhe
// Date    2026.01.20
// Desc
// </summary>

#endregion

using System;
using HHL.Common;
using UnityEngine;

namespace IGG.Game.Game.HHL.Attr
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HHLKeyFunVerAttr : HHLFunVerAttr
    {
        public KeyCode KeyCode { get; set; }

        public HHLKeyFunVerAttr(Ver ver, KeyCode keyCode) : base(ver)
        {
            KeyCode = keyCode;
        }
    }
}