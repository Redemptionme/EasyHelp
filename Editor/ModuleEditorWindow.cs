#region FileInfo
// <summary>
// Author  hanlinhe
// Date    2022.01.20
// Desc
// </summary>
#endregion

using System;
using System.IO;
using System.Text;
using IGG.EditorTools;
using IGG.Framework.EditorTools.Module;
using IGG.Game.Managers.Network;
using UnityEditor;
using UnityEngine;

namespace Game.HHL.Editor
{
    public class ModuleEditorWindow:EditorWindow
    {
        private string m_moduleName = "Test";
        private bool m_bNeedPanelGroup = true;
        private string m_panelName = "TestPanel";
        private string m_authorName = "hanlinhe";
        
        [MenuItem("HHL/BC模块")]
        public static void OpenBC()
        {
            ModuleCodeBuildWnd wnd = EditorWindow.GetWindow<ModuleCodeBuildWnd>("模块代码生成工具");
            wnd.minSize = new Vector2(200, 250);
        }
        
        [MenuItem("HHL/生成代码")]
        public static void OpenUiEditor()
        {
            ModuleEditorWindow wnd = EditorWindow.GetWindow<ModuleEditorWindow>("模块代码生成工具");
            wnd.minSize = new Vector2(300, 200);
        }

        [MenuItem("HHL/Proto更新并生成")]
        public static void GenProto()
        {
            ProtoTool.GenProto();
        }

        [MenuItem("HHL/网络测试")]
        public static void NetworkDebugPanelShow()
        {
            NetworkDebugPanel.Show();
        }

        private void OnGUI()
        {
            var basePath = Application.dataPath.Replace("Assets","Assets//Scripts//Game//Module");
            
            GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
            m_moduleName = EditorGUILayout.TextField ("基础名", m_moduleName);
            m_bNeedPanelGroup= EditorGUILayout.BeginToggleGroup ("生成界面", m_bNeedPanelGroup);
            //m_panelName = m_moduleName + "Panel";
            m_panelName = EditorGUILayout.TextField ("界面名字", m_panelName);
            EditorGUILayout.EndToggleGroup ();
            m_authorName = EditorGUILayout.TextField ("作者名", m_authorName);
           

            if (GUILayout.Button("生成代码"))
            {
                var moduleDir = basePath + "//" +m_moduleName;
                if (!Directory.Exists(moduleDir))
                {
                    Directory.CreateDirectory(moduleDir);
                }
                
                var panelDir = moduleDir + "//View";
                if (m_bNeedPanelGroup)
                {
                    CreatePanel(panelDir,m_panelName,m_moduleName);
                }
            };
        }

        private void CreatePanel(string panelDir,string panelName,string moduleName)
        {
            if (!Directory.Exists(panelDir))
            {
                Directory.CreateDirectory(panelDir);
            }
            var outputFileName = panelDir + "//" + panelName + ".cs";
            var codeTemplateFileName = Application.dataPath.Replace("Assets","Assets//Scripts/Game//HHL//GenCode/PanelCodeTemplate.txt");
            var contentTxt= File.ReadAllText(codeTemplateFileName, Encoding.UTF8);
            var sb = new StringBuilder(contentTxt);
            sb.Replace("__PERSON_NAME__", m_authorName);
            sb.Replace("__DATA_TABLE_CREATE_TIME__", DateTime.UtcNow.ToLocalTime().ToString("yyyy.MM.dd"));
            sb.Replace("__MODULE_NAME__", m_moduleName);
            sb.Replace("__PANEL_NAME__", panelName);
       
            EditorHelper.WriteFile(outputFileName,sb.ToString());
        }
    }
}