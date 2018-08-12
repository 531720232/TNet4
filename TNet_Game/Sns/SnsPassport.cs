using System;
using System.Collections.Generic;
using System.Data;
using TNet.Config;
using TNet.Configuration;
using TNet.Data;
using TNet.Extend;

namespace TNet.Sns
{
    /// <summary>
    /// 用户中心 - 通行证ID操作类
    /// </summary>
    public class SnsPassport : IDisposable
    {
        private MiddlewareSection section;
        /// <summary>
        /// 
        /// </summary>
        public SnsPassport()
        {
            section = ConfigManager.Configger.GetFirstOrAddConfig<MiddlewareSection>();
        }

        /// <summary>
        /// ID的状态
        /// </summary>
        private enum PassMark
        {
            /// <summary>
            /// 未分配下发
            /// </summary>
            UnPush = 0,
            /// <summary>
            /// 已分配下发到新注册用户的请求
            /// </summary>
            IsPushToNewUser,
            /// <summary>
            /// 已被注册
            /// </summary>
            IsReg
        }

        /// <summary>
        /// 从DB中加载未被注册的通行证ID
        /// </summary>
        /// <returns></returns>
        public string GetRegPassport()
        {
            var watch = TNet.Timing.RunTimeWatch.StartNew("GetRegPassport");
            bool isGet = false;
            //从未下发和过期1天已分配的账号从取
            var command = ConnectManager.Provider.CreateCommandStruct("SnsPassportLog", CommandMode.Inquiry, "PASSPORTID");
            command.Top = 100;
            command.OrderBy = "PASSPORTID ASC";
            command.Filter = ConnectManager.Provider.CreateCommandFilter();
            command.Filter.Condition = string.Format("{0} AND {1}",
                command.Filter.FormatExpression("MARK", "<"),
                command.Filter.FormatExpression("RegPushTime", "<"));
            command.Filter.AddParam("MARK", Convert.ToInt32(PassMark.IsReg));
            command.Filter.AddParam("RegPushTime", DateTime.Now.Date.AddDays(-1));
            command.Parser();
            var passsportList = new List<string>();
            string iPassportId = String.Empty;
            using (var aReader = ConnectManager.Provider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                while (aReader.Read())
                {
                    isGet = true;
                    passsportList.Add(aReader["PASSPORTID"].ToString());
                }
            }
            watch.Check("get pid");
            if (isGet)
            {
                iPassportId = FormatPassport(passsportList.Count > 1 ? passsportList[RandomUtils.GetRandom(0, passsportList.Count)] : passsportList[0]); //随机取

                if (!SetStat(iPassportId, PassMark.IsPushToNewUser))
                {
                    throw new Exception("Update passport state error");
                }
                watch.Check("update state");
                watch.Flush(true, 100);
                return iPassportId;
            }
            //新创建
            command = ConnectManager.Provider.CreateCommandStruct("SnsPassportLog", CommandMode.Insert);
            command.ReturnIdentity = true;
            command.AddParameter("MARK", Convert.ToInt32(PassMark.IsPushToNewUser));
            command.AddParameter("RegPushTime", MathUtils.Now);
            command.Parser();

            using (var aReader = ConnectManager.Provider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                if (aReader.Read())
                {
                    iPassportId = FormatPassport(aReader[0].ToString());
                    watch.Check("new pid");
                    watch.Flush(true, 100);
                    return iPassportId;
                }
                else
                {
                    throw new Exception("Generate passport error");
                }
            }
        }

        private string FormatPassport(string pid)
        {
            return section.PreAccount + pid;
        }

        /// <summary>
        /// 检验注册的通行证ID是否在SnsPassportLog列表中。
        /// </summary>
        /// <param name="aPid"></param>
        /// <returns>检测通过，则返回True，否则返回False</returns>
        public bool VerifyRegPassportId(string aPid)
        {
            try
            {
                string sPidPre = aPid.Substring(0, section.PreAccount.Length).ToUpper();
                if (sPidPre != section.PreAccount)
                {
                    return false;
                }

                string sTmp = aPid.Substring(section.PreAccount.Length);
                var command = ConnectManager.Provider.CreateCommandStruct("SnsPassportLog", CommandMode.Inquiry, "passportid");
                command.Top = 1;
                command.OrderBy = "PASSPORTID ASC";
                command.Filter = ConnectManager.Provider.CreateCommandFilter();
                command.Filter.Condition = command.Filter.FormatExpression("PassportId");
                command.Filter.AddParam("PassportId", sTmp);
                command.Parser();
                return ConnectManager.Provider.ExecuteScalar(CommandType.Text, command.Sql, command.Parameters) != null;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// </summary>
        /// <param name="aPid"></param>
        /// <returns></returns>
        public bool SetPassportReg(string aPid)
        {
            return this.SetStat(aPid, PassMark.IsReg);
        }


        private bool SetStat(string aPid, PassMark aMark)
        {
            try
            {
                string sTmp = aPid.Substring(section.PreAccount.Length);
                var command = ConnectManager.Provider.CreateCommandStruct("SnsPassportLog", CommandMode.Modify);
                command.AddParameter("mark", Convert.ToInt32(aMark));
                if (aMark == PassMark.IsPushToNewUser)
                {
                    command.AddParameter("regpushtime", MathUtils.Now);
                }
                else if (aMark == PassMark.IsReg)
                {
                    command.AddParameter("regtime", MathUtils.Now);
                }
                command.Filter = ConnectManager.Provider.CreateCommandFilter();
                command.Filter.Condition = command.Filter.FormatExpression("PassportId");
                command.Filter.AddParam("PassportId", sTmp);
                command.Parser();
                return ConnectManager.Provider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {

        }
    }
}