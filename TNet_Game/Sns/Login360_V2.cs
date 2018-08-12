using System;
using System.Collections.Generic;
using System.Text;
using TNet.Configuration;
using TNet.Context;
using TNet.Log;
using TNet.Serialization;

namespace TNet.Sns
{
    /// <summary>
    /// Login360_ v2.
    /// </summary>
    public class Login360_V2 : AbstractLogin
    {
        private string _retailID = string.Empty;
        private string _pid = string.Empty;
        private string _retailUser = string.Empty;
        private string AppId = string.Empty;
        private string AppKey = string.Empty;
        private string _code = string.Empty;
        private string Url = string.Empty;
        private string _aceessTokenUrl = string.Empty;
        private string _appSecret = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="TNet.Sns.Login360_V2"/> class.
        /// </summary>
        /// <param name="retailID">Retail I.</param>
        /// <param name="retailUser">Retail user.</param>
        /// <param name="pid">Pid.</param>
        /// <param name="code">Code.</param>
        public Login360_V2(string retailID, string retailUser, string pid, string code)
        {
            this._retailID = retailID;
            this._pid = pid;
            this._retailUser = retailUser;
            this._code = code;
            GameChannel gameChannel = TNGameBaseConfigManager.GameSetting.GetChannelSetting(ChannelType.channel360);
            if (gameChannel != null)
            {
                Url = gameChannel.Url;
                _aceessTokenUrl = gameChannel.TokenUrl;
                GameSdkSetting setting = gameChannel.GetSetting(retailID);
                if (setting != null)
                {
                    AppId = setting.AppId;
                    AppKey = setting.AppKey;
                    _appSecret = setting.AppSecret;
                }
                else
                {
                    TraceLog.ReleaseWrite("The sdkChannel section channel360:{0} is null.", retailID);
                }
            }
            else
            {
                TraceLog.ReleaseWrite("The sdkChannel 360 section is null.");
            }
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

            string getAccessTokenUrl = string.Format("{0}?grant_type=authorization_code&code={1}&client_id={2}&client_secret={3}&redirect_uri=oob",
                _aceessTokenUrl,
                _code,
                AppKey,
                _appSecret);
            string resultGetToken = HttpRequestManager.GetStringData(getAccessTokenUrl, "GET");
            if (!string.IsNullOrEmpty(resultGetToken))
            {
                var sdkGetToken = JsonUtils.Deserialize<SDK360GetTokenError>(resultGetToken);
                if (!string.IsNullOrEmpty(sdkGetToken.error_code))
                {
                    TraceLog.ReleaseWrite("360sdk login get token fail:{0},errorCode:{1},request url:{2}", sdkGetToken.error,
                                          sdkGetToken.error_code, getAccessTokenUrl);
                    return false;
                }
                AccessToken = sdkGetToken.access_token;
                RefeshToken = sdkGetToken.refresh_token;
                Scope = sdkGetToken.scope;
                ExpiresIn = Convert.ToInt32(sdkGetToken.expires_in);
            }
            else
            {
                return false;
            }
            string urlData = string.Format("{0}?access_token={1}",
                Url,
                AccessToken
            );

            string result = HttpRequestManager.GetStringData(urlData, "GET");
            try
            {
                if (!string.IsNullOrEmpty(result))
                {
                    var sdk = JsonUtils.Deserialize<SDK360Error>(result);
                    if (!string.IsNullOrEmpty(sdk.error_code))
                    {
                        TraceLog.ReleaseWrite("360sdk login get user info fail:{0},errorCode:{1},request url:{2}", sdk.error,
                                              sdk.error_code, urlData);
                        return false;
                    }
                    string[] arr = SnsManager.LoginByRetail(_retailID, sdk.id);
                    this.UserID = arr[0];
                    this.PassportID = arr[1];
                    QihooUserID = sdk.id;
                    SessionID = GetSessionId();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
                return false;
            }
        }

        /// <summary>
        /// SDK360Error.
        /// </summary>
        public class SDK360Error
        {
            /// <summary>
            /// Gets or sets the error_code.
            /// </summary>
            /// <value>The error_code.</value>
            public string error_code { get; set; }
            /// <summary>
            /// Gets or sets the error.
            /// </summary>
            /// <value>The error.</value>
            public string error { get; set; }
            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>The identifier.</value>
            public string id { get; set; }
            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>The name.</value>
            public string name { get; set; }
            /// <summary>
            /// Gets or sets the avatar.
            /// </summary>
            /// <value>The avatar.</value>
            public string avatar { get; set; }
            /// <summary>
            /// Gets or sets the sex.
            /// </summary>
            /// <value>The sex.</value>
            public string sex { get; set; }
            /// <summary>
            /// Gets or sets the area.
            /// </summary>
            /// <value>The area.</value>
            public string area { get; set; }
            /// <summary>
            /// Gets or sets the nick.
            /// </summary>
            /// <value>The nick.</value>
            public string nick { get; set; }
        }
        /// <summary>
        /// SDK360GetTokenError.
        /// </summary>
        public class SDK360GetTokenError
        {
            /// <summary>
            /// Gets or sets the error_code.
            /// </summary>
            /// <value>The error_code.</value>
            public string error_code { get; set; }
            /// <summary>
            /// Gets or sets the error.
            /// </summary>
            /// <value>The error.</value>
            public string error { get; set; }
            /// <summary>
            /// Gets or sets the access_token.
            /// </summary>
            /// <value>The access_token.</value>
            public string access_token { get; set; }
            /// <summary>
            /// Gets or sets the expires_in.
            /// </summary>
            /// <value>The expires_in.</value>
            public string expires_in { get; set; }
            /// <summary>
            /// Gets or sets the refresh_token.
            /// </summary>
            /// <value>The refresh_token.</value>
            public string refresh_token { get; set; }
            /// <summary>
            /// Gets or sets the scope.
            /// </summary>
            /// <value>The scope.</value>
            public string scope { get; set; }

        }
    }
}
