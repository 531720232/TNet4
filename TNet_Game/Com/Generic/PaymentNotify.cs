using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using TNet.Context_;
using TNet.Log;
using TNet.Pay;

namespace TNet.Com.Generic
{
    /// <summary>
    /// 付款通知
    /// </summary>
    [Serializable, ProtoContract]
    public abstract class PaymentNotify
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void Notify(IUser user)
        {
            int gameId = Runtime.GameZone.ProductCode;
            int serverId = Runtime.GameZone.ProductServerId;
            int userId = user.GetUserId();
            string pid = user.GetPassportId();
            OrderInfo[] orderList = Pay.PayManager.getPayment(gameId, serverId, pid);
            foreach (var orderInfo in orderList)
            {
                if (DoNotify(userId, orderInfo))
                {
                    PayManager.Abnormal(orderInfo.OrderNO);
                    TraceLog.ReleaseWriteFatal("Payment order:{0},Pid:{1} notify success", orderInfo.OrderNO, pid);
                }
                else
                {
                    TraceLog.ReleaseWriteFatal("Payment order:{0},Pid:{1} notify faild", orderInfo.OrderNO, pid);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        protected abstract bool DoNotify(int userId, OrderInfo orderInfo);
    }
}
