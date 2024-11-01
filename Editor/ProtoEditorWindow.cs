#region FileInfo

// <summary>
// Author  hanhinhe
// Date    2024.07.12
// Desc
// </summary>

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IGG.Framework.Extend;
using UnityEditor;
using UnityEngine;

namespace Game.HHL.Editor
{
    public class ProtoEditorWindow : EditorWindow
    {
        private string m_moduleName = "Test";
        private string m_authorName = "hanlinhe";


        private Vector2 m_protoScrollPos;
        private string m_protoStr;

        private Vector2 m_outPutBigScroll;

        private Vector2 m_outputDebugScroll;
        private string m_outputDebugStr;

        private Vector2 m_outputModuleScroll1;
        private string m_outputModuleStr1;

        private Vector2 m_outputModuleScroll2;
        private string m_outputModuleStr2;

        private Vector2 m_outputCacheScroll;
        private string m_outputCacheStr;

        private Vector2 m_outputNotifyScroll;
        private string m_outputNotifyStr;

        private List<ProtoStruct> m_infos = new();

        [MenuItem("HHL/生成Proto处理相关")]
        public static void OpenProtoEditor()
        {
            var wnd = GetWindow<ProtoEditorWindow>("Proto消息生成工具");
            wnd.minSize = new Vector2(800, 600);
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            //GUILayout.Label("基础设置", EditorStyles.boldLabel);
            m_moduleName = EditorGUILayout.TextField("基础名", m_moduleName);
            m_authorName = EditorGUILayout.TextField("作者名", m_authorName);

            var bc = GUI.backgroundColor;
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            GUILayout.Label("proto结构",
                EditorStyles.boldLabel);
            m_protoScrollPos = EditorGUILayout.BeginScrollView(m_protoScrollPos, GUILayout.Height(200));
            m_protoStr = EditorGUILayout.TextArea(m_protoStr);
            EditorGUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("生成Proto消息代码", GUILayout.Width(200)))
            {
                m_infos.Clear();
                using (var strReader = new StringReader(m_protoStr))
                {
                    var Annotation = "";
                    List<string> TempList = new();
                    ProtoStruct info = null;
                    var depth = 0;

                    while (true)
                    {
                        var line = strReader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }

                        line = line.Replace("\t", " ");

                        if (line == "")
                        {
                            continue;
                        }

                        if (line == " ")
                        {
                            continue;
                        }

                        if (line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Length == 0)
                        {
                            continue;
                        }

                        if (line.StartsWith("//"))
                        {
                            Annotation = line;
                            continue;
                        }
                        else if (line.StartsWith(TypeNameHelper.Message) &&
                                 line.Contains(TypeNameHelper.Msg))
                        {
                            info = new ProtoStruct
                            {
                                Annotation = Annotation
                            };
                            var sublines = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                            if (sublines.Length > 1)
                            {
                                info.ClassName = sublines[1];
                                m_infos.Add(info);
                                Annotation = "";
                                depth = 0;
                                continue;
                            }
                        }
                        else if (line.Contains('{'))
                        {
                            depth++;
                            continue;
                        }
                        else if (line.Contains('}'))
                        {
                            depth--;
                            continue;
                        }
                        else if (depth > 1)
                        {
                            // 嵌套后面再做
                            continue;
                        }
                        else
                        {
                            var pos = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            var equalIndex = 0;
                            var isRepeated = false;
                            for (var i = 0; i < pos.Length; i++)
                            {
                                if (i == 0 && pos[i] == "repeated")
                                {
                                    isRepeated = true;
                                }

                                if (pos[i] == "=")
                                {
                                    equalIndex = i;
                                    break;
                                }
                            }

                            var st = new ProtoStruct
                            {
                                FieldName = pos[equalIndex - 1],
                                TypeName = TypeNameHelper.GetClientName(pos[equalIndex - 2], isRepeated)
                            };

                            info.Childs.Add(st);
                        }
                    }
                }

