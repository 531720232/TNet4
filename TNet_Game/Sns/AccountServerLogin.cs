﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TNet.Extend;
using TNet.Log;
using TNet.Security;
using TNet.Serialization;
using TNet.Sns.Service;

namespace TNet.Sns
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountServerLogin : ILogin
    {
        private int _timeout;
        private readonly string _url;
        private readonly string _imei;
        /// <summary>
        /// 
        /// </summary>
        public AccountServerLogin()
        {
            ContentType = "application/json";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <param name="imei"></param>
        /// <param name="timeout"></param>
        public AccountServerLogin(string url, string token, string imei, int timeout = 3000)
            : this()
        {
            _url = url;
            _imei = imei;
            _timeout = timeout;
            Token = token;
        }

        #region property
        /// <summary>
        /// 
        /// </summary>
        public string PassportID { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserID { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public int UserType { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SessionID { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public StateCode State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string GetRegPassport()
        {
            var query = new Dictionary<string, string>();
            query["Handler"] = "Passport";
            query["IMEI"] = _imei;
            string queryString = HttpUtils.BuildPostParams(query);
            using (var response = Send(queryString, _timeout))
            {
                var responseStream = response.GetResponseStream();
                if (responseStream == null)
                {
                    TraceLog.Write("Response stream is null.\r\nUrl:{0}/?{1}", _url, queryString);
                    return null;
                }
                using (var sr = new StreamReader(responseStream, Encoding.UTF8))
                {
                    string json = sr.ReadToEnd();
                    var body = JsonUtils.Deserialize<ResponseBody<PassportInfo>>(json);
                    if (body != null && body.StateCode == StateCode.OK)
                    {
                        var token = body.Data as PassportInfo;
                        if (token != null)
                        {
                            PassportID = token.PassportId;
                            Password = token.Password;
                            return PassportID;
                        }
                    }
                    TraceLog.WriteError("AccountServer get passport error:{0}", json);
                }
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckLogin()
        {
            var query = new Dictionary<string, string>();
            query["Handler"] = "Validate";
            query["Token"] = Token;
            string queryString = HttpUtils.BuildPostParams(query);
            queryString = AppendSign(queryString);
            string json;
            using (var response = Send(queryString, _timeout))
            {
                var responseStream = response.GetResponseStream();
                if (responseStream == null)
                {
                    TraceLog.Write("Response stream is null.\r\nUrl:{0}/?{1}", _url, queryString);
                    return false;
                }
                using (var sr = new StreamReader(responseStream, Encoding.UTF8))
                {
                    json = sr.ReadToEnd();
                    responseStream.Close();
                }
            }
            var body = JsonUtils.Deserialize<ResponseBody<LoginToken>>(json);
            if (body == null)
            {
                TraceLog.Write("Response stream convert to json error.Json:{0}\r\nUrl:{1}/?{2}", json, _url, queryString);
                State = StateCode.ParseError;
                return false;
            }

            State = body.StateCode;
            if (body.StateCode == StateCode.OK)
            {
                var token = body.Data as LoginToken;
                if (token != null)
                {
                    PassportID = token.PassportId;
                    UserID = token.UserId.ToString();
                    UserType = token.UserType;
                    return true;
                }
            }
            if (body.StateCode == StateCode.TokenExpired || body.StateCode == StateCode.NoToken)
            {
                TraceLog.Write("AccountServer login fail, StateCode:{0}-{1}", body.StateCode, body.StateDescription);
                throw new HandlerException(body.StateCode, body.StateDescription);
            }
            TraceLog.WriteError("AccountServer login error:{0}", json);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual WebResponse Send(string query, int timeout = 3000, bool isPost = false)
        {
            return isPost
                ? HttpUtils.Post(_url, query, timeout, null, Encoding.UTF8, ContentType, null)
                : HttpUtils.Get(string.Format("{0}/?{1}", _url, query), ContentType, timeout, null, null);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected string AppendSign(string query)
        {
            string signKey = Runtime.GameZone.Setting != null ? Runtime.GameZone.Setting.ProductSignKey : "";
            if (string.IsNullOrEmpty(signKey))
            {
                return query;
            }
            string sign = CryptoHelper.MD5_Encrypt(query + signKey, Encoding.UTF8);
            return string.Format("{0}&Sign={1}", query, sign);
        }
    }

    /// <summary>
    /// 直接连接Redis服务验证
    /// </summary>
    public class AccountServerRedisLogin : AccountServerLogin
    {
        private readonly string _host;
        private readonly int _db;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="db"></param>
        /// <param name="token"></param>
        public AccountServerRedisLogin(string host, int db, string token)
        {
            _host = host;
            _db = db;
            Token = token;
        }


        public override bool CheckLogin()
        {
            UserToken userToken = null;
            if ((userToken = HandlerManager.GetUserToken(Token, _host, _db)) == null)
            {
                return false;
            }
            if (userToken.ExpireTime < DateTime.Now)
            {
                return false;
            }
            PassportID = userToken.PassportId;
            UserID = userToken.UserId.ToString();
            UserType = userToken.UserType;
            return true;
        }
    }
}
