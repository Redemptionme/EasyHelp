#region FileInfo

// <summary>
// Author  hanhinhe
// Date    2026.01.20
// Desc
// </summary>

#endregion

using System;
using System.Linq;
using System.Reflection;
using IGG.Framework.Panel;
using IGG.Game.Data.Cache;
using IGG.Game.Data.Cache.Activity;
using IGG.Game.Game.HHL.Attr;
using IGG.Game.Module.Activity;
using IGG.Game.Module.Activity.View;
using IGG.Game.Module.GiantBoss.View;
using IGG.Game.Module.Hero;
using IGG.Game.Module.Hero.View;
using IGG.Game.Module.Hero.View.HeroEquip;
using IGG.Game.Module.Kof.View;
using IGG.Game.Module.Return.View;
using IGG.Game.Module.TroopEquip.View;
using Protomsg;
using UnityEngine;

namespace HHL.Common
{
    public partial class HHLGOTools
    {
        // 断点调试用：按基础名分组存储 GameObject 列表，既能看数量又能展开查具体对象
        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<GameObject>>
            m_goGroupDict = new();

        // 断点调试用：按 Component 类型名分组，对应 Memory Profiler 的 Managed Objects
        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<Component>>
            m_compGroupDict = new();


        private void OnClick(KeyCode keyCode)
        {
            var method = GetType()
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(m =>
                {
                    var attr = m.GetCustomAttribute<HHLKeyFunVerAttr>();
                    return attr != null &&
                           attr.Version == Log.Inst.Version &&
                           attr.KeyCode == keyCode;
                });

            method?.Invoke(this, null);
        }

        private void PrintAllGameObject()
        {
            // ── 1. 内存概览 ──────────────────────────────────────────────
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            var gcUsed = GC.GetTotalMemory(false);
            var monoUsed = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong();
            var monoHeap = UnityEngine.Profiling.Profiler.GetMonoHeapSizeLong();
            var totalAlloc = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();

            Log.Inst.Print("");
            Log.Inst.Print("===========Memory Overview=");
            Log.Inst.Print($"  GC Total Memory : {gcUsed / 1024f / 1024f:F2} MB");
            Log.Inst.Print($"  Mono Used       : {monoUsed / 1024f / 1024f:F2} MB");
            Log.Inst.Print($"  Mono Heap       : {monoHeap / 1024f / 1024f:F2} MB");
            Log.Inst.Print($"  Total Allocated : {totalAlloc / 1024f / 1024f:F2} MB");
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            //Log.Inst.Print(IGG.Framework.Resource.ResourceRef.DumpAliveKeys());
            // ResourceMgr pool 大小（pool 里的 GO 虽 SetActive=false 但仍占 Native 内存）
            //Log.Inst.Print(IGG.Framework.Resource.ResourceMgr.Inst.DumpPoolStats());
#endif

            // ── 2. GameObject 分组（断点调试用） ─────────────────────────
            var all = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            m_goGroupDict.Clear();

            // 按 Component 类型统计的字典（断点调试用）
            m_compGroupDict.Clear();
            var compDict = m_compGroupDict;

            foreach (var item in all)
            {
                if (!item.scene.isLoaded)
                {
                    continue;
                }

                // 去掉末尾 "(数字)" 实例编号，统计基础名
                var name = item.name;
                var p = name.LastIndexOf(" (", StringComparison.Ordinal);
                if (p >= 0 && name.EndsWith(")"))
                {
                    name = name.Substring(0, p);
                }

                if (!m_goGroupDict.TryGetValue(name, out var goList))
                {
                    goList = new System.Collections.Generic.List<GameObject>();
                    m_goGroupDict[name] = goList;
                }

                goList.Add(item);

                // 收集每个 GO 上的所有 Component
                var comps = item.GetComponents<Component>();
                foreach (var comp in comps)
                {
                    if (comp == null)
                    {
                        continue; // missing script guard
                    }

                    var typeName = comp.GetType().FullName;
                    if (!compDict.TryGetValue(typeName, out var compList))
                    {
                        compList = new System.Collections.Generic.List<Component>();
                        compDict[typeName] = compList;
                    }

                    compList.Add(comp);
                }
            }

            // ── 3. 输出 GameObject 统计（按数量降序） ───────────────────
            var sortedGO =
                new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string,
                    System.Collections.Generic.List<GameObject>>>(m_goGroupDict);
            sortedGO.Sort((a, b) => b.Value.Count.CompareTo(a.Value.Count));

            Log.Inst.Print($"===========GameObject= total:{all.Length} types:{sortedGO.Count}");
            foreach (var kv in sortedGO)
            {
                Log.Inst.Print($"{kv.Value.Count,5}  {kv.Key}");
            }

            // ── 4. 输出 Component 类型统计（按数量降序，最能对应 Managed Objects） ──
            var sortedComp =
                new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string,
                    System.Collections.Generic.List<Component>>>(compDict);
            sortedComp.Sort((a, b) => b.Value.Count.CompareTo(a.Value.Count));

            Log.Inst.Print($"===========Component Types= types:{sortedComp.Count}");
            foreach (var kv in sortedComp)
            {
                Log.Inst.Print($"{kv.Value.Count,5}  {kv.Key}");
            }

            // ── 5. 详细路径（需要时取消注释）────────────────────────────
            // Log.Inst.Print($"===========detail=");
            // foreach (var kv in sortedGO)
            //     foreach (var go in kv.Value)
            //         Log.Inst.Print($"  [{kv.Key}]  {GetGameObjectPath(go)}");

            Log.Inst.Print($"===========End=");
            Log.Inst.Print("");
            // 断点调试：
            //   m_goGroupDict["xxx"]  → 查看具体 GameObject
            //   compDict["IGG.Framework.Resource.ResourceRefProxy"] → 查看具体组件实例及其 GO 路径
        }

