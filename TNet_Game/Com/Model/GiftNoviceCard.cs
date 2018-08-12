using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using TNet.Model;

namespace TNet.Com.Model
{

    /// <summary>
    /// 媒体礼包新手卡
    /// </summary>
    [Serializable, ProtoContract]
    public abstract class GiftNoviceCard : BaseEntity
    {
        /// <summary>
        /// Gets or sets the card no.
        /// </summary>
        /// <value>The card no.</value>
        public abstract string CardNo
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the type of the gift.
        /// </summary>
        /// <value>The type of the gift.</value>
        public abstract string GiftType
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public abstract int UserId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the activate date.
        /// </summary>
        /// <value>The activate date.</value>
        public abstract DateTime ActivateDate
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is invalid.
        /// </summary>
        /// <value><c>true</c> if this instance is invalid; otherwise, <c>false</c>.</value>
        public abstract bool IsInvalid
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the create ip.
        /// </summary>
        /// <value>The create ip.</value>
        public abstract string CreateIp
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public abstract DateTime CreateDate
        {
            get;
            set;
        }
    }
}
