#region FileInfo
// <summary>
// Author  hanlinhe
// Date    2022.01.20
// Desc
// </summary>
#endregion

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using IGG.Framework.EditorTools.Module;
using IGG.Game.Managers.Network;
using UnityEditor;
using UnityEngine;

namespace Game.HHL.Editor
{
    public class ModuleEditorWindow:EditorWindow
    {
        private string m_moduleName = "Test";
        public string[] panelOptions = new string[]{"Normal","ActivityView","PersonalActivityView","CompView"};
        private int m_curPanelIndex = 0;
        private string m_panelName = "TestPanel";
        private string m_authorName = "hanlinhe";
        private static bool m_bMute;
        
        [MenuItem("HHL/音量开关")]
        public static void WwiseMute()
        {
            var audioListener = FindObjectOfType<AkAudioListener>();
            audioListener.enabled = m_bMute;
            m_bMute = !m_bMute;
        }
        
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

        // [MenuItem("HHL/Proto更新并生成")]
        // public static void GenProto()
        // {
        //     ProtoTool.GenProto();
        // }

        [MenuItem("HHL/网络测试")]
        public static void NetworkDebugPanelShow()
        {
            NetworkDebugPanel.Show();
        }
        [MenuItem("HHL/打开LOG")]
        public static void OpenLog()
        {
            var winToolsPath = "C:/Program Files/Microsoft VS Code/Code.exe";
            var uiProjectPath = Application.dataPath.Replace("Assets","output//HHL.log");
            Process.Start(winToolsPath, uiProjectPath);
        }
        [MenuItem("HHL/打开string_cn表")]
        public static void OpenStringCN()
        {
            var winToolsPath = "C:/Program Files/Microsoft VS Code/Code.exe";
            var uiProjectPath = Application.dataPath.Replace("Assets","Assets/Config/string_cn.csv");
            Process.Start(winToolsPath, uiProjectPath);
        }
        [MenuItem("HHL/打开message表")]
        public static void OpenMessage()
        {
            var winToolsPath = "C:/Program Files/Microsoft VS Code/Code.exe";
            var uiProjectPath = Application.dataPath.Replace("Assets","Assets/Config/message.csv");
            Process.Start(winToolsPath, uiProjectPath);
        }

        private void OnGUI()
        {
            var basePath = Application.dataPath.Replace("Assets","Assets//Scripts//Game//Module");
            
            GUILayout.Label ("基础设置", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            m_moduleName = EditorGUILayout.TextField ("基础名", m_moduleName);
        
            //m_panelName = m_moduleName + "Panel";
            //EditorGUILayout.BeginHorizontal();
            m_panelName = EditorGUILayout.TextField ("界面名字", m_panelName);
            //EditorGUILayout.EndToggleGroup ();
            
            
            m_authorName = EditorGUILayout.TextField ("作者名", m_authorName);
           
            m_curPanelIndex = EditorGUILayout.Popup("界面类型",m_curPanelIndex,panelOptions);
            
            if (GUILayout.Button("生成代码"))
            {
                var moduleDir = basePath + "//" +m_moduleName;
                if (!Directory.Exists(moduleDir))
                {
                    Directory.CreateDirectory(moduleDir);
                }
                
                var panelDir = moduleDir + "//View";

                switch (m_curPanelIndex)
                {
                    case 0:
                        CreatePanel(panelDir,m_panelName,m_moduleName);
                        break;
                    case 1:
                        CreateActivityView(panelDir,m_panelName,m_moduleName);
                        break;
                    case 2:
                        break;
                    case 3:
                        CreateCompView(panelDir,m_panelName,m_moduleName);
                        break;
                }
            };
            GUIUtility.ExitGUI();
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
        
        private void CreateActivityView(string panelDir,string panelName,string moduleName)
        {
            if (!Directory.Exists(panelDir))
            {
                Directory.CreateDirectory(panelDir);
            }
            var outputFileName = panelDir + "//" + panelName + ".cs";
            var codeTemplateFileName = Application.dataPath.Replace("Assets","Assets//Scripts/Game//HHL//GenCode/ActivityViewTemplate.txt");
            var contentTxt= File.ReadAllText(codeTemplateFileName, Encoding.UTF8);
            var sb = new StringBuilder(contentTxt);
            sb.Replace("__PERSON_NAME__", m_authorName);
            sb.Replace("__DATA_TABLE_CREATE_TIME__", DateTime.UtcNow.ToLocalTime().ToString("yyyy.MM.dd"));
            sb.Replace("__MODULE_NAME__", m_moduleName);
            sb.Replace("__PANEL_NAME__", panelName);
       
            EditorHelper.WriteFile(outputFileName,sb.ToString());
        }
        
        private void CreateCompView(string panelDir,string panelName,string moduleName)
        {
            if (!Directory.Exists(panelDir))
            {
                Directory.CreateDirectory(panelDir);
            }
            var outputFileName = panelDir + "//" + panelName + ".cs";
            var codeTemplateFileName = Application.dataPath.Replace("Assets","Assets//Scripts/Game//HHL//GenCode/CompView.txt");
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