using ProtoBuf;
using System;
using System.Collections.Generic;
using TNet.Com.Model;
using TNet.Context_;
using TNet.Extend;

namespace TNet.Com.Mall
{
    /// <summary>
    /// 商家
    /// </summary>
    [Serializable, ProtoContract]
    public class Merchant
    {
        private readonly int _merchantId;
        private readonly MallController _controller;
        private List<GoodsData> _goodsList = new List<GoodsData>();
        /// <summary>
        /// Initializes a new instance of the <see cref="TNet.Com.Mall.Merchant"/> class.
        /// </summary>
        /// <param name="merchantId">Merchant identifier.</param>
        /// <param name="controller">Controller.</param>
        public Merchant(int merchantId, MallController controller)
        {
            if (controller == null)
            {
                throw new ArgumentException("controller");
            }
            _merchantId = merchantId;
            _controller = controller;
        }

        /// <summary>
        /// 初始化商品
        /// </summary>
        public void InitializeGoods()
        {
            var list = _controller.LoadGoodsData(_merchantId);
            foreach (var goods in list)
            {
                _goodsList.InsertSort(goods, _controller.SortGoods);
            }
        }

        /// <summary>
        /// 展示商品
        /// </summary>
        /// <returns></returns>
        public IList<GoodsData> ShowGoods(Predicate<GoodsData> match, int pageIndex, int pageSize, out int pageCount)
        {
            if (match == null)
            {
                return _goodsList.GetPaging(pageIndex, pageSize, out pageCount);
            }
            return _goodsList.FindAll(match).GetPaging(pageIndex, pageSize, out pageCount);
        }

        /// <summary>
        /// 购买商品
        /// </summary>
        /// <param name="user"></param>
        /// <param name="goodsId"></param>
        /// <param name="goodsNum"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public bool BuyGoods(BaseUser user, int goodsId, int goodsNum, out TradeErrorCode errorCode)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            var goods = _goodsList.Find(m => m.Id == goodsId);
            if (goods != null)
            {
                return _controller.TradeGoods(user, goods, goodsNum, out errorCode);
            }
            errorCode = TradeErrorCode.NoGodds;
            return false;
        }
    }
}