using System;
using System.Collections.Generic;
using System.Text;
using TNet.Configuration;
using TNet.Extend;
using TNet.Log;
using TNet.Serialization;
using TNet.Sns._91sdk;

namespace TNet.Sns
{
    /// <summary>
    /// 91SDK 0001
    /// </summary>
    public class Login91sdk : AbstractLogin
    {
        private string _retailID = string.Empty;
        private string AppId;
        private string AppKey;
        private string Url;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Sns.Login91sdk"/> class.
        /// </summary>
        /// <param name="retailID">Retail I.</param>
        /// <param name="retailUser">Retail user.</param>
        /// <param name="sessionID">Session I.</param>
        public Login91sdk(string retailID, string retailUser, string sessionID)
        {
            this._retailID = retailID;
            SessionID = sessionID;
            Uin = retailUser;
            GameChannel gameChannel = TNGameBaseConfigManager.GameSetting.GetChannelSetting(ChannelType.channel91);
            if (gameChannel != null)
            {
                Url = gameChannel.Url;
                GameSdkSetting setting = gameChannel.GetSetting(retailID);
                if (setting != null)
                {
                    AppId = setting.AppId;
                    AppKey = setting.AppKey;
                }
                else
                {
                    TraceLog.ReleaseWrite("The sdkChannel section channel91:{0} is null.", retailID);
                }
            }
            else
            {
                TraceLog.ReleaseWrite("The sdkChannel 91 section is null.");
            }
        }


        private string Uin
        {
            get;
            set;
        }
        /// <summary>
        /// 注册通行证
        /// </summary>
        /// <returns></returns>
        public override string GetRegPassport()
        {
            return this.PassportID;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CheckLogin()
        {
            int Act = 4;
            string Sign = string.Format("{0}{1}{2}{3}{4}", AppId.ToNotNullString(), Act, Uin.ToNotNullString(), SessionID.ToNotNullString(), AppKey.ToNotNullString());
            Sign = HashToMD5Hex(Sign);

            string urlData = string.Format("{0}?AppId={1}&Act={2}&Uin={3}&SessionID={4}&Sign={5}",
                Url.ToNotNullString(),
                AppId.ToNotNullString(),
                Act,
                Uin.ToNotNullString(),
                SessionID.ToNotNullString(),
                Sign.ToNotNullString()
            );

            string result = HttpPostManager.GetStringData(urlData);
            try
            {
                if (string.IsNullOrEmpty(result))
                {
                    TraceLog.ReleaseWrite("91sdk login fail result:{0},request url:{1}", result, urlData);
                    return false;
                }
                SDKError sdk = JsonUtils.Deserialize<SDKError>(result);
                if (sdk.ErrorCode != "1")
                {
                    TraceLog.ReleaseWrite("91sdk login fail:{0},request url:{1}", sdk.ErrorDesc, urlData);
                    return false;
                }
                string[] arr = SnsManager.LoginByRetail(_retailID, Uin);
                this.UserID = arr[0];
                this.PassportID = arr[1];
                return true;
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
                return false;
            }

            //return !string.IsNullOrEmpty(UserID) && UserID != "0";
        }

    }
    /// <summary>
    /// SDK error.
    /// </summary>
    public class SDKError
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>The error code.</value>
        public string ErrorCode { get; set; }
        /// <summary>
        /// Gets or sets the error desc.
        /// </summary>
        /// <value>The error desc.</value>
        public string ErrorDesc { get; set; }
    }
}
