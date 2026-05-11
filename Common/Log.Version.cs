#region FileInfo

// <summary>
// Author  hanhinhe
// Date    2026.01.20
// Desc
// </summary>

#endregion

using System.Linq;
using System.Reflection;
using IGG.Game.Game.HHL.Attr;

namespace HHL.Common
{
    public partial class Log
    {
        private Ver m_version = Ver.V1_58;

        public Ver Version => m_version;

        private void InitVersion()
        {
            var method = GetType()
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(m => m.GetCustomAttribute<HHLFunVerAttr>()?.Version == m_version);

            method?.Invoke(this, null);
        }

        [HHLFunVerAttr(Ver.V1_58)]
        private void Init1Dot58()
        {
            InitTroopEquipProto();
        }

        [HHLFunVerAttr(Ver.V1_57)]
        private void Init1Dot57()
        {
            InitNewFixTurntable();
            InitAchievementProto();
            InitBattleRoyale();
        }

        [HHLFunVerAttr(Ver.V1_56)]
        private void Init1Dot56()
        {
            InitHeroEquip();
            InitSheepProto();
        }

        [HHLFunVerAttr(Ver.V1_55)]
        private void Init1Dot55()
        {
            InitJigsaw();
            InitKsJigsaw();
        }

        [HHLFunVerAttr(Ver.V1_54)]
        private void Init1Dot54()
        {
            InitPacificRimProto();
        }


        [HHLFunVerAttr(Ver.V1_53)]
        private void Init1Dot53()
        {
            InitGiantWorldBoss();
        }
    }
}