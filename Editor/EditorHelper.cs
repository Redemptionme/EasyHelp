#region FileInfo
// <summary>
// Author  hanlinhe
// Date    2022.01.20
// Desc
// </summary>
#endregion

using System.IO;
using System.Text;
using UnityEngine;

namespace Game.HHL.Editor
{
    public class EditorHelper
    {
        public static void WriteFile(string outputFileName,string sb)
        {
            using (FileStream fileStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter stream = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    stream.Write(sb);
                    Debug.Log("生成文件" + outputFileName);
                }
            }
        }
    }
}