﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace TNet.Service
{
    /// <summary>
    /// 接口访问次数统计类
    /// </summary>
    public static class ActionCount
    {
        /// <summary>
        /// 当前日期
        /// </summary>
        private static DateTime curDate;
        /// <summary>
        /// 当日目前各接口访问情况统计
        /// </summary>
        private static ConcurrentDictionary<int, ActionLog> dicActionInfo;

        /// <summary>
        /// 接口访问次数加1
        /// </summary>
        /// <param name="actionId">接口编号</param>
        /// <param name="aStat">访问状态</param>
        public static void ActionVisit(int actionId, GameStruct.LogActionStat aStat)
        {
            if (dicActionInfo == null)
            {
                if (dicActionInfo == null)
                {
                    curDate = DateTime.Now.Date;
                    dicActionInfo = new ConcurrentDictionary<int, ActionLog>();
                }
            }

            if (!dicActionInfo.ContainsKey(actionId))
            {
                ActionLog tmpLog = new ActionLog(actionId, curDate);
                dicActionInfo.TryAdd(actionId, tmpLog);

            }

            dicActionInfo[actionId].Visitor(aStat);

            if (curDate != DateTime.Now.Date)
            {
                //已经进入第二天，全部写入DB，并初始化数据
                DateTime newDate = DateTime.Now.Date;
                foreach (KeyValuePair<int, ActionLog> item in dicActionInfo)
                {
                    item.Value.InsertDB(newDate);
                }
                curDate = newDate;
            }
        }
    }

    /// <summary>
    /// 接口访问情况记录类
    /// </summary>
    public class ActionLog
    {
        private DateTime lastDbTime;
        /// <summary>
        /// 成功次数
        /// </summary>
        public int SucCount { get; protected set; }
        /// <summary>
        /// 失败次数
        /// </summary>
        public int FailCount { get; protected set; }
        /// <summary>
        /// 当前累计的访问次数
        /// </summary>
        public int TotalCount { get { return SucCount + FailCount; } }
        private int actionId;
        private DateTime curDate;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="_actionid">接口编号</param>
        /// <param name="_curDate">当前日期</param>
        public ActionLog(int _actionid, DateTime _curDate)
        {
            actionId = _actionid;
            curDate = _curDate;
            SucCount = 0;
            FailCount = 0;
            lastDbTime = DateTime.Now;
        }

        /// <summary>
        /// 累加访问次数
        /// </summary>
        /// <param name="aStat">接口访问是成功还是失败</param>
        public void Visitor(GameStruct.LogActionStat aStat)
        {
            if (aStat == GameStruct.LogActionStat.Sucess)
            {
                this.SucCount++;
            }
            else
            {
                this.FailCount++;
            }
            if (TotalCount >= 100)
            {
                this.InsertDB(DateTime.Now.Date);
            }
            else
            {
                if (lastDbTime.AddMinutes(5).CompareTo(DateTime.Now) <= 0)
                {
                    this.InsertDB(DateTime.Now.Date);
                }
            }
        }

        /// <summary>
        /// 更新数据到DB，并数据清零
        /// </summary>
        /// <param name="_curNewDate"></param>
        public void InsertDB(DateTime _curNewDate)
        {
            this.lastDbTime = DateTime.Now;
            curDate = _curNewDate;
        }
    }
}
