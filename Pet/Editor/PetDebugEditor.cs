#region FileInfo

// <summary>
// Author  hanhinhe
// Date    2025.01.03
// Desc
// </summary>

#endregion

using System.Collections.Generic;
using IGG.Game.Module.Pet.AI.Role;
using IGG.Game.Module.Pet.Comp;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace HHL.Common.Pet.Editor
{
    [CustomEditor(typeof(PetDebug))]
    public class PetDebugEditor : UnityEditor.Editor
    {
        public string Color_StakeId = "StakeId_";
        public string Color_StakePos = "StakePos_";
        public string Color_PetId = "PetId_";

        private AnimBool StakeGroups;
        private List<AnimBool> StakeGroupList = new();

        private AnimBool PetGroups;
        private List<AnimBool> PetGroupList = new();
        private Dictionary<string, Color> m_colors = new();

        private GUIStyle Style = new();

        private void OnEnable()
        {
            StakeGroups = new AnimBool(true, Repaint);
            for (var i = 0; i < PetMgr.Inst.StakeS.Count; i++)
            {
                var animBool = new AnimBool(true, Repaint);
                StakeGroupList.Add(animBool);
            }

            PetGroups = new AnimBool(true, Repaint);
            for (var i = 0; i < PetMgr.Inst.Pets.Count; i++)
            {
                var animBool = new AnimBool(true, Repaint);
                PetGroupList.Add(animBool);
            }

            foreach (var item in PetMgr.Inst.StakeS)
            {
                var color = GetRandomColor();
                m_colors.Add($"{Color_StakeId}{item.Value.UId}", color);

                for (var i = 0; i < 3; i++)
                {
                    color = GetRandomColor();
                    m_colors.Add($"{Color_StakePos}{item.Value.UId}_{i}", color);
                }
            }

            foreach (var item in PetMgr.Inst.Pets)
            {
                var color = GetRandomColor();
                m_colors.Add($"{Color_PetId}{item.Value.UId}", color);
            }
        }

        public Color GetColor(string colorKey)
        {
            return m_colors.GetValueOrDefault(colorKey);
        }

        public void SetColor(string colorKey, Color color)
        {
            m_colors[colorKey] = color;
        }

        public Color GetRandomColor()
        {
            return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }

        public override void OnInspectorGUI()
        {
            StakeGroups.target = EditorGUILayout.Foldout(StakeGroups.target, "木桩");
            var oldColor = Style.normal.textColor;
            if (EditorGUILayout.BeginFadeGroup(StakeGroups.faded))
            {
                var tag = 0;
                foreach (var item in PetMgr.Inst.StakeS)
                {
                    var stake = item.Value;
                    var animBool = StakeGroupList[tag];
                    // var color = GetColor($"{Color_StakeId}{stake.UId}");
                    // color = EditorGUILayout.ColorField("木桩色", color,GUILayout.Width(8));
                    // SetColor($"{Color_StakeId}{stake.UId}", color);
                    // Style.normal.textColor = color;

                    animBool.target = EditorGUILayout.Foldout(animBool.target, $"木桩 {stake.UId}", Style);
                    if (EditorGUILayout.BeginFadeGroup(animBool.faded))
                    {
                        GUILayout.BeginHorizontal();
                        for (var i = 0; i < stake.AttackPosList.Count; i++)
                        {
                            var attackPos = stake.AttackPosList[i];
                            // var bc = GUI.backgroundColor;
                            // GUI.backgroundColor = Color.green;


                            GUILayout.BeginVertical();
                            // var colorIndex = GetColor($"{Color_StakePos}{stake.UId}_{i}");
                            // colorIndex = EditorGUILayout.ColorField("位置色", colorIndex,GUILayout.Width(10));
                            // SetColor($"{Color_StakePos}{stake.UId}_{i}", colorIndex);
                            // Style.normal.textColor = colorIndex;
                            GUILayout.Label($"Index {attackPos.Index}", Style);
                            GUILayout.Label($"Pos {attackPos.Pos}");
                            GUILayout.Label($"Attack {attackPos.AttackId}");
                            GUILayout.Label($"Owner {attackPos.OwnerId}");
                            GUILayout.EndHorizontal();

                            Style.normal.textColor = oldColor;
                            //GUI.backgroundColor = bc;
                        }

                        GUILayout.EndHorizontal();
                    }

                    EditorGUILayout.EndFadeGroup();
                    tag++;
                }
            }

            EditorGUILayout.EndFadeGroup();

            PetGroups.target = EditorGUILayout.Foldout(PetGroups.target, "宠物");
            if (EditorGUILayout.BeginFadeGroup(PetGroups.faded))
            {
                var tag = 0;
                foreach (var item in PetMgr.Inst.Pets)
                {
                    var pet = item.Value;
                    var animBool = PetGroupList[tag];
                    if (tag % 2 == 0)
                    {
                        GUILayout.BeginHorizontal();
                    }

                    // var color = GetColor($"{Color_PetId}{pet.UId}");
                    // color = EditorGUILayout.ColorField("宠物色", color);
                    // SetColor($"{Color_PetId}{pet.UId}", color);
                    // Style.normal.textColor = color;
                    animBool.target = EditorGUILayout.Foldout(animBool.target, $"宠物 {pet.UId}", Style);
                    if (EditorGUILayout.BeginFadeGroup(StakeGroups.faded))
                    {
                        GUILayout.BeginVertical();
                        var targetId = pet.OwnerAttrComp.GetIntAttr(PetAttrInt.TargetId);
                        var posIndex = pet.OwnerAttrComp.GetIntAttr(PetAttrInt.TargetPosIndex);

                        if (targetId != 0)
                        {
                            // var stakecolor = GetColor($"{Color_StakeId}{targetId}");
                            // Style.normal.textColor = stakecolor;
                            GUILayout.Label($"TargetId {targetId}", Style);
                            // var posColor = GetColor($"{Color_StakePos}{targetId}_{posIndex}");
                            // Style.normal.textColor = posColor;
                            GUILayout.Label($"TargetPosIndex {posIndex}", Style);
                        }
                        else
                        {
                            GUILayout.Label($"TargetId ---");
                            GUILayout.Label($"TargetPosIndex ---");
                        }

                        GUILayout.Label($"TargetHitCount {pet.OwnerAttrComp.GetIntAttr(PetAttrInt.TargetHitCount)}");
                        GUILayout.Label(
                            $"TargetMaxHitCount {pet.OwnerAttrComp.GetIntAttr(PetAttrInt.TargetMaxHitCount)}");

                        GUILayout.EndHorizontal();
                    }

                    EditorGUILayout.EndFadeGroup();
                    if (tag % 2 == 1)
                    {
                        GUILayout.EndHorizontal();
                    }

                    tag++;
                }
            }

            EditorGUILayout.EndFadeGroup();
        }
    }
}