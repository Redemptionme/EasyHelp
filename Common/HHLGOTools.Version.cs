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
using IGG.Game.Module.Activity;
using IGG.Game.Module.Activity.View;
using IGG.Game.Module.GiantBoss.View;
using IGG.Game.Module.Hero;
using IGG.Game.Module.Hero.View;
using IGG.Game.Module.Hero.View.HeroEquip;
using IGG.Game.Module.Kof.View;
using IGG.Game.Module.TroopEquip.View;
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

        [HHLKeyFunVerAttr(Ver.V1_56, KeyCode.F7)]
        private void OnF7Click_56()
        {
            //HeroModule.Inst.OpenHeroEquipDevelopPanel();
            //PanelMgr.Inst.OpenPanel<HeroEquipDevelopPanel>(ActivityModule.Inst.SheepActId);
        }
        
        [HHLKeyFunVerAttr(Ver.V1_56, KeyCode.F4)]
        private void OnF4Click_56()
        {
            PanelMgr.Inst.OpenPanel<Match2MainPanel>(ActivityModule.Inst.SheepActId);
        }

        [HHLKeyFunVerAttr(Ver.V1_56, KeyCode.F3)]
        private void OnF3Click_56()
        {
            PanelMgr.Inst.OpenPanel<HeroEquipRenovationPanel>(1);
        }

        [HHLKeyFunVerAttr(Ver.V1_55, KeyCode.F4)]
        private void OnF4Click_55()
        {
            PanelMgr.Inst.OpenPanel<TroopsEquipBagPanel>();
        }


        [HHLKeyFunVerAttr(Ver.V1_55, KeyCode.F3)]
        private void OnF3Click_55()
        {
            PanelMgr.Inst.OpenPanel<ActivityPanel>((uint)ActivityCache.PersonalActivityAdd + 70, true);
        }


        [HHLKeyFunVerAttr(Ver.V1_53, KeyCode.F3)]
        private void OnF3Click_53()
        {
            PanelMgr.Inst.OpenPanel<GiantBossPanel>();
        }
    }
}