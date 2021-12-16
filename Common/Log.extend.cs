using System;
using System.Text;

namespace HHL.Common
{
    public partial class Log
    {
        // 写文件单行字符长度,stringbuilder长度现在默认是200,所以别超过200
        private int _fileTxtLen = 120;
        private bool _allMsg = false;
        
         public enum eLogType
         {
             eLog = 0,
             eWarning,
             eErro,
             eRootNetwork,
         }
         
         public enum eLogOut
         {
             eNone = 0,
             eFile = 1,
             eUnity = 1<<1,
         }

         private void InitExtend()
         {
             //_filePath = System.Environment.CurrentDirectory + "\\Assets\\Scripts\\Game\\HHL" + "\\HHL.log";
             _filePath = System.Environment.CurrentDirectory + "\\output" + "\\HHL.log";
             
             _typeList.Add(eLogType.eLog);
             _typeList.Add(eLogType.eWarning);
             _typeList.Add(eLogType.eErro);
             _typeList.Add(eLogType.eRootNetwork);

             _outType |= (uint) eLogOut.eFile ;
             //_outType |= (uint) eLogOut.eUnity ;

             //InitMsgListen();
             
             var strFu = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
             Print("", eLogType.eLog, false);
             Print("-----HHL Log----------" + strFu + "--------------");
         }
         
         public void PrintUnity(StringBuilder preSb,object message)
         {
             // var sb = GetSb();
             // sb.Append("<color=#").Append(UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.blue)).Append(">");
             // sb.Append(preSb.ToString()).Append(message);
             // sb.Append("</color>");
             // UnityEngine.Debug.Log(sb.ToString());
             // RetrunSb(sb);
         }
          
    }
}