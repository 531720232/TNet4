using System;
using System.Collections.Generic;
using System.Text;
using TNet.Pay.Section;
using TNet.Service;

namespace TNet.Contract.Action
{
    /// <summary>
    /// App store detail action.
    /// </summary>
    public class AppStoreDetailAction : BaseStruct
    {
        private List<AppStorePayElement> appList = new List<AppStorePayElement>();
        private MobileType MobileType = 0;
        /// <summary>
        /// Initializes a new instance of the <see cref="TNet.Contract.Action.AppStoreDetailAction"/> class.
        /// </summary>
        /// <param name="aActionId">A action identifier.</param>
        /// <param name="httpGet">Http get.</param>
        public AppStoreDetailAction(short aActionId, ActionGetter httpGet)
            : base(aActionId, httpGet)
        {
        }
        /// <summary>
        /// 创建返回协议内容输出栈
        /// </summary>
        public override void BuildPacket()
        {
            PushIntoStack(appList.Count);
            foreach (var item in appList)
            {
                DataStruct ds = new DataStruct();
                ds.PushIntoStack(item.Dollar.ToString());
                ds.PushIntoStack(item.ProductId);
                ds.PushIntoStack(item.SilverPiece);
                PushIntoStack(ds);
            }
        }
        /// <summary>
        /// 接收用户请求的参数，并根据相应类进行检测
        /// </summary>
        /// <returns></returns>
        public override bool GetUrlElement()
        {
            if (actionGetter.GetEnum("MobileType", ref MobileType))
            {
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
            var paySection = AppStoreFactory.GetPaySection();
            var rates = paySection.Rates;

            foreach (AppStorePayElement rate in rates)
            {
                if (rate.MobileType == MobileType.Normal || rate.MobileType == MobileType)
                {
                    appList.Add(rate);
                }
            }
            return true;
        }
    }
}
