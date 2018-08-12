using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using TNet.Extend;

namespace TNet.Com.Generic
{
    /// <summary>
    /// 新手卡
    /// </summary>
    [Serializable, ProtoContract]
    public abstract class NoviceCard
    {
        /// <summary>
        /// The curr user identifier.
        /// </summary>
        protected int CurrUserId;
        /// <summary>
        /// The length of the card.
        /// </summary>
        protected int CardLength = 10;
        /// <summary>
        /// Initializes a new instance of the <see cref="TNet.Com.Generic.NoviceCard"/> class.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        protected NoviceCard(int userId)
        {
            CurrUserId = userId;
        }

        /// <summary>
        /// 检查是否生成新手卡
        /// </summary>
        public void CheckGenerateCard()
        {
            if (HasGenerateID)
            {
                DoGenerate(GenerateID());
            }
        }
        /// <summary>
        /// Dos the generate.
        /// </summary>
        /// <param name="generateID">Generate I.</param>
        protected abstract void DoGenerate(string generateID);
        /// <summary>
        /// Gets a value indicating whether this instance has generate I.
        /// </summary>
        /// <value><c>true</c> if this instance has generate I; otherwise, <c>false</c>.</value>
        protected abstract bool HasGenerateID { get; }

        /// <summary>
        /// 生成卡号
        /// </summary>
        /// <returns></returns>
        protected virtual string GenerateID()
        {
            return "C" + CurrUserId.ToString().PadLeft(CardLength, '0');
        }

        /// <summary>
        /// 激活
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public bool Activate(string cardId)
        {
            int cardUserId;
            if (!TryGetCardUserId(ref cardId, out cardUserId)) return false;
            return DoActivateCard(cardId, cardUserId);
        }

        private bool TryGetCardUserId(ref string cardId, out int cardUserId)
        {
            cardUserId = 0;
            cardId = cardId ?? "";
            cardId = cardId.Trim().Replace('c', 'C');
            if (string.IsNullOrEmpty(cardId) ||
                !cardId.StartsWith("C") ||
                !int.TryParse(cardId.Substring(1), out cardUserId))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Dos the activate card.
        /// </summary>
        /// <returns><c>true</c>, if activate card was done, <c>false</c> otherwise.</returns>
        /// <param name="cardId">Card identifier.</param>
        /// <param name="cardUserId">Card user identifier.</param>
        protected abstract bool DoActivateCard(string cardId, int cardUserId);

        /// <summary>
        /// 领取奖励
        /// </summary>
        public bool CheckLvPrize(short userLv, string cardId)
        {
            int cardUserId;
            if (!TryGetCardUserId(ref cardId, out cardUserId)) return false;

            var prizeLvs = PackagePrizeLv;
            foreach (var item in prizeLvs)
            {
                short lv = item.ToShort();
                if (HasPrize(userLv, lv))
                {
                    DoPrize(cardId, cardUserId, lv);
                }
            }
            return true;
        }
        /// <summary>
        /// Dos the prize.
        /// </summary>
        /// <param name="cardId">Card identifier.</param>
        /// <param name="cardUserId">Card user identifier.</param>
        /// <param name="lv">Lv.</param>
        protected abstract void DoPrize(string cardId, int cardUserId, short lv);
        /// <summary>
        /// Determines whether this instance has prize the specified userLv prizeLv.
        /// </summary>
        /// <returns><c>true</c> if this instance has prize the specified userLv prizeLv; otherwise, <c>false</c>.</returns>
        /// <param name="userLv">User lv.</param>
        /// <param name="prizeLv">Prize lv.</param>
        protected abstract bool HasPrize(short userLv, short prizeLv);
        /// <summary>
        /// Gets the package prize lv.
        /// </summary>
        /// <value>The package prize lv.</value>
        protected abstract string[] PackagePrizeLv { get; }
    }
}
