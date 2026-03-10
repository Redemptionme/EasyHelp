#region FileInfo

// <summary>
// Author  hanhinhe
// Date    2026.01.20
// Desc
// </summary>

#endregion

using System;
using System.Linq;
using System.Reflection;
using IGG.Framework.Panel;
using IGG.Game.Data.Cache.Activity;
using IGG.Game.Game.HHL.Attr;
using IGG.Game.Module.Activity.View;
using IGG.Game.Module.GiantBoss.View;
using UnityEngine;

namespace HHL.Common
{
    public partial class HHLGOTools
    {
        private void OnClick(KeyCode keyCode)
        {
            var method = GetType()
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(m =>
                {
                    var attr = m.GetCustomAttribute<HHLKeyFunVerAttr>();
                    return attr != null &&
                           attr.Version == Log.Inst.Version &&
                           attr.KeyCode == keyCode;
                });

            method?.Invoke(this, null);
        }


        [HHLKeyFunVerAttr(Ver.V1_53, KeyCode.F3)]
        private void OnF3Click1_53()
        {
            PanelMgr.Inst.OpenPanel<GiantBossPanel>();
        }

        [HHLKeyFunVerAttr(Ver.V1_55, KeyCode.F3)]
        private void OnF3Click1_55()
        {
            PanelMgr.Inst.OpenPanel<ActivityPanel>((uint)ActivityCache.PersonalActivityAdd + 70, true);
        }
    }
}