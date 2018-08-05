using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Service
{

    /// <summary>
    /// 手机类型
    /// </summary>
    public enum MobileType
    {
        /// <summary>
        /// The normal.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// iPod
        /// </summary>
        iPod,
        /// <summary>
        /// iPad
        /// </summary>
        iPad,
        /// <summary>
        /// 破解版iPhone和iPad
        /// </summary>
        iPhone,
        /// <summary>
        /// 非破解版iPhone
        /// </summary>
        Phone_AppStore,
        /// <summary>
        /// Android
        /// </summary>
        Android,
        /// <summary>
        /// Mac
        /// </summary>
        Mac,
        /// <summary>
        /// WP7
        /// </summary>
        WindowsPhone7,
        /// <summary>
        /// 未知
        /// </summary>
        Unknow

    }
}
