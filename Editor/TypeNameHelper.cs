#region FileInfo

// <summary>
// Author  hanhinhe
// Date    2024.07.12
// Desc
// </summary>

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using HHL.Common;

namespace Game.HHL.Editor
{
    public enum EProtoType
    {
        None,
        Notice,
        Request,
        Reply
    }

    public class ProtoStruct
    {
        public string Annotation;


        public string m_className;

        public string ClassName
        {
            get => m_className;
            set
            {
                m_className = value;
                GenProtoType();
            }
        }

        public string FuncName = "";
        public string TypeName;
        public EProtoType ProtoType;

        public void GenProtoType()
        {
            if (StringUtils.Contains(ClassName, EProtoType.Notice.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                ProtoType = EProtoType.Notice;
            }
            else if (StringUtils.Contains(ClassName, EProtoType.Request.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                ProtoType = EProtoType.Request;
            }
            else if (StringUtils.Contains(ClassName, EProtoType.Reply.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                ProtoType = EProtoType.Reply;
            }

            FuncName = ClassName.Substring(8);
        }

        private string TotalFieldName;
        private string m_originFieldName;
        public string ParamName;

        public string FieldName
        {
            get => TotalFieldName;

            set
            {
                m_originFieldName = value;
                var paramStr = m_originFieldName.Split("_");
                if (paramStr.Length > 1)
                {
                    if (paramStr.Length == 1)
                    {
                        return;
                    }

                    TotalFieldName = "";
                    foreach (var str in paramStr)
                    {
                        TotalFieldName += str.Substring(0, 1).ToUpper() + str.Substring(1);
                    }
                }
                else
                {
                    TotalFieldName = m_originFieldName.Substring(0, 1).ToUpper() + m_originFieldName.Substring(1);
                }

                ParamName = TotalFieldName.Substring(0, 1).ToLower() + TotalFieldName.Substring(1);
            }
        }


        //public Type ClassType;
        public List<ProtoStruct> Childs = new();
    }

    public static class TypeNameHelper
    {
        public const string MsgCL2GS = "MsgCL2GS";
        public const string MsgGS2CL = "MsgGS2CL";
        public const string Message = "message";
        public const string Repeated = "repeated";
        public const string Msg = "Msg";

        public static string GetClientName(string name, bool isRepeated)
        {
            var typeName = m_clientName.TryGetValue(name, out var type) ? type : name;
            return isRepeated ? $"List<{typeName}>" : typeName;
        }

        public static Dictionary<string, string> m_clientName = new()
        {
            { "uint32", "uint" },
            { "int32", "int" },
            { "uint64", "ulong" },
            { "int64", "long" },
            { "float", "float" },
            { "double", "double" },
            { "string", "string" },
            { "message", "ProtoStruct" }
        };

        public static Type GetType(string name)
        {
            return m_Types.TryGetValue(name, out var type) ? type : null;
        }

        public static Dictionary<string, Type> m_Types = new()
        {
            { "uint32", typeof(uint) },
            { "int32", typeof(int) },
            { "uint64", typeof(ulong) },
            { "int64", typeof(long) },
            { "float", typeof(float) },
            { "double", typeof(double) },
            { "string", typeof(string) },
            { "message", typeof(ProtoStruct) }
        };

        public static Type GetRepeatedType(string name)
        {
            return m_repeatedTypes.TryGetValue(name, out var type) ? type : null;
        }

        public static Dictionary<string, Type> m_repeatedTypes = new()
        {
            { "uint32", typeof(List<uint>) },
            { "int32", typeof(List<int>) },
            { "uint64", typeof(List<ulong>) },
            { "int64", typeof(List<long>) },
            { "float", typeof(List<float>) },
            { "double", typeof(List<double>) },
            { "string", typeof(List<string>) },
            { "message", typeof(List<ProtoStruct>) }
        };
    }
}