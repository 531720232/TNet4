using System.Text;
namespace TNet.RPC.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    public class DataMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public sbyte OpCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get { return Encoding.UTF8.GetString(Data); }
            set { Data = Encoding.UTF8.GetBytes(value); }
        }
    }
}