                var sb = new StringBuilder();
                sb.Append("public void Init").Append(m_moduleName).Append("Proto()").AppendLine().Append("{")
                    .AppendLine();
                foreach (var info in m_infos)
                {
                    var enumName = $"k{info.ClassName}".ToLowerInvariant();
                    foreach (var msgtype in Enum.GetValues(typeof(Msgtype.MsgType)))
                    {
                        if (enumName == msgtype.ToString().ToLowerInvariant())
                        {
                            enumName = msgtype.ToString();
                            break;
                        }
                    }

                    sb.Append("    AddListenMsgType(MsgType.").Append(enumName).Append(");").AppendLine();
                }

                sb.Append("}");
                m_outputDebugStr = sb.ToString();

                sb.Clear();
                foreach (var info in m_infos)
                {
                    if (info.ProtoType == EProtoType.Request)
                    {
                        continue;
                    }

                    sb.Append("AddMsgListener<").Append(info.ClassName).Append(">(On").Append(info.ClassName)
                        .Append(");").AppendLine();
                }

                m_outputModuleStr1 = sb.ToString();

                sb.Clear();
                foreach (var info in m_infos)
                {
                    switch (info.ProtoType)
                    {
                        case EProtoType.None:
                            break;
                        case EProtoType.Notice:
                            sb.Append("        ").Append(info.Annotation).AppendLine();
                            sb.Append("        ").Append("public void On").Append(info.ClassName).Append("(")
                                .Append(info.ClassName)
                                .Append(" obj)").AppendLine();
                            sb.Append("        ").Append("{").AppendLine();
                            //sb.Append("        ").Append("    //todo hhl").AppendLine();
                            sb.Append("        ").Append("    ").Append("// AppCache.").Append(m_moduleName).Append(";")
                                .AppendLine();
                            sb.Append("        ").Append("    ").Append("SendNotify(").Append(m_moduleName)
                                .Append("Notify.").Append(info.FuncName).Append(");").AppendLine();
                            sb.Append("        ").Append("}").AppendLine();
                            break;
                        case EProtoType.Request:
                            sb.Append("        ").Append(info.Annotation).AppendLine();
                            sb.Append("        ").Append("public void Send").Append(info.ClassName).Append("(");
                            for (var i = 0; i < info.Childs.Count; i++)
                            {
                                var proto = info.Childs[i];
                                sb.Append(proto.TypeName).Append(" ").Append(proto.ParamName);
                                if (i != info.Childs.Count - 1)
                                {
                                    sb.Append(",");
                                }
                            }

                            sb.Append(")").AppendLine();
                            sb.Append("        ").Append("{").AppendLine();
                            sb.Append("        ").Append("    var msg = new ").Append(info.ClassName).Append("()");
                            if (info.Childs.Count == 0)
                            {
                                sb.Append("        ").Append("{};");
                            }
                            else
                            {
                                sb.AppendLine().Append("        ").Append("    {").AppendLine();
                                for (var i = 0; i < info.Childs.Count; i++)
                                {
                                    var childInfo = info.Childs[i];
                                    sb.Append("        ").Append("        ").Append(childInfo.FieldName).Append(" = ")
                                        .Append(childInfo.ParamName).Append(",").AppendLine();
                                }

                                sb.Append("        ").Append("    };");
                            }
                            sb.AppendLine();
                            sb.Append("        ").Append("    SendMsg(msg);").AppendLine();
                            sb.Append("        ").Append("}").AppendLine();
                            break;
                        case EProtoType.Reply:
                            sb.Append("        ").Append(info.Annotation).AppendLine();
                            sb.Append("        ").Append("public void On").Append(info.ClassName).Append("(")
                                .Append(info.ClassName)
                                .Append(" obj)").AppendLine();
                            sb.Append("        ").Append("{").AppendLine();
                            sb.Append("        ").Append("    ")
                                .Append("if ((ErrorCode)obj.ErrorCode != ErrorCode.KEcsuccess)")
                                .AppendLine();
                            sb.Append("        ").Append("    {").AppendLine();
                            sb.Append("        ").Append("        ")
                                .Append("ErrorHelper.ShowError((int)obj.ErrorCode);").AppendLine();
                            sb.Append("        ").Append("        ").Append("return;").AppendLine();
                            sb.Append("        ").Append("    }").AppendLine();
                            //sb.Append("        ").Append("    // todo hhl;").AppendLine();
                            //sb.Append("        ").Append("    ").Append("AppCache.").Append(m_moduleName).Append(";").AppendLine();
                            sb.Append("        ").Append("    ").Append("SendNotify(").Append(m_moduleName)
                                .Append("Notify.").Append(info.FuncName).Append(")").Append(";")
                                .AppendLine();
                            sb.Append("        ").Append("}").AppendLine();
                            break;
                        default:
                            break;
                            ;
                    }
                }

