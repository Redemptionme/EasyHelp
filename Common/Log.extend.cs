using System;
using System.Collections.Generic;
using System.Text;
using IGG.Game.Managers.Network;
using Msgtype;
using Google.Protobuf;
using IGG.Framework.Cache;
using IGG.Framework.IO;
using IGG.Game.Data.Cache;
using IGG.Game.Data.Cache.Common;
using IGG.Game.Data.Cache.Mail.Type;
using IGG.Game.Data.Cache.WorldMap.Entity.Comp;
using IGG.Game.Data.Config;
using IGG.Game.Module.Reward;
using IGG.Game.Module.Rune.Comp;
using Protomsg;

namespace HHL.Common
{
    public partial class Log
    {
        // 写文件单行字符长度,stringbuilder长度现在默认是200,所以别超过200
        private int _fileTxtLen = 120;
        private bool _allMsg = false;
        private bool m_bAllEntityComp = false;
        private readonly List<MsgType> m_MsgList = new List<MsgType>();
        private readonly List<MsgType> m_ignoreMsgList = new List<MsgType>();
        private readonly List<MsgType> m_speicalList = new List<MsgType>();
        private readonly List<ulong> m_checkEntitys = new List<ulong>();
        private readonly List<EntityType> m_entityTypes = new List<EntityType>();
        private readonly Dictionary<Type, Type> m_compsBindDic = new Dictionary<Type, Type>();

        public enum eLogType
        {
            eLog = 0,
            eWarning,
            eErro,
            eRootNetwork,
            ewise
        }

        public enum eLogOut
        {
            eNone = 0,
            eFile = 1,
            eUnity = 1 << 1
        }

        private void InitExtend()
        {
            //_filePath = System.Environment.CurrentDirectory + "\\Assets\\Scripts\\Game\\HHL" + "\\HHL.log";
            _filePath = Environment.CurrentDirectory + "\\output" + "\\HHL.log";

            _typeList.Add(eLogType.eLog);
            _typeList.Add(eLogType.eWarning);
            _typeList.Add(eLogType.eErro);
            _typeList.Add(eLogType.eRootNetwork);

            _outType |= (uint)eLogOut.eFile;
            //_outType |= (uint) eLogOut.eUnity ;

            InitMsgListen();

            var strFu = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Print("", eLogType.eLog, false);
            Print("-----HHL Log----------" + strFu + "--------------");
        }

        public void PrintUnity(StringBuilder preSb, object message)
        {
            // var sb = GetSb();
            // sb.Append("<color=#").Append(UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.blue)).Append(">");
            // sb.Append(preSb.ToString()).Append(message);
            // sb.Append("</color>");
            // UnityEngine.Debug.Log(sb.ToString());
            // RetrunSb(sb);
        }

        private void InitMsgListen()
        {
            InitMsgFunction();
            InitEntityTrack();
            InitIgnoreMsg();
            InitSpeicalMsg();
        }

        private void InitWise()
        {
            _typeList.Add(eLogType.ewise);
        }

        private void InitSpeicalMsg()
        {
            AddSpecialMsgType(MsgType.KMsgGs2ClsyncEntitiesDataNotice);
        }

        private void InitIgnoreMsg()
        {
            // AddIgnoreMsgType(MsgType.KMsgGs2ClsyncEntitiesDataNotice);
            // AddIgnoreMsgType(MsgType.KMsgGs2ClentityMovePathNotice);
            // AddIgnoreMsgType(MsgType.KMsgGs2ClentityStopMoveNotice);
            // AddIgnoreMsgType(MsgType.KMsgCl2GskeepLiveRequest);
            // AddIgnoreMsgType(MsgType.KMsgGs2ClkeepLiveReply);
        }

