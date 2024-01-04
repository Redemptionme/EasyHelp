using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using HHL.Game;
using IGG.Framework.Panel;
using IGG.Game.Data.Cache;
using IGG.Game.Data.Config;
using IGG.Game.Helper;
using IGG.Game.Module.BattleRoyale;
using IGG.Game.Module.BattleRoyale.View;
using IGG.Game.Module.CampIsland;
using IGG.Game.Module.CityBuilding;
using IGG.Game.Module.Common.View;
using IGG.Game.Module.NewCity;
using IGG.Game.Module.PlayerOp.OpStates;
using IGG.Game.Module.WorldMap.Help;
using Protomsg;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HHL.Common
{
    public class HHLGOTools : MonoBehaviour
    {
        public static HHLGOTools Self;
        public Vector3 TestPos1;
        public Vector3 TestPos2;
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
            if (Input.GetKeyDown(KeyCode.F4))
            {
                gameObject.AddComponent<CityMapTool>();
                //BattleRoyaleModule.Inst.OpenLoading();
                //CampIslandModule.Inst.OpenActivityPanel();
                //CampIslandModule.Inst.OpenInnerActivityRank();
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                HHL.Common.Log.Inst.Print($"------------------------------------------------------");
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                var index = CityBuildingModule.Inst.CityStatus == CityStatus.PlayGame ? 1 : (uint)CityBuildingModule.Inst.CityStatus + 1;
                var cfg = CityWorktimeDao.Inst.GetCfg(index);
                CityBuildingModule.Inst.CurWorkTimeConfig = cfg;
                CityBuildingModule.Inst.CityStatus = (CityStatus)index;

                // var dic = AppCache.CityBuilding.MyCity.BuildingInfoes;
                // var list = new List<BuildingInfo>();
                // foreach (var item in dic)
                // {
                //     list.Add(item.Value);
                // }
                //
                // var startId = list[Random.Range(0, list.Count)].Id;
                // var endId = list[Random.Range(0, list.Count)].Id;
                //
                // CityBuildingModule.Inst.FakeRoleMove(startId,endId);
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
            
            if (Input.GetKeyDown(KeyCode.I))
            {
                var wall = AppCache.CityBuilding.MyCompCity.GetBuilding(EBuildingType.CityWall);
                if (wall == null || wall.Body == null)
                {
                    return;
                }

                
                var pos1 = wall.Owner.LocalCenterOffset + wall.Owner.Trans.position;
                AddCube(pos1, new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, -45f, 0f), Color.red, "pos1");
                var pos2 = wall.Body.GetBoundingBox().center + pos1;
                AddCube(pos2, new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, -45f, 0f), Color.yellow, "pos2");
                var pos3 = wall.Body.GetBoundingBox().extents + pos1;
                AddCube(pos3, new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, -45f, 0f), Color.magenta, "pos3");
                var pos4 = pos2 + pos3 - pos1;
                AddCube(pos4, new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, -45f, 0f), Color.green, "pos4");
                
                float fLen = pos2.x;
                var dis = 4.3f;
                var pos5 = new Vector3(fLen - dis, 0, 0) + pos1; 
                AddCube(pos5, new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, -45f, 0f), Color.blue, "pos5");
                var pos6 = new Vector3(0, 0, fLen - dis) + pos1;
                AddCube(pos6, new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, -45f, 0f), Color.blue, "pos6");
                var pos7 = new Vector3(0, 0, dis - fLen) + pos1;
                AddCube(pos7, new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, -45f, 0f), Color.blue, "pos7");
                var pos8 = new Vector3(dis - fLen, 0, 0) + pos1;
                AddCube(pos8, new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, -45f, 0f), Color.blue, "pos8");
                
                
                
                
                
                
            }
             
            
        }

        public void AddCube(Vector3 pos,Vector3 scale,Vector3 rotate,Color color,string cubeName)
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
            msg.BaseInfo.NowInIdleNum = (uint)(msg.BaseInfo.NowInIdleNum + num);

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
    }
}