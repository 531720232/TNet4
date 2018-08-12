namespace TNet.RPC.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    public class OpCode
    {
        /// <summary>
        /// no process
        /// </summary>
        public const sbyte Empty = -1;

        /// <summary>
        /// 
        /// </summary>
        public const sbyte Continuation = 0;
        /// <summary>
        /// 
        /// </summary>
        public const sbyte Text = 1;
        /// <summary>
        /// 
        /// </summary>
        public const sbyte Binary = 2;
        /// <summary>
        /// 
        /// </summary>
        public const sbyte Close = 8;
        /// <summary>
        /// 
        /// </summary>
        public const sbyte Ping = 9;
        /// <summary>
        /// 
        /// </summary>
        public const sbyte Pong = 10;
    }
}