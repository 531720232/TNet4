using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TNet.Com.Model;

namespace TNet.Com.Mall
{
    /// <summary>
    /// 单例交易
    /// </summary>
    [Serializable, ProtoContract]
    public class SingleTrade : ITrade
    {
        private GoodsData _goods;
        private readonly int _timeOut;
        /// <summary>
        /// Initializes a new instance of the <see cref="TNet.Com.Mall.SingleTrade"/> class.
        /// </summary>
        /// <param name="timeOut">Time out.</param>
        public SingleTrade(int timeOut = 1000)
        {
            _timeOut = timeOut;
        }
        /// <summary>
        /// Tries the enter traded.
        /// </summary>
        /// <returns>true</returns>
        /// <c>false</c>
        /// <param name="goods">Goods.</param>
        public bool TryEnterTraded(GoodsData goods)
        {
            if (goods == null)
            {
                throw new ArgumentNullException("goods");
            }
            _goods = goods;
            return Monitor.TryEnter(_goods, _timeOut);
        }
        /// <summary>
        /// Exits the traded.
        /// </summary>
        public void ExitTraded()
        {
            Monitor.Exit(_goods);
        }

    }
}
