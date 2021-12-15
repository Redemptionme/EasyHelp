using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HHL.Common
{
    public partial class Log : Singleton<Log>
    {
        private readonly List<eLogType> _typeList = new List<eLogType>();
        private string _filePath = "";
        private uint _outType = 0;
        private Queue<StringBuilder> m_sbPools = new Queue<StringBuilder>();
        
        public override void Init()
        {
            InitExtend();
        }

        public override void Dispose()
        {
            
        }

        public StringBuilder GetSb()
        {
            if (m_sbPools.Count > 1)
            {
                return m_sbPools.Dequeue();
            }

            return new StringBuilder(200);
        }

        public void RetrunSb(StringBuilder sb)
        {
            sb.Clear();
            m_sbPools.Enqueue(sb);
        }
            
        
        public void Print(object message, eLogType eType = eLogType.eLog, bool bAppend = true)
        {
            if (!_typeList.Contains(eType))
                return;

            StringBuilder preSb = GetSb();
            preSb.Append(DateTime.Now.ToString("HH:mm:ss:fff")).Append(" [").Append(eType.ToString()).Append("] ");
            
            if ((eLogOut.eUnity & (eLogOut)_outType) == eLogOut.eUnity)
            {
                PrintUnity(preSb,message);
            }
            
            if ((eLogOut.eFile & (eLogOut)_outType) == eLogOut.eFile)
            {
                WriteFile(preSb,message,bAppend);
            } 
            RetrunSb(preSb);
        }

        public void WriteFile(StringBuilder preSb,object message, bool bAppend = true)
        {
            // todo 多线程问题，写文件锁
            var sb = GetSb();
            sb.Append(preSb.ToString()).Append(message);

            if (sb.Length <= _fileTxtLen)
            {
                using (var sw = new StreamWriter(_filePath, bAppend)) { sw.WriteLine(sb.ToString()); }                    
            }
            else
            {
                int startIndex = 0;
                
                
                var tempSb = GetSb();
                for (int i = 0; i < preSb.Length; i++)
                {
                    tempSb.Append(' ');
                }
                
                while (startIndex < sb.Length)
                {
                    var t = GetSb();
                    if (startIndex != 0)
                        t.Append(tempSb.ToString());
                    int len = _fileTxtLen - t.Length;
                    
                    if (startIndex + len > sb.Length)
                    {
                        len = sb.Length - startIndex;
                    }
                    
                    t.Append(sb.ToString(startIndex, len));
                    using (var sw = new StreamWriter(_filePath, bAppend)) { sw.WriteLine(t.ToString()); }
                    startIndex += len;
                    RetrunSb(t);
                }
                
                RetrunSb(tempSb);
            }

            RetrunSb(sb);
        }
        
        public void PrintUnity(StringBuilder preSb,object message)
        {
            var sb = GetSb();
            sb.Append("<color=#").Append(UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.blue)).Append(">");
            sb.Append(preSb.ToString()).Append(message);
            sb.Append("</color>");
            UnityEngine.Debug.Log(sb.ToString());
            RetrunSb(sb);
        }

    }
}