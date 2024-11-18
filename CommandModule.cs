using IGG.Framework;
using IGG.Framework.Module;
using System.IO;
using System.Reflection;
using Logger = IGG.Framework.Logging.Logger;

namespace IGG.Game.Module.Collect
{
    /// <summary>
    /// Author  jinaghaiwu
    /// Date    2024.9.12
    /// Desc    Command模块 用于执行热更新的代码（方便不重新编译就可以测试部分代码）
    /// </summary>
    public partial class CommandModule : BaseModule<CommandModule>
    {
        public override string Id
        {
            get { return "Command"; }
        }

        private Assembly m_hotfixAssembly;
        /// <summary>
        /// 使用的logger
        /// </summary>
        /// <returns></returns>
        protected override Framework.Logging.ILogger CreateLogger()
        {
            return Logger.Default;
        }

        protected override void OnInit()
        {
            if (FrameworkConfig.Inst.DebugLevel != DebugLevel.Disable)
            {
                FairyGUI.Timers.inst.Add(0.0f, -1, OnUpdate);
            }
        }

        void OnUpdate(object obj)
        {
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F6))
            {
                if (m_hotfixAssembly != null)
                {
                    try
                    {
                        var type = m_hotfixAssembly.GetType("dls.hotfix.Game");
                        var metho = type.GetMethod("Shut", BindingFlags.Static | BindingFlags.Public);
                        metho.Invoke(null, null);
                    }
                    finally
                    {
                        m_hotfixAssembly = null;
                    }
                }
                else
                {
#if UNITY_EDITOR
                    var rootDir = "./Tools/dls.hotfix/bin/";
#else
                    rootDir = Application.persistentDataPath + "/hotfix";
#endif
                    var dllPath = Path.Combine(rootDir, "dls.hotfix.dll");

                    if (File.Exists(dllPath))
                    {
                        var bytes = File.ReadAllBytes(dllPath);
                        var pdb_bytes = File.ReadAllBytes(Path.Combine(rootDir, "dls.hotfix.pdb"));
                        m_hotfixAssembly = Assembly.Load(bytes, pdb_bytes);
                        var type = m_hotfixAssembly.GetType("dls.hotfix.Game");
                        var metho = type.GetMethod("Init", BindingFlags.Static | BindingFlags.Public);
                        metho.Invoke(null, null);
                    }
                }
            }
        }
        protected override void OnDispose()
        {
        }
    }
}