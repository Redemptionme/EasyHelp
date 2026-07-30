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
        private Ver m_version = Ver.V1_60;

        public Ver Version => m_version;

        private void InitVersion()
        {
            var method = GetType()
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(m => m.GetCustomAttribute<HHLFunVerAttr>()?.Version == m_version);

            method?.Invoke(this, null);
        }

        [HHLFunVerAttr(Ver.V1_60)]
        private void Init1Dot60()
        {
            InitHeroEquip();
            InitPinBall();
        }

        [HHLFunVerAttr(Ver.V1_59)]
        private void Init1Dot59()
        {
        }

        [HHLFunVerAttr(Ver.V1_58)]
        private void Init1Dot58()
        {
            //InitHeroEquip();
            InitPoolLottery();
        }

        [HHLFunVerAttr(Ver.V1_57)]
        private void Init1Dot57()
        {
            InitNewFixTurntable();
            InitAchievementProto();
            InitBattleRoyale();
            InitPacificRimProto();
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