        private static string GetGameObjectPath(GameObject go)
        {
            var sb = new System.Text.StringBuilder(go.name);
            var t = go.transform.parent;
            while (t != null)
            {
                sb.Insert(0, t.name + "/");
                t = t.parent;
            }

            return sb.ToString();
        }

        [HHLKeyFunVerAttr(Ver.V1_60, KeyCode.F3)]
        private void OnF3Click_60()
        {
            HeroModule.Inst.OpenHeroEquipPanel(1032);
            HeroModule.Inst.OpenHeroEquipDevelopPanel(1002, HeroEquipDevelopPanelType.Upgrade);   
        }
        
        [HHLKeyFunVerAttr(Ver.V1_60, KeyCode.F4)]
        private void OnF4Click_60()
        {
            //PanelMgr.Inst.OpenPanel<PinBallPanel>();    
        }

        [HHLKeyFunVerAttr(Ver.V1_59, KeyCode.F3)]
        private void OnF3Click_59()
        {
        }

        [HHLKeyFunVerAttr(Ver.V1_58, KeyCode.F3)]
        private void OnF3Click_58()
        {
            PanelMgr.Inst.OpenPanel<FairyTailLotteryPanel>(ActivityModule.Inst.FairyTailLotteryActivityId);
        }

        [HHLKeyFunVerAttr(Ver.V1_58, KeyCode.F4)]
        private void OnF4Click_58()
        {
            var msg = new MsgGS2CLPoolLotteryActivityStageRewardReply
            {
                ActivityId = 191208,
                Stage = 460,
                Rewards =
                {
                    new Resource
                    {
                        ResType = 2,
                        SubType = 72431,
                        Value = 1
                    }
                },
                ReturnRewards =
                {
                    new Resource
                    {
                        ResType = 2,
                        SubType = 48566,
                        Value = 450000
                    }
                }
            };

            PanelMgr.Inst.OpenPanel<FairyTailRewardPanel>(FairyTailRewardPanel.PanelType.StageReward, msg);
        }


        [HHLKeyFunVerAttr(Ver.V1_57, KeyCode.F4)]
        private void OnF4Click_57()
        {
            PanelMgr.Inst.OpenPanel<ReturnSlotMachinePanel>(ActivityModule.Inst.ReturnSlotMachineId);
        }

        [HHLKeyFunVerAttr(Ver.V1_57, KeyCode.F3)]
        private void OnF3Click_57()
        {
            PanelMgr.Inst.OpenPanel<RegressTurntablePanel>((uint)AppCache.Activity.RegressLabaId);
        }

        [HHLKeyFunVerAttr(Ver.V1_56, KeyCode.F7)]
        private void OnF7Click_56()
        {
            PrintAllGameObject();
        }

        [HHLKeyFunVerAttr(Ver.V1_56, KeyCode.F4)]
        private void OnF4Click_56()
        {
            PanelMgr.Inst.OpenPanel<Match2MainPanel>(ActivityModule.Inst.SheepActId);
        }

        [HHLKeyFunVerAttr(Ver.V1_56, KeyCode.F3)]
        private void OnF3Click_56()
        {
            PanelMgr.Inst.OpenPanel<HeroEquipRenovationPanel>(1);
        }

        [HHLKeyFunVerAttr(Ver.V1_55, KeyCode.F4)]
        private void OnF4Click_55()
        {
            PanelMgr.Inst.OpenPanel<TroopsEquipBagPanel>();
        }


        [HHLKeyFunVerAttr(Ver.V1_55, KeyCode.F3)]
        private void OnF3Click_55()
        {
            PanelMgr.Inst.OpenPanel<ActivityPanel>((uint)ActivityCache.PersonalActivityAdd + 70, true);
        }


        [HHLKeyFunVerAttr(Ver.V1_53, KeyCode.F3)]
        private void OnF3Click_53()
        {
            PanelMgr.Inst.OpenPanel<GiantBossPanel>();
        }
    }
}