﻿
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using TNet.Configuration;
using TNet.Extend;
using TNet.Log;
using TNet.Redis;
using TNet.Serialization;

namespace TNet.Sns.Service
{
    /// <summary>
    /// 
    /// </summary>
    public static class HandlerManager
    {
        private const string AccountServerToken = "__AccountToken";
        /// <summary>
        /// 
        /// </summary>
        public static string SignKey;
        /// <summary>
        /// 
        /// </summary>
        public static string ClientDesDeKey;
        /// <summary>
        /// 
        /// </summary>
        public static string RedisHost;
        /// <summary>
        /// 
        /// </summary>
        public static int RedisDb;
        private static Dictionary<string, Type> handlerTypes;
        private static ConcurrentDictionary<string, UserToken> userTokenCache = new ConcurrentDictionary<string, UserToken>();
        private static ConcurrentDictionary<int, string> userHashCache = new ConcurrentDictionary<int, string>();
        private static bool RedisConnected;

        static HandlerManager()
        {
            SignKey = ConfigUtils.GetSetting("Product.SignKey", "");
            ClientDesDeKey = ConfigUtils.GetSetting("Product.ClientDesDeKey", "");
            RedisHost = ConfigUtils.GetSetting("Redis.Host", "");
            RedisDb = ConfigUtils.GetSetting("Redis.Db", 0);
            handlerTypes = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        public static void Init(Assembly assembly)
        {
            var htype = typeof(IHandler<>);
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.GetInterfaces().Select(t => t.Name).Contains(htype.Name))
                {
                    handlerTypes[type.Name] = type;
                }
            }
            if (!string.IsNullOrEmpty(RedisHost))
            {
                try
                {
                    //use redis cache.
                    RedisConnectionPool.Initialize(new RedisPoolSetting() { Host = RedisHost, DbIndex = RedisDb }, new JsonCacheSerializer(Encoding.UTF8));
                    RedisConnected = true;
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Redis Initialize error:{0}", ex);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="userToken"></param>
        public static void SaveToken(string key, UserToken userToken)
        {
            string userKey = string.Format("{0}:{1}", AccountServerToken, userToken.UserId);
            string redisKey = string.Format("{0}@{1}", AccountServerToken, key);
            if (!string.IsNullOrEmpty(RedisHost) && RedisConnected)
            {
                try
                {
                    string script = @"
local redisKey = KEYS[1]
local userKey = KEYS[2]
local key = KEYS[3]
local timeout = ARGV[1]
local val = ARGV[2]
local oldToken = redis.call('Get', userKey)
if oldToken and oldToken ~= nil then
    local k =  '__AccountToken@'..oldToken
    redis.call('del', k)
end
redis.call('Set', redisKey, val, 'EX', timeout)
redis.call('Set', userKey, key, 'EX', timeout)
return 0
";
                    //use redis cache.
                    RedisConnectionPool.Process(client =>
                    {
                        int timeout = (int)(userToken.ExpireTime - DateTime.Now).TotalSeconds;
                        client.ExecLuaAsInt(script, new[] { redisKey, userKey, key }, new[] { timeout.ToString(), MathUtils.ToJson(userToken) });
                    });
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Save token errror:{0}", ex);
                }
                return;
            }
            if(HttpContext2.Current != null && HttpContext2.Current.Items != null)
            {
                var cache = HttpContext2.Current.Items;
                var oldToken = cache[userKey] as string;
                if (!string.IsNullOrEmpty(oldToken))
                {
                    cache.Remove(string.Format("{0}@{1}", AccountServerToken, oldToken));
                }
                cache[redisKey] = userToken;
                cache[userKey] = key;
            }
            else
            {
                string oldToken;
                if (userHashCache.TryGetValue(userToken.UserId, out oldToken))
                {
                    UserToken temp;
                    userTokenCache.TryRemove(oldToken, out temp);
                }
                userTokenCache[key] = userToken;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="redisHost"></param>
        /// <param name="redisDb"></param>
        /// <returns></returns>
        public static UserToken GetUserToken(string token, string redisHost, int redisDb)
        {
            UserToken userToken = null;
            string redisKey = string.Format("{0}@{1}", AccountServerToken, token);
            RedisConnectionPool.Process(client =>
            {
                var val = client.Get(redisKey);
                userToken = val == null || val.Length == 0 ? new UserToken() : MathUtils.ParseJson<UserToken>(Encoding.UTF8.GetString(val));
            }, new RedisPoolSetting() { Host = redisHost, DbIndex = redisDb });
            return userToken;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static UserToken GetUserToken(string token)
        {
            UserToken userToken = null;
            string redisKey = string.Format("{0}@{1}", AccountServerToken, token);
            if (!string.IsNullOrEmpty(RedisHost) && RedisConnected)
            {
                //use redis cache.
                RedisConnectionPool.Process(client =>
                {
                    var val = client.Get(redisKey);
                    userToken = val == null || val.Length == 0 ? new UserToken() : MathUtils.ParseJson<UserToken>(Encoding.UTF8.GetString(val));
                });
                return userToken;
            }
            if (HttpContext2.Current != null && HttpContext2.Current.Items != null)
            {
                userToken = (UserToken)HttpContext2.Current.Items;//.Cache[redisKey];
            }
            if (Equals(userToken, null) && userTokenCache.ContainsKey(redisKey))
            {
                userToken = (UserToken)userTokenCache[redisKey];
            }
            return userToken;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handlerData"></param>
        /// <returns></returns>
        public static ResponseData Excute(HandlerData handlerData)
        {
            Type type;
            if (!handlerTypes.TryGetValue(handlerData.Name, out type))
            {
                throw new HandlerException(StateCode.NoHandler, "Not Found Handler");
            }
            dynamic instance = type.CreateInstance();
            Type paramType = type.GetInterfaces().First().GetGenericArguments().First();
            dynamic paramObj = ParseObject(handlerData, paramType);
            var result = instance.Excute(paramObj);
            return result as ResponseData;
        }

        private static object ParseObject(HandlerData handlerData, Type paramType)
        {
            var paramValues = handlerData.Params;
            var obj = paramType.CreateInstance();
            foreach (PropertyInfo prop in paramType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (paramValues.ContainsKey(prop.Name) && (prop.PropertyType.IsValueType || typeof(string) == prop.PropertyType))
                {
                    object value;
                    var val = paramValues[prop.Name];
                    if (prop.PropertyType == typeof(bool))
                    {
                        value = val.ToBool();
                    }
                    else
                    {
                        value = Convert.ChangeType(val, prop.PropertyType);
                    }
                    prop.SetValue(obj, value, null);
                }
            }
            return obj;
        }
    }
}