using System;
using System.IO;
using System.Text;

namespace EasyLog
{
    [Flags]
    public enum LogType : ushort
    {
        None = 0,
        Debug = 1 << 0,
        Warning = 1 << 2,
        Err = 1 << 3,
        All = ushort.MaxValue
    }

    public class EasyFileLog
    {
        private EasyFileLog() { }

        private static EasyFileLog m_inst;
        public static EasyFileLog Inst => m_inst ??= new EasyFileLog();

        private string m_filePath = string.Empty;
        private bool m_bFirst = true;
        private StringBuilder m_sb = new();

        public string Path
        {
            get
            {
                if (m_filePath == string.Empty)
                {
                    m_filePath = Environment.CurrentDirectory.Replace("\\", "/") + "/output/HHL.log";
                }
                return m_filePath;
            }
            set => m_filePath = value;
        }

        public void Log(LogType logType, object message, string tag = "")
        {
            if (m_bFirst)
            {
                LogTitle();
                m_bFirst = false;
            }
            WriteFile(Path, Format(logType, message?.ToString() ?? string.Empty, tag), true);
        }

        public void Log(object message)
        {
            Log(LogType.Debug, message);
        }

        public void LogWarning(object message)
        {
            Log(LogType.Warning, message);
        }

        public void LogErr(object message)
        {
            Log(LogType.Err, message);
        }

        private void LogTitle()
        {
            WriteFile(Path, $"------[Title]{DateTime.Now:yyyy-M-d HH:mm:ss:fff}------", false);
        }

        public void WriteDirectory(string filePath)
        {
            FileInfo fileInfo = new(filePath);
            DirectoryInfo? directoryInfo = fileInfo.Directory;
            if (directoryInfo is { Exists: false })
            {
                directoryInfo.Create();
            }
        }

        public void WriteFile(string filePath, string str, bool bAppend = true)
        {
            WriteDirectory(filePath);
            try
            {
                using StreamWriter sw = new(filePath, bAppend);
                sw.WriteLine(str);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public string Format(LogType logType, string message, string tag = "")
        {
            DateTime now = DateTime.Now;
            m_sb.Clear();
            m_sb.Append("[").Append(now.ToString("yyyy-M-d HH:mm:ss:fff")).Append("] ");
            m_sb.Append("[");
            switch (logType)
            {
                case LogType.Debug: m_sb.Append("Debug"); break;
                case LogType.Warning: m_sb.Append("Warning"); break;
                case LogType.Err: m_sb.Append("Err"); break;
                default: m_sb.Append(logType.ToString()); break;
            }
            m_sb.Append("]");
            if (!string.IsNullOrEmpty(tag))
            {
                m_sb.Append("[").Append(tag).Append("] ");
            }
            if (!string.IsNullOrEmpty(message))
            {
                m_sb.Append(message);
            }
            return m_sb.ToString();
        }
    }
}