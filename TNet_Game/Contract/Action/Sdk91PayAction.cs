using System;
using System.Collections.Generic;
using System.Text;
using TNet.Log;
using TNet.Pay;
using TNet.Service;

namespace TNet.Contract.Action
{
    /// <summary>
    /// Sdk91 pay action.
    /// </summary>
    public class Sdk91PayAction : BaseStruct
    {
        private string OrderID = string.Empty;
        private int gameID = 0;
        private int serviceID = 0;
        private string passportId = string.Empty;
        private string servicename = string.Empty;
        private string _RetailID = "0000";
        /// <summary>
        /// Initializes a new instance of the <see cref="TNet.Contract.Action.Sdk91PayAction"/> class.
        /// </summary>
        /// <param name="aActionId">A action identifier.</param>
        /// <param name="httpGet">Http get.</param>
        public Sdk91PayAction(short aActionId, ActionGetter httpGet)
            : base(aActionId, httpGet)
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
            TraceLog.ReleaseWriteFatal("url");
            if (actionGetter.GetString("OrderID", ref OrderID)
                && actionGetter.GetInt("gameID", ref gameID)
                && actionGetter.GetInt("Server", ref serviceID)
                && actionGetter.GetString("ServiceName", ref servicename)
                && actionGetter.GetString("PassportID", ref passportId))
            {
                actionGetter.GetString("RetailID", ref _RetailID);
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
            SaveLog(string.Format("91SKD充值>>Order:{0},Pid:{1},servicename:{2}", OrderID, passportId, servicename));
            //PaymentService.Get91Payment(gameID, serviceID, passportId, servicename, OrderID);

            PayManager.get91PayInfo(gameID, serviceID, passportId, servicename, OrderID, _RetailID);
            return true;
        }
    }
}
