using System;
using System.Collections.Generic;
using System.Text;
using IGG.Game.Managers.Network;
using Msgtype;
using Google.Protobuf;
using IGG.Framework.Cache;
using IGG.Framework.IO;
using IGG.Game.Data.Cache;
using IGG.Game.Data.Cache.WorldMap.Entity.Comp;
using IGG.Game.Module.Rune.Comp;
using Protomsg;

namespace HHL.Common
{
    public partial class Log
    {
        // 写文件单行字符长度,stringbuilder长度现在默认是200,所以别超过200
        private int _fileTxtLen = 120;
        private bool _allMsg = false;
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
         }
         
         public enum eLogOut
         {
             eNone = 0,
             eFile = 1,
             eUnity = 1<<1,
         }

         private void InitExtend()
         {
             //_filePath = System.Environment.CurrentDirectory + "\\Assets\\Scripts\\Game\\HHL" + "\\HHL.log";
             _filePath = System.Environment.CurrentDirectory + "\\output" + "\\HHL.log";
             
             _typeList.Add(eLogType.eLog);
             _typeList.Add(eLogType.eWarning);
             _typeList.Add(eLogType.eErro);
             _typeList.Add(eLogType.eRootNetwork);

             _outType |= (uint) eLogOut.eFile ;
             //_outType |= (uint) eLogOut.eUnity ;

             InitMsgListen();
             
             var strFu = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
             Print("", eLogType.eLog, false);
             Print("-----HHL Log----------" + strFu + "--------------");
         }

         public void PrintUnity(StringBuilder preSb,object message)
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
             
             //InitHonorMsg();
             //InitExploreMsg();
             //InitGrowthFund();
             //InitHonorMsg2();
             //InitHXFDerbyMsg();
             //InitGuildResourceMsg();
             //InitIncidentMsg();
             //InitRuneMsg();
             //InitCityPerson();
             InitCharter();
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
             m_compsBindDic.Add(typeof(ECompRuneMapData),typeof(RuneMapData));
             
             m_entityTypes.Add(EntityType.KEntityTypeMarch);
             m_compsBindDic.Add(typeof(ECompPickUp),typeof(MarchPickUpData));
             m_compsBindDic.Add(typeof(ECompMarchCommand),typeof(MarchCommand));
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

         private void InitGrowthFund(){
             AddListenMsgType(MsgType.KMsgCl2GsgrowthFundRewardRequest);
             AddListenMsgType(MsgType.KMsgCl2GsgrowthFundRewardReply);
             AddListenMsgType(MsgType.KMsgGs2ClbuyGrowthFundNotice);
             AddListenMsgType(MsgType.KMsgGs2ClmallAllDataNotice);
             
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
             if (m_speicalList.Contains(eType))
             {
                 return;
             }
             m_speicalList.Add(eType);
         }
         public void AddIgnoreMsgType(MsgType eType)
         {
             if (m_ignoreMsgList.Contains(eType))
             {
                 return;
             }
             m_ignoreMsgList.Add(eType);
         }

         public void AddListenMsgType(MsgType eType)
         {
             if (m_MsgList.Contains(eType))
             {
                 return;
             }
             m_MsgList.Add(eType);
         }
         
         public void PrintMsg(IMessage msg, bool bSend = false)
         {
             if (msg == null) return;
             var msgType = (MsgType) MsgMap.GetIdByType(msg.GetType());

             if (m_ignoreMsgList.Contains(msgType)) return;
             if (!_allMsg && !m_MsgList.Contains(msgType)) return;

             if (m_speicalList.Contains(msgType))
             {
                 if (bSend)
                     PrintSpeical("Send " + (int) msgType + " ==>" + msgType + " " ,msg, eLogType.eRootNetwork);
                 else
                     PrintSpeical("Recv " + (int) msgType + " ==>" + msgType + " " ,msg, eLogType.eRootNetwork);
                 
                 return;
             }
             
             
             if (bSend)
                 Print("Send " + (int) msgType + " ==>" + msgType + " " + msg, eLogType.eRootNetwork);
             else
                 Print("Recv " + (int) msgType + " ==>" + msgType + " " + msg, eLogType.eRootNetwork);
         }
         
         
         public void PrintSpeical(object message,IMessage msg,eLogType eType = eLogType.eLog, bool bAppend = true)
         {
             if (!_typeList.Contains(eType))
                 return;

             StringBuilder preSb = GetSb();
             preSb.Append(DateTime.Now.ToString("HH:mm:ss:fff")).Append(" [").Append(eType.ToString()).Append("] ");


             message += getMsgGS2CLSyncEntitiesDataNotice(msg as MsgGS2CLSyncEntitiesDataNotice);
             if ((eLogOut.eUnity & (eLogOut)_outType) == eLogOut.eUnity)
             {
                 PrintUnity(preSb,message);
             }
            
             if ((eLogOut.eFile & (eLogOut)_outType) == eLogOut.eFile)
             {
                 WriteFile(preSb,message,bAppend);
             } 
             RetrunSb(preSb);
         }
         
        private  string getMsgGS2CLSyncEntitiesDataNotice(MsgGS2CLSyncEntitiesDataNotice msg)
        {
            bool bInCheck = false;
            if (msg == null)
            {
                return "Msg is not MsgGS2CLSyncEntitiesDataNotice";
            }
            var sb = SbPool.Get();
            bool hasRegion = AppCache.WorldMap.TryUseWorldContext(msg.RegionId, out var context, false);
            sb.Append("{RegionId=").Append(msg.RegionId).Append(hasRegion ? "" : "(!exist)");

            var syncBinder = AppCache.WorldMap.SyncBinder;
            var bytes = msg.SyncData.ToByteArray();
            var input = new ByteArray(bytes);
            var entityData = AppCache.WorldMap.CacheSyncData;
            
            var count = input.ReadU32();
            sb.Append(", Count=").Append(count).Append(" ===");

            for (int i = 0; i < count; i++)
            {
                //sb.AppendLine();
                entityData.Read(input);
                sb.Append("{ ---EntiyId = ").Append(entityData.Id).Append(",").Append(entityData.Type).Append(",");
                
                if (m_checkEntitys.Count > 0 && !m_checkEntitys.Contains(entityData.Id))
                {
                    continue;
                } 
                
                if (!m_entityTypes.Contains(entityData.Type))
                {
                    continue;
                }
                
                sb.Append(" Comp{ len=").Append(entityData.Components.Length).Append("}");
                
                for (int j = 0; j < entityData.Components.Length; j++)
                {
                    //sb.AppendLine().Append("\t");
                    var compData = entityData.Components[j];
                    var bindVo = syncBinder.GetByPropType(compData.Type);
                    if (bindVo == null)
                    {
                        sb.Append("[unbind]Components, type=").Append(compData.Type);
                        continue;
                    }

                    if (!m_compsBindDic.TryGetValue(bindVo.CompType, out var dataType))
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