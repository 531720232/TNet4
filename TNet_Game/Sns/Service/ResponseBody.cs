
namespace TNet.Sns.Service
{

    /// <summary>
    /// 
    /// </summary>
    public class ResponseBody<T> where T : ResponseData, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public ResponseBody()
        {
            StateCode = StateCode.OK;
            StateDescription = "";
            Vesion = "1.0";
            Handler = "";
            Data = new T();
        }
        /// <summary>
        /// 
        /// </summary>
        public StateCode StateCode { get; set; }
        /// <summary>
        /// /
        /// </summary>
        public string StateDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Vesion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Handler { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ResponseBody : ResponseBody<ResponseData>
    {

    }
}