                m_outputModuleStr2 = sb.ToString();

                sb.Clear();
                foreach (var info in m_infos)
                {
                    if (info.ProtoType == EProtoType.Request)
                    {
                        continue;
                    }

                    sb.Append("public static string ").Append(info.FuncName).Append(" = ").Append('\"')
                        .Append(m_moduleName)
                        .Append("Notify_").Append(info.FuncName).Append("\"").Append(";").AppendLine();
                }

                m_outputNotifyStr = sb.ToString();

                sb.Clear();
            }

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("拷贝HHL调试代码", GUILayout.Width(200)))
            {
                GUIUtility.systemCopyBuffer = m_outputDebugStr;
            }

            GUI.backgroundColor = Color.blue;
            if (GUILayout.Button("拷贝Module注册", GUILayout.Width(200)))
            {
                GUIUtility.systemCopyBuffer = m_outputModuleStr1;
            }

            if (GUILayout.Button("拷贝Module处理", GUILayout.Width(200)))
            {
                GUIUtility.systemCopyBuffer = m_outputModuleStr2;
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("拷贝cache处理", GUILayout.Width(200)))
            {
                GUIUtility.systemCopyBuffer = m_outputCacheStr;
            }

            GUI.backgroundColor = Color.magenta;
            if (GUILayout.Button("拷贝Notify", GUILayout.Width(200)))
            {
                GUIUtility.systemCopyBuffer = m_outputNotifyStr;
            }

            GUI.backgroundColor = bc;
            GUILayout.EndHorizontal();


            var rect = new Rect(0, 300, 800, 1800);
            //GUILayout.BeginArea(rect);
            m_outPutBigScroll = EditorGUILayout.BeginScrollView(m_outPutBigScroll, GUILayout.Height(200));
            GUILayout.BeginVertical();

            EditorGUILayout.Space();


            GUILayout.Label("HHL调试代码", EditorStyles.boldLabel);
            m_outputDebugScroll = EditorGUILayout.BeginScrollView(m_outputDebugScroll, GUILayout.Height(200));
            m_outputDebugStr = EditorGUILayout.TextArea(m_outputDebugStr);
            EditorGUILayout.EndScrollView();

            GUILayout.Label("模块注册", EditorStyles.boldLabel);
            m_outputModuleScroll1 = EditorGUILayout.BeginScrollView(m_outputModuleScroll1, GUILayout.Height(200));
            m_outputModuleStr1 = EditorGUILayout.TextArea(m_outputModuleStr1);
            EditorGUILayout.EndScrollView();

            GUILayout.Label("模块消息处理", EditorStyles.boldLabel);
            m_outputModuleScroll2 = EditorGUILayout.BeginScrollView(m_outputModuleScroll2, GUILayout.Height(200));
            m_outputModuleStr2 = EditorGUILayout.TextArea(m_outputModuleStr2);
            EditorGUILayout.EndScrollView();

            GUILayout.Label("Cache处理", EditorStyles.boldLabel);
            m_outputCacheScroll = EditorGUILayout.BeginScrollView(m_outputCacheScroll, GUILayout.Height(200));
            m_outputCacheStr = EditorGUILayout.TextArea(m_outputCacheStr);
            EditorGUILayout.EndScrollView();

            GUILayout.Label("Notify处理", EditorStyles.boldLabel);
            m_outputNotifyScroll = EditorGUILayout.BeginScrollView(m_outputNotifyScroll, GUILayout.Height(200));
            m_outputNotifyStr = EditorGUILayout.TextArea(m_outputNotifyStr);
            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndScrollView();
            //GUILayout.EndArea();
            GUILayout.EndScrollView();
            GUIUtility.ExitGUI();
        }
    }
}