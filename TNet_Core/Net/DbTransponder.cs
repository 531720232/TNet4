using System;
using System.Collections.Generic;
using System.Text;
using TNet.Model;

namespace TNet.Net
{
    /// <summary>
    /// 数据转发器
    /// </summary>
    public class DbTransponder : ITransponder
    {
        /// <summary>
        /// 
        /// </summary>
        public DbTransponder()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="receiveParam"></param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public bool TryReceiveData<T>(TransReceiveParam receiveParam, out List<T> dataList) where T : AbstractEntity, new()
        {
            dataList = new List<T>();
            return true;
            //using (IDataReceiver getter = new SqlDataReceiver(receiveParam.Schema, receiveParam.DbFilter))
            //{
            //    return getter.TryReceive<T>(out dataList);
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="sendParam"></param>
        public bool SendData<T>(T[] dataList, TransSendParam sendParam) where T : AbstractEntity, new()
        {
            return false;
            //using (var sender = new SqlDataSender(sendParam.IsChange, sendParam.Schema.ConnectKey))
            //{
            //    return sender.Send(dataList);
            //}
        }
    }
}
