using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Collections;
using HHL.Game;
using IGG.Framework.Cache;
using IGG.Framework.Panel;
using IGG.Game.Data.Cache;
using IGG.Game.Data.Cache.Activity;
using IGG.Game.Data.Cache.Bag;
using IGG.Game.Data.Cache.Setting;
using IGG.Game.Data.Config;
using IGG.Game.Helper;
using IGG.Game.Managers;
using IGG.Game.Module.Activity;
using IGG.Game.Module.Activity.View;
using IGG.Game.Module.Bag.View;
using IGG.Game.Module.BattleRoyale;
using IGG.Game.Module.BattleRoyale.View;
using IGG.Game.Module.CampIsland;
using IGG.Game.Module.Cargo;
using IGG.Game.Module.Cargo.View;
using IGG.Game.Module.CityBuilding;
using IGG.Game.Module.Common;
using IGG.Game.Module.Common.View;
using IGG.Game.Module.NewCity;
using IGG.Game.Module.PlayerOp.OpStates;
using IGG.Game.Module.Reward;
using IGG.Game.Module.Vehicle.View;
using IGG.Game.Module.WorldMap.Help;
using IGG.Game.UI.Hero;
using Protomsg;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HHL.Common
{
    public class HHLGOTools : MonoBehaviour
    {
        public static HHLGOTools Self;
        public Vector3 Param1;

        public Vector3 Param2;

        // Start is called before the first frame update
        private void Start()
        {
            Self = this;
            DontDestroyOnLoad(this);
        }

        // Update is called once per frame
        private void Update()
        {
            CheckInput();
        }

        private void OnDestroy()
        {
            Log.Inst.ToolsInit = false;
        }

        private void CheckInput()
        {
            if (Input.GetKeyDown(KeyCode.F12))
            {
                Log.Inst.Print($"------------------------------------------------------");
                //CityBuildingModule.Inst.TriggerCityRoleHappy();
                //CityBuildingModule.Inst.FlyOutCityIcon();
            }

            if (Input.GetKeyDown(KeyCode.F3))
            {
                HHL.Common.Log.Inst.TestShowSeniorRewardPanel();
                // AppPrefs.CargoFirstAdd.Value = false;
                // AppPrefs.Save();
                // CargoModule.Inst.Lock = Self.Param1.x > 0f;
                // PanelMgr.Inst.OpenPanel<CargoPanel>(0);
                //PanelMgr.Inst.OpenPanel<SlidingPuzzlePanel>();

                //CityChange();
                //FakeMove();

                //PanelMgr.Inst.OpenPanel<KOFPuzzleDisplayDetailPanel>();

                // var list = new List<Resource>();
                // for (var i = 0; i < Param1.x; i++)
                // {
                //     list.Add(new Resource());
                // }
                //
                // RewardModule.Inst.ShowSeniorRewardPanel(list.ToArray());

                //RedDotMgr.Inst.Set(RedDotPath.KofPuzzlePuzzle, Random.Range(1,3));

                //WatchInfo();
                //TestVehicleGetPanel();
                //TestPlaneWarPanel();
                //PanelMgr.Inst.OpenPanel<KOFPuzzleDrawPanel>((uint)ActivityModule.KofPuzzleActId);
            }

            if (Input.GetKeyDown(KeyCode.F4))
            {
                gameObject.AddComponent<CityMapTool>();
                //BattleRoyaleModule.Inst.OpenLoading();
                //CampIslandModule.Inst.OpenActivityPanel();
                //CampIslandModule.Inst.OpenInnerActivityRank();

                // var panel = PanelMgr.Inst.GetPanel<KOFPuzzleDisplayDetailPanel>();
                // panel.RefreshPos(Param1, Param2);

                // ActivityModule.Inst.OpenKOFPuzzleDrawPanel();

                //ShowRandomPiece();
                //ShowRandomPiece2();
                //FlyKofReward();
                //RedDotMgr.Inst.Set(RedDotPath.KofPuzzlePuzzleTimes, Random.Range(1,3));
                //TestVehiclePanel();
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                //m_panelMgr.OpenPanel("CommonNeedResPanel", new object[] { list });
                var vos = new List<UseItemVo>();
                var itemVo = new UseItemVo()
                {
                    ResType = BaseResType.Item, 
                    NeedCount = 25,
                    Id =  AppCache.Cargo.Vo.PlayerRefreshItemId,
                };
                vos.Add(itemVo);
                PanelMgr.Inst.OpenPanel<BagResourcesPanel>(new object[] { vos });
            }


            if (Input.GetKeyDown(KeyCode.F8))
            {
                //PanelMgr.Inst.ClosePanel<BattleRoyaleLoadingPanel>();
                OutCityIntoCity(1);
            }

            if (Input.GetKeyDown(KeyCode.F9))
            {
                AddOutCity(1);
            }

            if (Input.GetKeyDown(KeyCode.F10))
            {
                AddOutCity(-1);
            }

            if (Input.GetKeyDown(KeyCode.F7))
            {
                DeadCity(-1);
            }

            // if (Input.GetKeyDown(KeyCode.I))
            // {
            //     var wall = AppCache.CityBuilding.MyCompCity.GetBuilding(EBuildingType.CityWall);
            //     if (wall == null || wall.Body == null)
            //     {
            //         return;
            //     }
            //
            //
            //     var pos1 = wall.Owner.LocalCenterOffset + wall.Owner.Trans.position;
            //     AddCube(pos1, new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, -45f, 0f), Color.red, "pos1");
            //     var pos2 = wall.Body.GetBoundingBox().center + pos1;
            //     AddCube(pos2, new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, -45f, 0f), Color.yellow, "pos2");
            //     var pos3 = wall.Body.GetBoundingBox().extents + pos1;
            //     AddCube(pos3, new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, -45f, 0f), Color.magenta, "pos3");
            //     var pos4 = pos2 + pos3 - pos1;
            //     AddCube(pos4, new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, -45f, 0f), Color.green, "pos4");
            //
            //     var fLen = pos2.x;
            //     var dis = 4.3f;
            //     var pos5 = new Vector3(fLen - dis, 0, 0) + pos1;
            //     AddCube(pos5, new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, -45f, 0f), Color.blue, "pos5");
            //     var pos6 = new Vector3(0, 0, fLen - dis) + pos1;
            //     AddCube(pos6, new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, -45f, 0f), Color.blue, "pos6");
            //     var pos7 = new Vector3(0, 0, dis - fLen) + pos1;
            //     AddCube(pos7, new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, -45f, 0f), Color.blue, "pos7");
            //     var pos8 = new Vector3(dis - fLen, 0, 0) + pos1;
            //     AddCube(pos8, new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, -45f, 0f), Color.blue, "pos8");
            // }
        }

        private void TestPlaneWarPanel()
        {
            PanelMgr.Inst.OpenPanel<ActivityPanel>((uint)160300, true, ActivityCache.EActivityState.PlaneWar);
        }

        private void TestVehiclePanel()
        {
            PanelMgr.Inst.OpenPanel<VehiclePanel>(EVehiclePanelState.Accessories, "",
                AppCache.Vehicle.CarportVehicleId);
        }

        private void TestVehicleGetPanel()
        {
            PanelMgr.Inst.OpenPanel<VehicleGetPanel>(WorkshopVehicleDao.Inst.SortList[0].Id);
        }

        private void WatchInfo()
        {
            var nCount = RedDotMgr.Inst.GetCount(RedDotPath.KofPuzzle);
            var nCount1 = RedDotMgr.Inst.GetCount(RedDotPath.KofPuzzlePuzzle);
            var nCount2 = RedDotMgr.Inst.GetCount(RedDotPath.KofPuzzlePuzzleTimes);
            var nCount3 = RedDotMgr.Inst.GetCount(RedDotPath.KofPuzzleGift);


            // var activityInfo = AppCache.Activity.GetActivity(ActivityModule.KofPuzzleActId);
            // if (activityInfo == null)
            // {
            //     return ;
            // }
            // if (activityInfo.Status == ActivityStatus.KActivityStatuEnd)
            // {
            //     return ;
            // }
            //
            // if (TimeHelper.ServerTime > activityInfo.TimeEnd)
            // {
            //     return ;
            // }
        }

        private void FlyKofReward()
        {
            var panel = PanelMgr.Inst.GetPanel<KOFPuzzleDrawPanel>();
            if (panel == null)
            {
                return;
            }

            var list = ListPool<RafflePuzzleRewardInfo>.Get();
            for (var i = 0; i < Param1.y; i++)
            {
                var info = new RafflePuzzleRewardInfo();
                var pieces = ActivityPuzzlePiecesDao.Inst.GetPieces((uint)i + 1);
                info.Type = RafflePuzzleRewardType.Pieces;
                info.IsRepeated = false;
                info.Id = pieces[Random.Range(0, pieces.Count)];
                info.Num = 1;

                list.Add(info);
            }


            panel.FlyEffect(ListPool<RafflePuzzleRewardInfo>.PutAndToArray(list));
        }

        private void ShowRandomPiece2()
        {
            var list = new List<RafflePuzzleRewardInfo>();
            for (var i = 0; i < Param1.y; i++)
            {
                var info = new RafflePuzzleRewardInfo();
                var pieceList = ActivityPuzzlePiecesDao.Inst.GetPieces((uint)Param1.z);
                info.Type = RafflePuzzleRewardType.Pieces;
                info.IsRepeated = Random.Range(0, 2) == 1;
                info.Id = pieceList[Random.Range(0, pieceList.Count)];
                info.Num = 1;

                list.Add(info);
            }

            PanelMgr.Inst.OpenPanel<KOFPuzzleRewardPanel>((uint)190161, list.ToArray());
        }

        private void ShowRandomPiece()
        {
            var list = new List<RafflePuzzleRewardInfo>();
            for (var i = 0; i < Param1.y; i++)
            {
                var info = new RafflePuzzleRewardInfo();
                info.Type = (RafflePuzzleRewardType)Random.Range(0, 2);
                switch (info.Type)
                {
                    case RafflePuzzleRewardType.Item:
                        info.Id = 45159;
                        info.Num = (uint)Random.Range(0, 100);
                        break;
                    case RafflePuzzleRewardType.Pieces:
                        info.IsRepeated = Random.Range(0, 2) == 1;
                        info.Id = ActivityPuzzlePiecesDao.Inst
                            .Configs[Random.Range(0, ActivityPuzzlePiecesDao.Inst.Configs.Length)].PiecesId;
                        info.Num = 1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                list.Add(info);
            }

            PanelMgr.Inst.OpenPanel<KOFPuzzleRewardPanel>((uint)190161, list.ToArray());
        }

        private void FakeMove()
        {
            var dic = AppCache.CityBuilding.MyCity.BuildingInfoes;
            var list = new List<BuildingInfo>();
            foreach (var item in dic)
            {
                list.Add(item.Value);
            }

            var startId = list[Random.Range(0, list.Count)].Id;
            var endId = list[Random.Range(0, list.Count)].Id;

            CityBuildingModule.Inst.FakeRoleMove(startId, endId);
        }

        public void CityChange()
        {
            CityBuildingModule.Inst.Debug = true;
            var index = CityBuildingModule.Inst.CityStatus == CityStatus.PlayGame
                ? 1
                : (uint)CityBuildingModule.Inst.CityStatus + 1;
            var cfg = CityWorktimeDao.Inst.GetCfg(index);
            CityBuildingModule.Inst.CurWorkTimeConfig = cfg;
            CityBuildingModule.Inst.CityStatus = (CityStatus)index;
        }

        public void AddCube(Vector3 pos, Vector3 scale, Vector3 rotate, Color color, string cubeName)
        {
            var cellObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cellObj.transform.position = pos;
            cellObj.transform.localScale = scale;
            cellObj.transform.localRotation = Quaternion.Euler(rotate);

            var msRender = cellObj.GetComponent<MeshRenderer>();
            msRender.material.shader = Shader.Find("Transparent/Diffuse");
            color.a = 1f;
            msRender.material.color = color;
            cellObj.name = cubeName;
        }

        private void DeadCity(int num)
        {
            var msg = new MsgGS2CLPopulationBaseDataNotice();
            foreach (var item in AppCache.CityBuilding.BuildPopulationDic)
            {
                msg.BuildAppoint.Add(item.Value);
            }

            msg.BaseInfo = AppCache.CityBuilding.PopulationInfo.Clone();

            for (var i = 0; i < -num; i++)
            {
                if (msg.BaseInfo.NowInIdleNum > 0)
                {
                    msg.BaseInfo.NowInIdleNum--;
                }
                else
                {
                    foreach (var info in msg.BuildAppoint)
                    {
                        if (info.AppointNum > 0)
                        {
                            info.AppointNum--;
                        }
                    }

                    foreach (var info in msg.BuildAppoint.ToArray())
                    {
                        if (info.AppointNum == 0)
                        {
                            msg.BuildAppoint.Remove(info);
                        }
                    }
                }
            }

            CityBuildingModule.Inst.OnMsgGS2CLPopulationBaseDataNotice(msg);
        }

        private void AddOutCity(int num)
        {
            var msg = new MsgGS2CLPopulationBaseDataNotice();
            foreach (var item in AppCache.CityBuilding.BuildPopulationDic)
            {
                msg.BuildAppoint.Add(item.Value);
            }

            msg.BaseInfo = AppCache.CityBuilding.PopulationInfo.Clone();
            if (msg.BaseInfo.RecruitInfo == null)
            {
                msg.BaseInfo.RecruitInfo = new RecruitPopulationInfo();
            }

            msg.BaseInfo.RecruitInfo.RecruitPopulationDisappearTime = (ulong)(TimeHelper.ServerTime + 3600);

            msg.BaseInfo.RecruitInfo.NowRecruitPopulationNum =
                (uint)(msg.BaseInfo.RecruitInfo.NowRecruitPopulationNum + num);
            CityBuildingModule.Inst.OnMsgGS2CLPopulationBaseDataNotice(msg);
        }

        private void OutCityIntoCity(int num)
        {
            var msg = new MsgGS2CLPopulationRecruitReply
            {
                NowInIdleNum = (uint)(AppCache.CityBuilding.PopulationInfo.NowInIdleNum + num),
                PopulationMax = (uint)(AppCache.CityBuilding.PopulationInfo.PopulationMax + num),
                RecruitInfo = new RecruitPopulationInfo
                {
                    RecruitPopulationDisappearTime =
                        AppCache.CityBuilding.PopulationInfo.RecruitInfo.RecruitPopulationDisappearTime,
                    NowRecruitPopulationNum =
                        (uint)(AppCache.CityBuilding.PopulationInfo.RecruitInfo.NowRecruitPopulationNum - num)
                }
            };

            CityBuildingModule.Inst.OnMsgGS2CLPopulationRecruitReply(msg);
        }

        [MenuItem("HHL/启动游戏")]
        private static void OpenGame()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Launch.unity");
        }

        [MenuItem("HHL/启动小人编辑器")]
        private static void OpenNewCityTools()
        {
            EditorSceneManager.OpenScene("Assets/GameTools/NewCity/NewCity.unity");
        }
    }
}