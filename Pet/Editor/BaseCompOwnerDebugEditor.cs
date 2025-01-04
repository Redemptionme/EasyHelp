#region FileInfo

// <summary>
// Author  hanhinhe
// Date    2025.01.02
// Desc
// </summary>

#endregion

using IGG.Game.Module.Pet.AI.Role;
using IGG.Game.Module.Pet.Buildings.Base;
using IGG.Game.Module.Pet.Comp;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace HHL.Common.Pet.Editor
{
    [CustomEditor(typeof(BaseCompOwnerDebug))]
    public class BaseCompOwnerDebugEditor : UnityEditor.Editor
    {
        private BaseCompOwner m_owner;

        private AnimBool AttrAnim;
        private AnimBool FsmAnim;

        private void OnEnable()
        {
            if (target is BaseCompOwnerDebug debug)
            {
                var go = debug.gameObject;
                foreach (var item in PetMgr.Inst.Pets)
                {
                    var goComp = item.Value.GetComp<PetGoComp>();
                    if (goComp != null && goComp.Go == go)
                    {
                        m_owner = item.Value;
                        break;
                    }
                }

                foreach (var item in PetMgr.Inst.StakeS)
                {
                    var goComp = item.Value.GetComp<PetGoComp>();
                    if (goComp != null && goComp.Go == go)
                    {
                        m_owner = item.Value;
                        break;
                    }
                }


                AttrAnim = new AnimBool(true, Repaint);
                FsmAnim = new AnimBool(true, Repaint);
            }
        }


        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            if (m_owner == null)
            {
                return;
            }

            GUILayout.Label($"类型 {m_owner.GetType().Name} ID {m_owner.UId}");

            AttrAnim.target = EditorGUILayout.Foldout(AttrAnim.target, "属性");
            if (EditorGUILayout.BeginFadeGroup(AttrAnim.faded))
            {
                var attrComp = m_owner.PetAttrComp;
                var intCount = attrComp.GetIntCount();
                for (var i = 0; i < intCount; i++)
                {
                    var type = (PetAttrInt)i;
                    var value = attrComp.GetIntAttr(type);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label($"{type} : ");
                    var newValue = GUILayout.TextField(value.ToString());
                    if (GUILayout.Button("Button"))
                    {
                        attrComp.SetIntAttr(type, int.Parse(newValue));
                    }

                    GUILayout.EndHorizontal();
                }

                var floatCount = attrComp.GetFloatCount();
                for (var i = 0; i < floatCount; i++)
                {
                    var type = (PetAttrFloat)i;
                    var value = attrComp.GetFloatAttr(type);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label($"{type} : ");
                    var newValue = GUILayout.TextField(value.ToString());
                    if (GUILayout.Button("Button"))
                    {
                        attrComp.SetFloatAttr(type, int.Parse(newValue));
                    }

                    GUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndFadeGroup();


            FsmAnim.target = EditorGUILayout.Foldout(FsmAnim.target, "状态机");
            if (EditorGUILayout.BeginFadeGroup(FsmAnim.faded))
            {
                var fsmComp = m_owner.GetComp<PetFsmComp>();
                var allStates = fsmComp.Fsm.GetAllStates();
                for (var i = 0; i < allStates.Length; i++)
                {
                    var state = allStates[i];
                    GUILayout.BeginHorizontal(fsmComp.Fsm.CurrentState == state ? "Ing" : "state");
                    GUILayout.Label(state.Name);

                    //如果状态不是当前状态 提供切换到该状态的Button按钮
                    if (fsmComp.Fsm.CurrentState != state)
                    {
                        if (GUILayout.Button("待定", GUILayout.Width(50f)))
                        {
                        }
                    }

                    GUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndFadeGroup();

            GUILayout.BeginHorizontal();
            GUILayout.EndHorizontal();
            EditorUtility.SetDirty(target);//增加脏标
        }
    }
}