﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Serialization
{
    /// <summary>
    /// ICacheSerializer
    /// </summary>
    public interface ICacheSerializer
    {
        /// <summary>
        /// Serialize object to byte[]
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        byte[] Serialize(object obj);

        /// <summary>
        /// Deserialize
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        object Deserialize(Byte[] data, Type type);
    }
}
