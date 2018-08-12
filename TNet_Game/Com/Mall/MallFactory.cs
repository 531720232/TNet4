using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Com.Mall
{
    /// <summary>
    /// 商城工厂
    /// </summary>
    [Serializable, ProtoContract]
    public static class MallFactory
    {
        private static Dictionary<int, Merchant> _merchantList = new Dictionary<int, Merchant>();
        /// <summary>
        /// Registers the merchant.
        /// </summary>
        /// <param name="merchantId">Merchant identifier.</param>
        /// <param name="controller">Controller.</param>
        public static void RegisterMerchant(int merchantId, MallController controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }
            Merchant merchant = new Merchant(merchantId, controller);
            merchant.InitializeGoods();
            if (!_merchantList.ContainsKey(merchantId))
            {
                _merchantList.Add(merchantId, merchant);
            }
        }
        /// <summary>
        /// Gets the merchant.
        /// </summary>
        /// <returns>The merchant.</returns>
        /// <param name="merchantId">Merchant identifier.</param>
        public static Merchant GetMerchant(int merchantId)
        {
            return _merchantList.ContainsKey(merchantId) ? _merchantList[merchantId] : null;
        }
        /// <summary>
        /// Releases all resource used by the object.
        /// </summary>
        public static void Dispose()
        {
            _merchantList.Clear();
            _merchantList = null;
        }
    }
}
