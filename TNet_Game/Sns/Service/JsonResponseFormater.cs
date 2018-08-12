
using System.Text;
using TNet.Serialization;

namespace TNet.Sns.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonResponseFormater : IResponseFormater
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public byte[] Serialize(ResponseBody body)
        {
            string json = JsonUtils.SerializeCustom(body);
            var encoding = new UTF8Encoding();
            return encoding.GetBytes(json);
        }
    }
}
