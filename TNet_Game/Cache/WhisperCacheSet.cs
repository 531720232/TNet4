

using System.Collections.Generic;
using TNet.Cache.Generic;
using TNet.Log;
using TNet.Model;

namespace TNet.Cache
{
    /// <summary>
    /// 私聊缓存
    /// </summary>
    public class WhisperCacheSet : ShareCacheStruct<ChatMessage>
    {
		/// <summary>
		/// 加载数据工厂
		/// </summary>
		/// <returns></returns>
        protected override bool LoadFactory(bool isReplace)
        {
            return true;
        }

        /// <summary>
        /// 加载子项数据工厂
        /// </summary>
        /// <returns></returns>
        /// <param name="key">Key.</param>
        /// <param name="isReplace"></param>
        protected override bool LoadItemFactory(string key, bool isReplace)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="periodTime"></param>
        /// <param name="isReplace"></param>
        /// <returns></returns>
        protected override bool InitCache(List<ChatMessage> dataList, int periodTime, bool isReplace)
        {
            bool result = false;
            foreach (ChatMessage data in dataList)
            {
                string groupKey = data.ToUserID.ToString();
                result = DataContainer.TryAddQueue(groupKey, data, periodTime, OnExpired);
                if (!result)
                {
                    TraceLog.WriteError("Load data:\"{0}\" tryadd key:\"{1}\" error.", DataContainer.RootKey, groupKey);
                    return false;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        public void Add(int userId, ChatMessage message)
        {
            string groupKey = userId.ToString();
            DataContainer.TryAddQueue(groupKey, message, 0, OnExpired);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool HasMessage(int userId)
        {
            string groupKey = userId.ToString();
            CacheQueue<ChatMessage> chatQueue;
            if (DataContainer.TryGetQueue(groupKey, out chatQueue))
            {
                return chatQueue.Count > 0;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ChatMessage[] GetMessage(int userId)
        {
            string groupKey = userId.ToString();
            CacheQueue<ChatMessage> chatQueue;
            if (DataContainer.TryGetQueue(groupKey, out chatQueue))
            {
                ChatMessage[] chatList;
                if (chatQueue.TryDequeueAll(out chatList))
                {
                    return chatList;
                }
            }
            return new ChatMessage[0];
        }

        private static bool OnExpired(string groupKey, CacheQueue<ChatMessage> messageQueue)
        {
            return true;
        }

    }
}