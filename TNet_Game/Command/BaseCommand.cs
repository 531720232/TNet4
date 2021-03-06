﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TNet.Config;
using TNet.Configuration;
using TNet.Extend;
using TNet.Log;
using TNet.Script;

namespace TNet.Command
{
    /// <summary>
    /// GM命令基类
    /// </summary>
    public abstract class BaseCommand
    {
        private const string SectionName = "TNGameBase-GM";

        //private static bool EnableGM;

        //static BaseCommand()
        //{
        //    EnableGM = ConfigUtils.GetSetting("EnableGM", false);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static bool Check(string command)
        {
            return command.StartsWith("gm:", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 运行GM命令解析
        /// </summary>
        /// <param name="userId">发送GM命令者</param>
        /// <param name="command">GM命令</param>
        /// <param name="assemblyName">指定解析的程序集</param>
        public static void Run(string userId, string command, string assemblyName = "")
        {
            try
            {
                var section = ConfigManager.Configger.GetFirstOrAddConfig<MiddlewareSection>();
                if (!section.EnableGM)
                {
                    return;
                }

                command = command != null ? command.Trim() : string.Empty;
                string[] paramList = command.Split(new char[] { ' ' });
                if (paramList.Length < 1)
                {
                    return;
                }

                string cmd = paramList[0].Trim();
                if (cmd.Length > 3)
                {
                    cmd = cmd.Substring(3, cmd.Length - 3);
                }

                string[] args = new string[0];
                if (paramList.Length > 1)
                {
                    args = paramList[1].Split(new char[] { ',' });
                }
                BaseCommand baseCommand = null;
                if (string.IsNullOrEmpty(assemblyName))
                {
                    baseCommand = new ScriptCommand(cmd);
                }
                else
                {
                    baseCommand = CreateCommand(cmd, assemblyName);
                }
                if (baseCommand == null)
                {
                    return;
                }
                baseCommand.UserID = userId;
                baseCommand.ProcessCmd(args);
            }
            catch (Exception ex)
            {
                ErrorFormat(command, ex);
            }
        }

        private static BaseCommand CreateCommand(string cmd, string assemblyName)
        {
            string typeName = "";

            var arr = cmd.Split(':');
            string tempcmd = arr.Length > 1 ? arr[1] : arr[0];
            if (TNGameBaseConfigManager.GameSetting.HasSetting)
            {
                typeName = TNGameBaseConfigManager.GameSetting.GetGmCommandType(cmd);
                typeName = !string.IsNullOrEmpty(typeName) ? typeName : tempcmd + "Command";
            }
            else
            {
                CommandCollection cmdList = ((GmSection)ConfigurationManager.GetSection(SectionName)).Command;
                CommandElement cmdElement = cmdList[cmd];
                typeName = cmdElement != null ? cmdElement.TypeName : tempcmd + "Command";
            }

            string commandType = typeName;
            if (typeName.IndexOf(",") == -1)
            {
                commandType = string.Format("{0}.{1},{0}", assemblyName, typeName);
            }
            var type = Type.GetType(commandType, false, true);
            if (type != null)
            {
                return type.CreateInstance<BaseCommand>();
            }
            return ScriptEngines.ExecuteCSharp(string.Format("{0}.{1}", assemblyName, typeName));
        }


        private static void ErrorFormat(string command, Exception ex)
        {
            TraceLog.WriteError("GM命令:{0}执行失败\r\nException:{1}", command, ex);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TNet.Command.BaseCommand"/> class.
        /// </summary>
        protected BaseCommand()
        {
        }
        /// <summary>
        /// Gets or sets the user I.
        /// </summary>
        /// <value>The user I.</value>
        public string UserID
        {
            get;
            set;
        }
        /// <summary>
        /// Processes the cmd.
        /// </summary>
        /// <param name="args">Arguments.</param>
        protected abstract void ProcessCmd(string[] args);

    }
}
