using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TNet.Data;
using TNet.Extend;

namespace TNet.Pay
{
    /// <summary>
    /// 
    /// </summary>
    public class PayOperator
    {
        internal PayOperator()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public List<FeedbackInfo> GetFeedBackList(Action<CommandFilter> match)
        {
            var command = ConfigManger.Provider.CreateCommandStruct("GMFeedBack", CommandMode.Inquiry);
            command.Columns = "GMID,UId,GameID,ServerID,GMType,content,SubmittedTime,RContent,ReplyTime,ReplyID,Pid,NickName";
            command.OrderBy = "SUBMITTEDTIME DESC";
            command.Filter = ConfigManger.Provider.CreateCommandFilter();
            if (match != null)
            {
                match(command.Filter);
            }
            command.Parser();
            return GetFeedBackList(command.Sql, command.Parameters);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uID"></param>
        /// <returns></returns>
        public List<FeedbackInfo> GetFeedBackList(int uID)
        {
            return GetFeedBackList(f =>
            {
                f.Condition = f.FormatExpression("UId");
                f.AddParam("UId", uID);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private List<FeedbackInfo> GetFeedBackList(string sql, params IDataParameter[] parameters)
        {
            using (var reader = ConfigManger.Provider.ExecuteReader(CommandType.Text, sql, parameters))
            {
                List<FeedbackInfo> olist = new List<FeedbackInfo>();
                while (reader.Read())
                {
                    FeedbackInfo gMFeedBackInfo = new FeedbackInfo();
                    gMFeedBackInfo.Uid = MathUtils.ToInt(reader["UId"]);
                    gMFeedBackInfo.GameID = MathUtils.ToInt(reader["GameID"]);
                    gMFeedBackInfo.ServerID = MathUtils.ToInt(reader["ServerID"]);
                    gMFeedBackInfo.Pid = MathUtils.ToNotNullString(reader["Pid"]);
                    gMFeedBackInfo.NickName = MathUtils.ToNotNullString(reader["NickName"]);
                    gMFeedBackInfo.Content = MathUtils.ToNotNullString(reader["content"]);
                    gMFeedBackInfo.ID = MathUtils.ToInt(reader["GMID"]);
                    gMFeedBackInfo.Type = MathUtils.ToInt(reader["GMType"]);
                    gMFeedBackInfo.ReplyContent = MathUtils.ToNotNullString(reader["RContent"]);
                    gMFeedBackInfo.ReplyID = MathUtils.ToInt(reader["ReplyID"]);

                    gMFeedBackInfo.ReplyDate = MathUtils.ToDateTime(reader["ReplyTime"], DateTime.MinValue);
                    gMFeedBackInfo.CreateDate = MathUtils.ToDateTime(reader["SubmittedTime"], DateTime.MinValue);


                    olist.Add(gMFeedBackInfo);
                }
                return olist;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        public bool PostFeedBack(FeedbackInfo info)
        {
            CommandStruct command = ConfigManger.Provider.CreateCommandStruct("GMFeedBack", CommandMode.Insert);
            command.AddParameter("UId", info.Uid);
            command.AddParameter("GameID", info.GameID);
            command.AddParameter("ServerID", info.ServerID);
            command.AddParameter("GMType", info.Type);
            command.AddParameter("content", info.Content);
            command.AddParameter("Pid", info.Pid);
            command.AddParameter("NickName", info.NickName);
            command.AddParameter("SubmittedTime", info.CreateDate);
            command.Parser();

            return ConfigManger.Provider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="feedbackId"></param>
        /// <param name="replyContent"></param>
        /// <param name="replyId">回复者</param>
        /// <returns></returns>
        public bool ReplyToFeedBack(int feedbackId, string replyContent, int replyId)
        {
            CommandStruct command = ConfigManger.Provider.CreateCommandStruct("GMFeedBack", CommandMode.Modify);
            command.AddParameter("RContent", replyContent);
            command.AddParameter("ReplyID", replyId);
            command.AddParameter("ReplyTime", DateTime.Now);
            command.Filter = ConfigManger.Provider.CreateCommandFilter();
            command.Filter.Condition = ConfigManger.Provider.FormatFilterParam("GMId");
            command.Filter.AddParam("GMId", feedbackId);
            command.Parser();

            return ConfigManger.Provider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
        }
    }
}
