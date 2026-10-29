using System;
using System.Collections.Generic;
using Google.Protobuf;
using IGG.Framework;
using IGG.Framework.Network;
using IGG.Game.Managers.Network;

namespace HHL.Common
{
    /// <summary>
    /// 自定义网络 dump 监听（HHL 侧代码）。
    /// 通过 NetworkMgr.StatsMgr（INetworkStats）注册到网络统计框架，
    /// 把收发报文交给 HHL 日志（eLogType.eRootNetwork），
    /// 从而无需改动 GameNetwork。打印统一在主线程 Update 里做（消息线程只缓存）。
    /// </summary>
    public class HHLNetworkDumpRecorder : INetworkStats, IUpdater
    {
        private readonly object m_lock = new object();
        private readonly Queue<KeyValuePair<IMessage, bool>> m_queue = new Queue<KeyValuePair<IMessage, bool>>();

        /// <summary>
        /// 注册到 NetworkMgr.StatsMgr，重复调用安全。
        /// StatsMgr 在 GameLaunch.Awake 中创建，之后任何时刻调用都可以。
        /// </summary>
        public static void Install()
        {
            var statsMgr = NetworkMgr.StatsMgr;
            if (statsMgr == null || statsMgr.HasStats(typeof(HHLNetworkDumpRecorder)))
            {
                return;
            }

            statsMgr.AddStats(new HHLNetworkDumpRecorder());
        }

        /// <summary>
        /// HHL 自启动：首个场景加载后（GameLaunch.Awake 之后、网络创建之前）自动装配，
        /// 游戏侧无需任何调用。仅 Editor 生效（与旧有 #if UNITY_EDITOR 行为一致）。
        /// </summary>
#if UNITY_EDITOR
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInstall()
        {
            Install();
        }
#endif

        // —— 消息线程：只缓存，不调用 Unity / HHL Log ——

        public void OnConnectBefore(string ip)
        {
        }

        public void OnConnectAfter(string ip, ConnectResult result)
        {
        }

        public void OnClose(string ip, Exception e = null)
        {
        }

        public void OnReceiveBytesAfter(ReadOnlySpan<byte> bytes)
        {
        }

        public void OnReceiveMsgAfter(ReadOnlySpan<byte> msgBytes, object msg, int msgId, ECompressPkgType compressPkgType,
                                      int originalSize, int innerIndex)
        {
            Capture(msg, false);
        }

        public void OnSendAfter(ReadOnlySpan<byte> msgBytes, object msg, int msgId, int sendSize)
        {
            Capture(msg, true);
        }

        private void Capture(object msg, bool bSend)
        {
            if (msg is IMessage imsg)
            {
                lock (m_lock)
                {
                    m_queue.Enqueue(new KeyValuePair<IMessage, bool>(imsg, bSend));
                }
            }
        }

        // —— 主线程：复用 PrintMsg 的过滤与格式，打印到 HHL.log ——

        public void Update()
        {
            KeyValuePair<IMessage, bool>[] items;
            lock (m_lock)
            {
                if (m_queue.Count == 0)
                {
                    return;
                }

                items = m_queue.ToArray();
                m_queue.Clear();
            }

            // 网络日志类型未开启时丢弃，避免队列/字符串无谓开销
            if (!Log.Inst.IsOpen(Log.eLogType.eRootNetwork))
            {
                return;
            }

            for (int i = 0; i < items.Length; i++)
            {
                Log.Inst.PrintMsg(items[i].Key, items[i].Value);
            }
        }
    }
}