        private void InitMsgFunction()
        {
            //InitWise();
            //InitHonorMsg();
            //InitScout();
            //InitExploreMsg();
            //InitGrowthFund();
            //InitHonorMsg2();
            //InitHXFDerbyMsg();
            //InitGuildResourceMsg();
            //InitIncidentMsg();
            //InitRuneMsg();
            //InitCityPerson();
            //InitCharter();
            //InitArenaShop();
            //InitTurnTable();
            //InitWaterStatus();
            //InitPompeii();
            //InitMail();
            //InitMagicLampNotice();
            //InitGatlin();
            //InitChristmasGame();
            //InitExSave();
            //InitNewPegie();
            //InitClean();
            InitRank();
        }

        private void InitRank()
        {
            AddListenMsgType(MsgType.KMsgCl2GsrankQueryRankBoardRequest);
            AddListenMsgType(MsgType.KMsgGs2ClrankQueryRankBoardReply);
        }

        private void InitClean()
        {
            // 只做表现，并非完整消息
            AddListenMsgType(MsgType.KMsgGs2ClcityInfoReply);
            AddListenMsgType(MsgType.KMsgGs2ClcityBuildingUpdateNotice);
            
            AddListenMsgType(MsgType.KMsgCl2GscityRemoveBuildingRequest);
            AddListenMsgType(MsgType.KMsgGs2ClcityRemoveBuildingReply);
            
            
        }

        public void TestShowSeniorRewardPanel()
        {
            var reward = new List<Resource>();
            foreach (var cfg in RewardDao.Inst.Configs)
            {
                var rewardVos = RewardDao.Inst.GetContentById(cfg.Id);
                foreach (var vo in rewardVos)
                {
                    reward.Add(new Resource(){ ResType = (uint)vo.Type,SubType = vo.Value,Value = vo.Count,});
                }
            }

            foreach (var cfg in ItemDao.Inst.Configs)
            {
                reward.Add(new Resource(){ ResType = (uint)PlayerAttributeType.KPlayerAttrItem,SubType = cfg.ItemId,Value = 2,});
            }
            
            
            //for(int i = 0; i < 10; i++)
            {
                // 英雄
                // reward.Add(new Resource(){ ResType = 11,SubType = 1063,Value = 2,});
                // reward.Add(new Resource(){ ResType = 2,SubType = 40000,Value = 41,});
                // reward.Add(new Resource(){ ResType = 2,SubType = 15001,Value = 1068,});
                // reward.Add(new Resource(){ ResType = 2,SubType = 20010,Value = 19,});
                // reward.Add(new Resource(){ ResType = 2,SubType = 21010,Value = 28,});
                // reward.Add(new Resource(){ ResType = 2,SubType = 40011,Value = 70,});
                // reward.Add(new Resource(){ ResType = 2,SubType = 20000,Value = 102,});
                // reward.Add(new Resource(){ ResType = 2,SubType = 14000,Value = 28,});
                // reward.Add(new Resource(){ ResType = 2,SubType = 23010,Value = 21,});
                // reward.Add(new Resource(){ ResType = 2,SubType = 21000,Value = 84,});
                // reward.Add(new Resource(){ ResType = 2,SubType = 14010,Value = 14,});
                // reward.Add(new Resource(){ ResType = 14,SubType = 141,Value = 1370,});
                // reward.Add(new Resource(){ ResType = 14,SubType = 109,Value = 1270,});
                // reward.Add(new Resource(){ ResType = 14,SubType = 143,Value = 1020,});
                // reward.Add(new Resource(){ ResType = 14,SubType = 113,Value = 1400,});
                // reward.Add(new Resource(){ ResType = 11,SubType = 1062,Value = 21,});
                // reward.Add(new Resource(){ ResType = 11,SubType = 1064,Value = 2,});
            }
            
            // 科技
            // reward.Add(new Resource(){ ResType = 24,SubType = 2051,Value = 1,});
            // reward.Add(new Resource(){ ResType = 24,SubType = 2051,Value = 2,});
            // reward.Add(new Resource(){ ResType = 24,SubType = 2051,Value = 3,});
            // reward.Add(new Resource(){ ResType = 24,SubType = 2052,Value = 2,});
            // reward.Add(new Resource(){ ResType = 24,SubType = 2052,Value = 3,});
            // reward.Add(new Resource(){ ResType = 24,SubType = 2053,Value = 4,});
            // reward.Add(new Resource(){ ResType = 24,SubType = 2053,Value = 4,});
            
            
            // reward.Add(new Resource()
            // {
            //     ResType = (uint)PlayerAttributeType.KPlayerAttrTechnology,
            //     SubType = 10001,
            //     Value = 1,
            // });
            // reward.Add(new Resource()
            // {
            //     ResType = (uint)PlayerAttributeType.KPlayerAttrHero,
            //     SubType = 1063,
            //     Value = 1,
            // });

            RewardModule.Inst.ShowSeniorRewardPanel(reward);
        }

