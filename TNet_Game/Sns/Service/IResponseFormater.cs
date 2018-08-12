
namespace TNet.Sns.Service
{
    /// <summary>
    /// 
    /// </summary>
    public interface IResponseFormater
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        byte[] Serialize(ResponseBody body);
    }
}