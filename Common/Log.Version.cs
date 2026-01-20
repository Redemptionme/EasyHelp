#region FileInfo
// <summary>
// Author  hanhinhe
// Date    2026.01.20
// Desc
// </summary>
#endregion

namespace HHL.Common
{
    public partial class Log
    {
        private string m_version = "1.55";
        
        private void InitVersion()
        {
            switch (m_version)
            {
                case "1.53":
                    Init1Dot53();
                    break;
                case "1.54":
                    Init1Dot54();
                    break;
                case "1.55":
                    Init1Dot55();
                    break;
            }
        }

        private void Init1Dot55()
        {
            InitJigsaw();
            InitKsJigsaw();
        }


        private void Init1Dot54()
        {
            InitPacificRimProto();
        }


        private void Init1Dot53()
        {
            InitGiantWorldBoss();
        }
    }
}