        private void InitNewPegie()
        {
            AddListenMsgType(MsgType.KMsgGs2ClallHeroNotice);
            AddListenMsgType(MsgType.KMsgGs2ClupdateHeroNotice);
            AddListenMsgType(MsgType.KMsgGs2ClheroAttrNotice);
            
            //AddListenMsgType(MsgType.MsgGS2CLPersonalExchangeSpecialHeroNotice);
        }

        // 客户端独立存储这块，大小不超过8字节都支持
        private void InitExSave()
        {
            AddListenMsgType(MsgType.KMsgGs2ClplayerBaseNotice);
            AddListenMsgType(MsgType.KMsgCl2GsupdateShortClientDataRequest);
            AddListenMsgType(MsgType.KMsgGs2ClupdateShortClientDataReply);
            
        }

        private void InitChristmasGame()
        {
            AddListenMsgType(MsgType.KMsgGs2ClplayerAllActivityNotice);
            AddListenMsgType(MsgType.KMsgGs2ClplayerActivityBattlePassNotice);
            AddListenMsgType(MsgType.KMsgGs2ClplayerActivityBattlePassBaseInfoNotice);  
            AddListenMsgType(MsgType.KMsgCl2GsactivityBattlePassLevelRewardRequest); 
            AddListenMsgType(MsgType.KMsgGs2ClactivityBattlePassLevelRewardReply); 
            AddListenMsgType(MsgType.KMsgCl2GsactivityBattlePassUpgradeRequest); 
            AddListenMsgType(MsgType.KMsgCl2GsactivityBattlePassUpgradeReply); 
            AddListenMsgType(MsgType.KMsgCl2GsactivityBattlePassEmoneyAdvanceRequest); 
            AddListenMsgType(MsgType.KMsgCl2GsactivityBattlePassEmoneyAdvanceReply);
        }

        private void InitGatlin()
        {
            // 新乌托邦
            
            //AddListenMsgType(MsgType.KMsgGs2ClplayerAllPersonalActivityNotice);
            
            AddListenMsgType(MsgType.KMsgGs2ClpersonalGatlinKillRewardNotice);
            AddListenMsgType(MsgType.KMsgGs2ClpersonalGatlinInfoNotice);
            AddListenMsgType(MsgType.KMsgCl2GspersonalGatlinShotRequest);
            AddListenMsgType(MsgType.KMsgCl2GspersonalGatlinShotReply);
            AddListenMsgType(MsgType.KMsgCl2GspersonalGatlinStageAwardRequest);
            AddListenMsgType(MsgType.KMsgCl2GspersonalGatlinStageAwardReply);
             
            
            AddListenMsgType(MsgType.KMsgGs2ClplayerPersonalActivityNotice);
            AddListenMsgType(MsgType.KMsgGs2ClplayerPersonalRemoveNotice);
            //InitMail();
        }

        private void InitMagicLampNotice()
        {
            // 跑马灯
            AddListenMsgType(MsgType.KMsgGs2ClmagicLampNotice);
        }

        private void InitScout()
        {
            AddListenMsgType(MsgType.KMsgGs2ClsyncEntitiesDataNotice);

            m_entityTypes.Add(EntityType.KEntityTypeScout);
            m_compsBindDic.Add(typeof(ECompScoutData), typeof(ScoutData));

            m_entityTypes.Add(EntityType.KEntityTypeMarch);
            m_compsBindDic.Add(typeof(ECompMarchCommand), typeof(MarchCommand));
        }

