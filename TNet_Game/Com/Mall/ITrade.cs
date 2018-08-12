using System;
using System.Collections.Generic;
using System.Text;
using TNet.Com.Model;

namespace TNet.Com.Mall
{
    /// <summary>
    /// 交易出错代码
    /// </summary>
    public enum TradeErrorCode
    {
        /// <summary>
        /// The sucess.
        /// </summary>
        Sucess = 0,
        /// <summary>
        /// The fail.
        /// </summary>
        Fail = 1000,
        /// <summary>
        /// The no godds.
        /// </summary>
        NoGodds,
        /// <summary>
        /// The trade timeout.
        /// </summary>
        TradeTimeout,
        /// <summary>
        /// The amount not enough.
        /// </summary>
        AmountNotEnough,
        /// <summary>
        /// 库存
        /// </summary>
        QuantityNotEnough,
        /// <summary>
        /// The limt number.
        /// </summary>
        LimtNumber
    }

    /// <summary>
    /// 交易策略
    /// </summary>
    public interface ITrade
    {
        /// <summary>
        /// Tries the enter traded.
        /// </summary>
        /// <returns><c>true</c>, if enter traded was tryed, <c>false</c> otherwise.</returns>
        /// <param name="goods">Goods.</param>
        bool TryEnterTraded(GoodsData goods);
        /// <summary>
        /// Exits the traded.
        /// </summary>
        void ExitTraded();
    }
}
