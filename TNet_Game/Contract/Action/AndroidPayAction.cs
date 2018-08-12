using System;
using System.Collections.Generic;
using System.Text;
using TNet.Extend;
using TNet.Log;
using TNet.Pay;
using TNet.Service;

namespace TNet.Contract.Action
{
    /// <summary>
    /// 触控安卓充值
    /// </summary>
    public abstract class AndroidPayAction : AuthorizeAction
    {
        private string orderno = string.Empty;
        private int _gameID = 0;
        private int _serviceID = 0;
        private string _deviceId = string.Empty;
        private string amount = string.Empty;
        private string gamecoins = string.Empty;
        private string _passportId = "";
        private string _RetailID;
        /// <summary>
        /// Initializes a new instance of the <see cref="TNet.Contract.Action.AndroidPayAction"/> class.
        /// </summary>
        /// <param name="actionID">Action I.</param>
        /// <param name="httpGet">Http get.</param>
        public AndroidPayAction(short actionID, ActionGetter httpGet)
            : base(actionID, httpGet)
        {
        }
        /// <summary>
        /// 创建返回协议内容输出栈
        /// </summary>
        public override void BuildPacket()
        {
        }
        /// <summary>
        /// 接收用户请求的参数，并根据相应类进行检测
        /// </summary>
        /// <returns></returns>
        public override bool GetUrlElement()
        {
            TraceLog.ReleaseWriteFatal("PayInfo---error");
            if (actionGetter.GetInt("gameID", ref _gameID)
                && actionGetter.GetInt("ServerID", ref _serviceID)
                && actionGetter.GetString("amount", ref amount)
                && actionGetter.GetString("gameconis", ref gamecoins)
                && actionGetter.GetString("orderno", ref orderno)
                && actionGetter.GetString("PassportID", ref _passportId))
            {
                actionGetter.GetString("RetailID", ref _RetailID);
                actionGetter.GetString("deviceId", ref _deviceId);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 子类实现Action处理
        /// </summary>
        /// <returns></returns>
        public override bool TakeAction()
        {
            decimal Amount = amount.ToDecimal();
            int silver = (Amount * (decimal)6.5).ToInt();
            PayManager.AddOrderInfo(orderno, Amount, _passportId, _serviceID, _gameID, silver, _deviceId, _RetailID);
            return true;
        }
    }
}
