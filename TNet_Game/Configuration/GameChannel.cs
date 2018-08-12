using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Configuration
{
    internal enum ChannelType
    {
        channel91 = 0,
        channelUC,
        channelDanle,
        channel10086,
        channel360,
        channelMIUI,
        channelAnySDK,
        channelTencent,
        channelFeiliu,
        channelMeiZu,
        channelND
    }
    /// <summary>
    /// 游戏渠道
    /// </summary>
    internal class GameChannel
    {
        private List<GameSdkSetting> _sdkSettingList = new List<GameSdkSetting>();

        public GameChannel(ChannelType channelType)
        {
            ChannelType = channelType;
            Init();
        }

        private void Init()
        {
            switch (ChannelType)
            {
                case ChannelType.channel91:
                    Url = "http://service.sj.91.com/usercenter/AP.aspx";
                    break;
                case ChannelType.channelUC:
                    Url = "http://sdk.g.uc.cn/ss";
                    ChannelId = "2";
                    Service = "ucid.user.sidInfo";
                    break;
                case ChannelType.channelDanle:
                    Url = "http://connect.d.cn/open/member/info/";
                    Version = "1.3";
                    break;
                case ChannelType.channel10086:
                    Url = "http://ospd.mmarket.com:8089/trust";
                    Version = "1.0.0";
                    CType = "1";
                    break;
                case ChannelType.channel360:
                    Url = "https://openapi.360.cn/user/me.json";
                    TokenUrl = "https://openapi.360.cn/oauth2/access_token";
                    break;
                case ChannelType.channelMIUI:
                    Url = "http://mis.migc.xiaomi.com/api/biz/service/verifySession.do";
                    break;
                case ChannelType.channelAnySDK:
                    Url = "http://oauth.anysdk.com/api/User/LoginOauth/";
                    break;
                case ChannelType.channelTencent:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 娓犻亾
        /// </summary>
        public ChannelType ChannelType { get; private set; }
        /// <summary>
        /// all
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Danle,10086
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 10086
        /// </summary>
        public string CType { get; set; }
        /// <summary>
        /// uc
        /// </summary>
        public string ChannelId { get; set; }
        /// <summary>
        /// uc
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// 360
        /// </summary>
        public string TokenUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting"></param>
        public void Add(GameSdkSetting setting)
        {
            _sdkSettingList.Add(setting);
        }
        /// <summary>
        /// 鑾峰緱閰嶇疆
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameSdkSetting GetSetting(string name = "")
        {
            if (string.IsNullOrEmpty(name))
            {
                return _sdkSettingList[0];
            }
            return _sdkSettingList.Find(m => m.RetailId == name);
        }
    }
}
