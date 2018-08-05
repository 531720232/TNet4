using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNet.IO
{
    /// <summary>
    ///     Endianness of a converter
    /// </summary>
    public enum Endianness
    {
        /// <summary>
        ///     Little endian - least significant byte first
        /// </summary>
        LittleEndian,

        /// <summary>
        ///     Big endian - most significant byte first
        /// </summary>
        BigEndian
    }
}
