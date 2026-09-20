using System;
using System.Collections.Generic;
using System.Net;
using Google.Protobuf;
using IGG.Framework.Network;
using IGG.Game.Managers.Network;

namespace HHL.Common
{
    /// <summary>
    /// 自定义网络 dump 监听（HHL 侧代码）。
    /// 通过 NetworkMgr.DumpRecorder 装配，把收发报文交给 HHL 日志（eLogType.eRootNetwork），
    /// 从而无需改动 GameNetwork。打印统一在主线程 OnRevDump 里做（消息线程只缓存）。
    /// </summary>
    public class HHLNetworkDumpRecorder : INetworkDumpRecorder
    {
        // 被装饰的内置 recorder（可能为 null），所有回调都会转发给它，保证旧有行为不受影响
        private readonly INetworkDumpRecorder m_inner;

        private readonly object m_lock = new object();
        private readonly Queue<KeyValuePair<IMessage, bool>> m_queue = new Queue<KeyValuePair<IMessage, bool>>();

        public HHLNetworkDumpRecorder(INetworkDumpRecorder inner)
        {
            m_inner = inner;
        }

        /// <summary>
        /// 包住当前的 DumpRecorder（可能是内置的，也可能为 null）。
        /// 必须在网络创建（new GameNetwork）之前调用；重复调用安全。
        /// </summary>
        public static void Install()
        {
            if (NetworkMgr.DumpRecorder is HHLNetworkDumpRecorder)
            {
                return;
            }

            NetworkMgr.DumpRecorder = new HHLNetworkDumpRecorder(NetworkMgr.DumpRecorder);
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

        // —— 消息线程：只缓存 + 转发，不调用 Unity / HHL Log ——

        public void OnRecvAfter(byte[] msgBytes, object msg, out object msgCopy, out int msgId)
        {
            if (m_inner != null)
            {
                m_inner.OnRecvAfter(msgBytes, msg, out msgCopy, out msgId);
            }
            else
            {
                msgCopy = null;
                msgId = -1;
            }

            Capture(msg, false, ref msgCopy, ref msgId);
        }

        public void OnSendAfter(byte[] msgBytes, object msg, out object msgCopy, out int msgId)
        {
            if (m_inner != null)
            {
                m_inner.OnSendAfter(msgBytes, msg, out msgCopy, out msgId);
            }
            else
            {
                msgCopy = null;
                msgId = -1;
            }

            Capture(msg, true, ref msgCopy, ref msgId);
        }

        private void Capture(object msg, bool bSend, ref object msgCopy, ref int msgId)
        {
            msgCopy = msg == null ? "null" : msg.ToString();
            msgId = -1;

            if (msg is IMessage imsg)
            {
                lock (m_lock)
                {
                    m_queue.Enqueue(new KeyValuePair<IMessage, bool>(imsg, bSend));
                }
            }
        }

        public void OnConnectBefore(string ip)
        {
            m_inner?.OnConnectBefore(ip);
        }

        public void OnConnectAfter(string ip, ConnectResult result)
        {
            m_inner?.OnConnectAfter(ip, result);
        }

        public void OnClose(IPEndPoint address, Exception e = null)
        {
            m_inner?.OnClose(address, e);
        }

        // —— 主线程：复用 PrintMsg 的过滤与格式，打印到 HHL.log ——

        public void OnRevDump(MessageDump[] dumps)
        {
            m_inner?.OnRevDump(dumps);

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
