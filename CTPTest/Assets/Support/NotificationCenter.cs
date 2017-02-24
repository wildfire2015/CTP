using System;
using System.Collections.Generic;
using PSupport.PSingleton;

namespace PSupport
{

    using SenderTable = Dictionary<object, List<MsgHandler>>;
    /// <summary>
    /// 消息回调
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="msg"></param>
    public delegate void MsgHandler(object sender, object msg);
    public class NotificationCenter : Singleton<NotificationCenter>
    {
        private Dictionary<string, SenderTable> _mtable = new Dictionary<string, SenderTable>();
        private HashSet<List<MsgHandler>> _minvoking = new HashSet<List<MsgHandler>>();

        public NotificationCenter() { }

        /// <summary>
        /// 添加一个观察者，不指定发送者，发送者默认为NotificationCenter
        /// </summary>
        /// <param name="handler">EventHandler</param>
        /// <param name="notificationName">NotificationName</param>
        public void addObserver(MsgHandler handler, string notificationName)
        {
            addObserver(handler, notificationName, null);
        }

        /// <summary>
        /// 添加一个观察者，并指定发送者
        /// </summary>
        public void addObserver(MsgHandler handler, string notificationName, System.Object sender)
        {
            if (handler == null)
            {
                DLoger.LogError("Can't add a null event handler for notification, " + notificationName);
                return;
            }

            if (string.IsNullOrEmpty(notificationName))
            {
                DLoger.LogError("Can't observe an unnamed notification");
                return;
            }

            if (!_mtable.ContainsKey(notificationName))
                _mtable.Add(notificationName, new SenderTable());

            SenderTable subTable = _mtable[notificationName];

            System.Object key = (sender != null) ? sender : this;

            if (!subTable.ContainsKey(key))
                subTable.Add(key, new List<MsgHandler>());

            List<MsgHandler> list = subTable[key];
            if (!list.Contains(handler))
            {
                if (_minvoking.Contains(list))
                    subTable[key] = list = new List<MsgHandler>(list);

                list.Add(handler);
            }
        }

        /// <summary>
        /// 派发一条通知
        /// </summary>
        public void postNotification(string notificationName)
        {
            postNotification(notificationName, null);
        }

        /// <summary>
        /// 派发一条通知 并指定发送者
        /// </summary>
        public void postNotification(string notificationName, System.Object sender)
        {
            postNotification(notificationName, sender, null);
        }

        /// <summary>
        /// 派发一条通知 并指定发送者, 指定发送信息
        /// </summary>
        public void postNotification(string notificationName, System.Object sender, System.Object e)
        {
            if (string.IsNullOrEmpty(notificationName))
            {
                DLoger.LogError("A notification name is required");
                return;
            }

            // No need to take action if we dont monitor this notification
            if (!_mtable.ContainsKey(notificationName))
                return;

            // Post to subscribers who specified a sender to observe
            SenderTable subTable = _mtable[notificationName];
            if (sender != null && subTable.ContainsKey(sender))
            {
                List<MsgHandler> handlers = subTable[sender];
                _minvoking.Add(handlers);
                for (int i = 0; i < handlers.Count; ++i)
                    handlers[i](sender, e);
                _minvoking.Remove(handlers);
            }

            // Post to subscribers who did not specify a sender to observe
            if (subTable.ContainsKey(this))
            {
                List<MsgHandler> handlers = subTable[this];
                _minvoking.Add(handlers);
                for (int i = 0; i < handlers.Count; ++i)
                    handlers[i](sender, e);
                _minvoking.Remove(handlers);
            }
        }

        public void removeObserver(MsgHandler handler, string notificationName)
        {
            removeObserver(handler, notificationName, null);
        }

        public void removeObserver(MsgHandler handler, string notificationName, System.Object sender)
        {
            if (handler == null)
            {
                DLoger.LogError("Can't remove a null event handler for notification, " + notificationName);
                return;
            }

            if (string.IsNullOrEmpty(notificationName))
            {
                DLoger.LogError("A notification name is required to stop observation");
                return;
            }

            // No need to take action if we dont monitor this notification
            if (!_mtable.ContainsKey(notificationName))
                return;

            SenderTable subTable = _mtable[notificationName];
            System.Object key = (sender != null) ? sender : this;

            if (!subTable.ContainsKey(key))
                return;

            List<MsgHandler> list = subTable[key];
            int index = list.IndexOf(handler);
            if (index != -1)
            {
                if (_minvoking.Contains(list))
                    subTable[key] = list = new List<MsgHandler>(list);
                list.RemoveAt(index);
            }
        }

        public void clean()
        {
            string[] notKeys = new string[_mtable.Keys.Count];
            _mtable.Keys.CopyTo(notKeys, 0);

            for (int i = notKeys.Length - 1; i >= 0; --i)
            {
                string notificationName = notKeys[i];
                SenderTable senderTable = _mtable[notificationName];

                object[] senKeys = new object[senderTable.Keys.Count];
                senderTable.Keys.CopyTo(senKeys, 0);

                for (int j = senKeys.Length - 1; j >= 0; --j)
                {
                    object sender = senKeys[j];
                    List<MsgHandler> handlers = senderTable[sender];
                    if (handlers.Count == 0)
                        senderTable.Remove(sender);
                }

                if (senderTable.Count == 0)
                    _mtable.Remove(notificationName);
            }
        }
    }
}