using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using TNet.Cache;
using TNet.Cache.Generic;
using TNet.Model;

namespace TNet.Context_
{
    /// <summary>
    /// 游戏角色
    /// </summary>
    [Serializable, ProtoContract]
    public abstract class BaseUser : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected BaseUser()
        {
        }
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="access">Access.</param>
        protected BaseUser(AccessLevel access)
            : base(access)
        {
        }

        private ContextCacheSet<CacheItem> _userData;
        /// <summary>
        /// Gets the user data.
        /// </summary>
        /// <value>The user data.</value>
        public ContextCacheSet<CacheItem> UserData
        {
           
            get
            {
             
                if (_userData == null)
                {
                    _userData = new ContextCacheSet<CacheItem>(string.Format("__gamecontext_{0}", GetUserId()));
                }
                return _userData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public abstract bool IsLock { get; }

        /// <summary>
        /// 
        /// </summary>
        [Obsolete("no use")]
        public virtual DateTime OnlineDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract int GetUserId();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string GetNickName();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string GetPassportId();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string GetRetailId();
    }
}