        private void InitMail()
        {
            // 邮件
            AddListenMsgType(MsgType.KMsgGs2ClmailListReply);
            AddListenMsgType(MsgType.KMsgGs2ClmailNewNotice);
            AddListenMsgType(MsgType.KMsgMas2GssaddMailNotice);
        }

        private void InitPompeii()
        {
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiStatusNotice);
            AddListenMsgType(MsgType.KMsgCl2GspompeiiGetMainUiRequest);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiGetMainUiReply);
            AddListenMsgType(MsgType.KMsgCl2GspompeiiGetPreGuildResultRequest);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiGetPreGuildResultReply);
            AddListenMsgType(MsgType.KMsgCl2GspompeiiSubmitRegistRequest);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiSubmitRegistReply);
            AddListenMsgType(MsgType.KMsgCl2GspompeiiGetRegistDetailRequest);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiGetRegistDetailReply);
            AddListenMsgType(MsgType.KMsgCl2GspompeiiSetupRegistDetailRequest);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiSetupRegistDetailReply);
            AddListenMsgType(MsgType.KMsgCl2GspompeiiGetPairingInfoRequest);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiGetPairingInfoReply);
            AddListenMsgType(MsgType.KMsgCl2GspompeiiGetCandidateListRequest);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiGetCandidateListReply);
            AddListenMsgType(MsgType.KMsgCl2GspompeiiEventQueryRequest);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiEventQueryReply);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiDataNotice);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiBuildBattleDataNotice);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiBattleEndNotice);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiGuildInfoNotice);
            AddListenMsgType(MsgType.KMsgCl2GspompeiiGameBaseDataRequest); // 分数靠这个取
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiGameBaseDataReply);

            // 以下可能与流程无关
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiBossNotice);
            AddListenMsgType(MsgType.KMsgCl2GspompeiiGameDataRequest);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiGameDataReply);
            AddListenMsgType(MsgType.KMsgCl2GspompeiiIntoRequest);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiIntoReply);
            AddListenMsgType(MsgType.KMsgCl2GspompeiiLeaveRequest);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiLeaveReply);
            AddListenMsgType(MsgType.KMsgCl2GspompeiiGameBaseDataRequest);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiGameBaseDataReply);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiGuildBaseDataNotice);
            AddListenMsgType(MsgType.KMsgCl2GspompeiiTempLeaveRequest);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiTempLeaveReply);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiEndNotice);
            
            AddListenMsgType(MsgType.KMsgCl2GsguildBuildingListRequest);
            AddListenMsgType(MsgType.KMsgGs2ClguildBuildingListReply);
            
            
            // 战报
            AddListenMsgType(MsgType.KMsgCl2GspompeiiBattleReportRequest);
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiBattleReportReply);
            
            // 庞贝展示期
            AddListenMsgType(MsgType.KMsgGs2ClpompeiiGuildRewardInfoNotice);
            AddListenMsgType(MsgType.KMsgCl2GspompeiiGuildRewardGetRequest);
            AddListenMsgType(MsgType.KMsgCl2GspompeiiGuildRewardGetReply);
            AddListenMsgType(MsgType.KMsgGs2ClallActivityNotice);
        }

        private void InitWaterStatus()
        {
            AddListenMsgType(MsgType.KMsgGs2ClmapPlayerStatusInfoNotice);
            AddListenMsgType(MsgType.KMsgGs2ClmapPlayerUpdateStatusNotice);
            AddListenMsgType(MsgType.KMsgGs2ClmapPlayerRemoveStatusNotice);
        }

        private void InitTurnTable()
        {
            // 轮盘抽奖 非验证，自己猜测
            AddListenMsgType(MsgType.KMsgGs2ClplayerAllActivityNotice);
            AddListenMsgType(MsgType.KMsgGs2ClplayerActivityTurntableNotice);
            AddListenMsgType(MsgType.KMsgCl2GsactivityTurntableRequest);
            AddListenMsgType(MsgType.KMsgCl2GsactivityTurntableReply);
            AddListenMsgType(MsgType.KMsgCl2GsactivityTurntableStageRewardRequest);
            AddListenMsgType(MsgType.KMsgGs2ClactivityTurntableStageRewardReply);
        }

        private void InitArenaShop()
        {
            AddListenMsgType(MsgType.KMsgGs2ClplayerAttribute);
            AddListenMsgType(MsgType.KMsgCl2GsshopBuyItemRequest);
            AddListenMsgType(MsgType.KMsgGs2ClshopBuyItemReply);
            AddListenMsgType(MsgType.KMsgCl2GsshopRefreshRequest);
            AddListenMsgType(MsgType.KMsgGs2ClshopRefreshReply);
            AddListenMsgType(MsgType.KMsgGs2ClshopNotice);
            
            AddListenMsgType(MsgType.KMsgCl2GslocalArenaRankListRequest);
            AddListenMsgType(MsgType.KMsgGs2CllocalArenaRankListReply);
        }

        private void InitCityPerson()
        {
            AddListenMsgType(MsgType.KMsgGs2CltaskInfoReply);
            AddListenMsgType(MsgType.KMsgGs2CltaskCityTaskUpdateNotice);
            AddListenMsgType(MsgType.KMsgCl2GstaskCityTaskAwardRequest);
            AddListenMsgType(MsgType.KMsgGs2CltaskCityTaskAwardReply);
            AddListenMsgType(MsgType.KMsgCl2GstaskClientActionCompleteRequest);
            AddListenMsgType(MsgType.KMsgGs2CltaskCleintActionCompleteReply);
        }

        private void InitCharter()
        {
            AddListenMsgType(MsgType.KMsgCl2GstaskInfoRequest);
            AddListenMsgType(MsgType.KMsgGs2CltaskInfoReply);
            AddListenMsgType(MsgType.KMsgGs2CltaskMainTaskUpdateNotice);
            AddListenMsgType(MsgType.KMsgCl2GstaskMainTaskAwardRequest);
            AddListenMsgType(MsgType.KMsgGs2CltaskMainTaskAwardReply);
            AddListenMsgType(MsgType.KMsgGs2CltaskOpenChapterNotice);
            AddListenMsgType(MsgType.KMsgCl2GstaskChapterAwardRequest);
            AddListenMsgType(MsgType.KMsgGs2CltaskChapterAwardReply);
        }

        private void InitRuneMsg()
        {
            AddListenMsgType(MsgType.KMsgGs2ClsyncEntitiesDataNotice);

            m_entityTypes.Add(EntityType.KEntityTypeGenericRune);
            m_compsBindDic.Add(typeof(ECompRuneMapData), typeof(RuneMapData));

            m_entityTypes.Add(EntityType.KEntityTypeMarch);
            m_compsBindDic.Add(typeof(ECompPickUp), typeof(MarchPickUpData));
            m_compsBindDic.Add(typeof(ECompMarchCommand), typeof(MarchCommand));
        }

        private void InitEntityTrack()
        {
            //m_checkEntitys.Add(123123);
        }

        private void InitIncidentMsg()
        {
            AddListenMsgType(MsgType.KMsgGs2ClradarAllInfoNotice);
            AddListenMsgType(MsgType.KMsgGs2ClradarBaseInfoNotice);
            AddListenMsgType(MsgType.KMsgGs2ClradarInfoNotice);
            AddListenMsgType(MsgType.KMsgGs2CldelRadarNotice);
            AddListenMsgType(MsgType.KMsgCl2GsradarAcceptRequest);
            AddListenMsgType(MsgType.KMsgGs2ClradarAcceptReply);
            AddListenMsgType(MsgType.KMsgCl2GsradarRewardRequest);
            AddListenMsgType(MsgType.KMsgGs2ClradarRewardReply);
            AddListenMsgType(MsgType.KMsgGs2ClvirtualQueueAllInfoNotice);
            AddListenMsgType(MsgType.KMsgGs2ClvirtualQueueUpdateInfoNotice);
            AddListenMsgType(MsgType.KMsgCl2GsradarCompleteRequest);
            AddListenMsgType(MsgType.KMsgGs2ClradarCompleteReply);
            AddListenMsgType(MsgType.KMsgCl2GsradarRefreshRequest);
            AddListenMsgType(MsgType.KMsgGs2ClradarRefreshReply);
        }

        private void InitGuildResourceMsg()
        {
            AddListenMsgType(MsgType.KMsgCl2GsguildQueryPlayerGuildResourceRequest);
            AddListenMsgType(MsgType.KMsgGs2ClguildQueryPlayerGuildResourceReply);
            AddListenMsgType(MsgType.KMsgCl2GsguildCollectPlayerGuildResourceRequest);
            AddListenMsgType(MsgType.KMsgGs2ClguildCollectPlayerGuildResourceReply);
        }

        private void InitHXFDerbyMsg()
        {
            AddListenMsgType(MsgType.KMsgCl2GsexchangeHeroDebrisRequest);
            AddListenMsgType(MsgType.KMsgGs2ClexchangeHeroDebrisReply);
            AddListenMsgType(MsgType.KMsgCl2GsguildDerbyBuyCountRequest);
            AddListenMsgType(MsgType.KMsgGs2ClguildDerbyBuyCountReply);
            AddListenMsgType(MsgType.KMsgCl2GscityUpgradeBuidlingRequest);
            AddListenMsgType(MsgType.KMsgGs2ClcityUpgradeBuidlingReply);
            AddListenMsgType(MsgType.KMsgCl2GsguildDerbySubmitTaskRequest);
            AddListenMsgType(MsgType.KMsgGs2ClguildDerbySubmitTaskReply);
            AddListenMsgType(MsgType.KMsgCl2GsguildDerbyQuitTaskRequest);
            AddListenMsgType(MsgType.KMsgGs2ClguildDerbyQuitTaskReply);
            AddListenMsgType(MsgType.KMsgCl2GsguildDerbyAcceptTaskRequest);
            AddListenMsgType(MsgType.KMsgGs2ClguildDerbyAcceptTaskReply);
            AddListenMsgType(MsgType.KMsgGs2ClguildDerbyPlayerInfoNotice);
            AddListenMsgType(MsgType.KMsgGs2ClguildDerbyTaskNotice);
            AddListenMsgType(MsgType.KMsgCl2GsguildDerbyGetRewardRequest);
            AddListenMsgType(MsgType.KMsgGs2ClguildDerbyGetRewardReply);
            AddListenMsgType(MsgType.KMsgGs2ClguildDerbyScoreNotice);
        }

        private void InitGrowthFund()
        {
            AddListenMsgType(MsgType.KMsgCl2GsgrowthFundRewardRequest);
            AddListenMsgType(MsgType.KMsgCl2GsgrowthFundRewardReply);
            AddListenMsgType(MsgType.KMsgGs2ClbuyGrowthFundNotice);
            AddListenMsgType(MsgType.KMsgGs2ClmallAllDataNotice);
            
            
            // 城建 不完整，很多都直接走taskMoudle内部那套了
            AddListenMsgType(MsgType.KMsgGs2ClactivityStatusNotice);
            AddListenMsgType(MsgType.KMsgCl2GstaskBranchTaskAwardRequest);
            AddListenMsgType(MsgType.KMsgGs2CltaskBranchTaskAwardReply);
            AddListenMsgType(MsgType.KMsgGs2CltaskInfoReply);
        }

        private void InitExploreMsg()
        {
            AddListenMsgType(MsgType.KMsgCl2GsexploreRequest);
            AddListenMsgType(MsgType.KMsgGs2ClexploreReply);
            AddListenMsgType(MsgType.KMsgGs2ClexploreUpdateVillageNotice);
            AddListenMsgType(MsgType.KMsgGs2ClexploreUpdateCaveNotice);
            AddListenMsgType(MsgType.KMsgGs2ClexploreUpdateBuildingNotice);
            AddListenMsgType(MsgType.KMsgGs2ClexploreAwardReply);
            AddListenMsgType(MsgType.KMsgCl2GsexploreVisitReply);
            AddListenMsgType(MsgType.KMsgGs2ClexploreBatchAwardReply);
            AddListenMsgType(MsgType.KMsgGs2ClmistOpenBuildingMistReply);
            AddListenMsgType(MsgType.KMsgGs2ClexploreCaveExtraAwardReply);
            AddListenMsgType(MsgType.KMsgCl2GsexploreCaveExtraAwardRequest);
            AddListenMsgType(MsgType.KMsgCl2GsmistOpenBuildingMistRequest);
            AddListenMsgType(MsgType.KMsgCl2GsexploreBatchAwardRequest);
            AddListenMsgType(MsgType.KMsgCl2GsexploreAwardRequest);
            AddListenMsgType(MsgType.KMsgCl2GsexploreVisitRequest);
        }

        private void InitHonorMsg()
        {
            AddListenMsgType(MsgType.KMsgGs2ClplayerAllActivityNotice);
            AddListenMsgType(MsgType.KMsgGs2ClplayerActivityHonorNotice);
            AddListenMsgType(MsgType.KMsgGs2ClplayerActivityHonorBaseNotice);
            AddListenMsgType(MsgType.KMsgCl2GsactivityHonorTaskRewardRequest);
            AddListenMsgType(MsgType.KMsgCl2GsactivityHonorTaskRewardReply);
            AddListenMsgType(MsgType.KMsgCl2GsactivityHonorLevelRewardRequest);
            AddListenMsgType(MsgType.KMsgCl2GsactivityHonorLevelRewardReply);
            AddListenMsgType(MsgType.KMsgCl2GsactivityHonorBuyLevelRequest);
            AddListenMsgType(MsgType.KMsgCl2GsactivityHonorBuyLevelReply);
            AddListenMsgType(MsgType.KMsgGs2ClplayerActivityTaskUpdateNotice);
            AddListenMsgType(MsgType.KMsgGs2ClplayerActivityTaskUpdateNotice);
            AddListenMsgType(MsgType.KMsgGs2ClmailListReply);
        }

        private void InitHonorMsg2()
        {
            AddListenMsgType(MsgType.KMsgGs2ClhonroAllInfoNotice);
            AddListenMsgType(MsgType.KMsgGs2ClhonorBaseInfoNotice);
            AddListenMsgType(MsgType.KMsgGs2ClhonorTaskInfoNotice);
            AddListenMsgType(MsgType.KMsgCl2GshonorTaskRewardRequest);
            AddListenMsgType(MsgType.KMsgGs2ClhonorTaskRewardReply);
            AddListenMsgType(MsgType.KMsgCl2GshonorLevelRewardRequest);
            AddListenMsgType(MsgType.KMsgGs2ClhonorLevelRewardReply);
            AddListenMsgType(MsgType.KMsgCl2GshonorBuyLevelRequest);
            AddListenMsgType(MsgType.KMsgGs2ClhonorBuyLevelReply);
        }

        public void AddSpecialMsgType(MsgType eType)
        {
            if (m_speicalList.Contains(eType)) return;
            m_speicalList.Add(eType);
        }

        public void AddIgnoreMsgType(MsgType eType)
        {
            if (m_ignoreMsgList.Contains(eType)) return;
            m_ignoreMsgList.Add(eType);
        }

        public void AddListenMsgType(MsgType eType)
        {
            if (m_MsgList.Contains(eType)) return;
            m_MsgList.Add(eType);
        }

        public void PrintMsg(IMessage msg, bool bSend = false)
        {
            if (msg == null) return;
            var msgType = (MsgType)MsgMap.GetIdByType(msg.GetType());

            if (m_ignoreMsgList.Contains(msgType)) return;
            if (!_allMsg && !m_MsgList.Contains(msgType)) return;

            if (m_speicalList.Contains(msgType))
            {
                if (bSend)
                    PrintSpeical("Send " + (int)msgType + " ==>" + msgType + " ", msg, eLogType.eRootNetwork);
                else
                    PrintSpeical("Recv " + (int)msgType + " ==>" + msgType + " ", msg, eLogType.eRootNetwork);

                return;
            }


            if (bSend)
                Print("Send " + (int)msgType + " ==>" + msgType + " " + msg, eLogType.eRootNetwork);
            else
                Print("Recv " + (int)msgType + " ==>" + msgType + " " + msg, eLogType.eRootNetwork);
        }


        public void PrintSpeical(object message, IMessage msg, eLogType eType = eLogType.eLog, bool bAppend = true)
        {
            if (!_typeList.Contains(eType))
                return;

            var preSb = GetSb();
            preSb.Append(DateTime.Now.ToString("HH:mm:ss:fff")).Append(" [").Append(eType.ToString()).Append("] ");


            message += getMsgGS2CLSyncEntitiesDataNotice(msg as MsgGS2CLSyncEntitiesDataNotice);
            if ((eLogOut.eUnity & (eLogOut)_outType) == eLogOut.eUnity) PrintUnity(preSb, message);

            if ((eLogOut.eFile & (eLogOut)_outType) == eLogOut.eFile) WriteFile(preSb, message, bAppend);
            RetrunSb(preSb);
        }

        private string getMsgGS2CLSyncEntitiesDataNotice(MsgGS2CLSyncEntitiesDataNotice msg)
        {
            var bInCheck = true;
            if (msg == null) return "Msg is not MsgGS2CLSyncEntitiesDataNotice";
            var sb = SbPool.Get();
            var hasRegion = AppCache.WorldMap.TryUseWorldContext(msg.RegionId, out var context, false);
            sb.Append("{RegionId=").Append(msg.RegionId).Append(hasRegion ? "" : "(!exist)");

            var syncBinder = AppCache.WorldMap.SyncBinder;
            var bytes = msg.SyncData.ToByteArray();
            var input = new ByteArray(bytes);
            var entityData = AppCache.WorldMap.CacheSyncData;

            var count = input.ReadU32();
            sb.Append(", Count=").Append(count).Append(" ===");

            for (var i = 0; i < count; i++)
            {
                //sb.AppendLine();
                entityData.Read(input);
                sb.Append("{ ---EntiyId = ").Append(entityData.Id).Append(",").Append(entityData.Type).Append(",");

                if (m_checkEntitys.Count > 0 && !m_checkEntitys.Contains(entityData.Id)) continue;

                if (!m_entityTypes.Contains(entityData.Type) && !m_bAllEntityComp) continue;

                sb.Append(" Comp{ len=").Append(entityData.Components.Length).Append("}");

                for (var j = 0; j < entityData.Components.Length; j++)
                {
                    //sb.AppendLine().Append("\t");
                    var compData = entityData.Components[j];
                    var bindVo = syncBinder.GetByPropType(compData.Type);
                    if (bindVo == null)
                    {
                        sb.Append("[unbind]Components, type=").Append(compData.Type);
                        continue;
                    }

                    if (!m_compsBindDic.TryGetValue(bindVo.CompType, out var dataType) && !m_bAllEntityComp)
                    {
                        sb.Append(" ... ");
                        continue;
                    }

                    bInCheck = true;
                    var data = Activator.CreateInstance(dataType);
                    if (data is IMessage iMsg)
                    {
                        iMsg.MergeFrom(compData.Content);
                        sb.Append("{").Append(dataType).Append(":").Append(iMsg).Append("}");
                    }
                }

                sb.Append("}");
                entityData.Reset();
            }

            sb.Append("}");

            if (!bInCheck)
            {
                sb.Clear();
                sb.Append("==========");
            }

            return SbPool.PutAndToStr(sb);
        }
    }
}