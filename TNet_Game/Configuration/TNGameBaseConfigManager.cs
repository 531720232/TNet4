using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using TNet.Timing;

namespace TNet.Configuration
{
   public static class TNGameBaseConfigManager
    {
        private static readonly object thisLock = new object();
        private const string KeyName = "TNGameBaseBll";
        private static TNGameBaseBllSection TNGameBaseBll;
        private static GameConfigSetting _gameConfigSetting = new GameConfigSetting();
        private static string _configFileName;

        /// <summary>
        /// 游戏配置
        /// </summary>
        internal static GameConfigSetting GameSetting
        {
            get
            {
                return _gameConfigSetting;
            }
        }

        private static TNGameBaseBllSection BaseConfig
        {
            get
            {
                if (TNGameBaseBll == null)
                {
                    lock (thisLock)
                    {
                        if (TNGameBaseBll == null)
                        {
                            TNGameBaseBll = (TNGameBaseBllSection)ConfigurationManager.GetSection(KeyName);
                        }
                    }
                }
                return TNGameBaseBll;
            }
        }

        /// <summary>
        /// 获取渠道登录处理提供类的配置
        /// </summary>
        /// <returns></returns>
        public static LoginElement GetLogin()
        {
            return BaseConfig != null ? BaseConfig.Login : null;
        }

        /// <summary>
        /// 获取战斗处理配置
        /// </summary>
        /// <returns></returns>
        public static CombatElement GetCombat()
        {
            return BaseConfig != null ? BaseConfig.Combat : null;
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        public static void Intialize()
        {
            string runtimePath = AppDomain.CurrentDomain.BaseDirectory;
            _configFileName = Path.Combine(runtimePath, "Game.config.xml");
            CacheListener routeListener = new CacheListener("__GAME_CONFIG", 0, (key, value, reason) =>
            {
                if (reason == CacheRemovedReason.Changed)
                {
                    _gameConfigSetting.Init(_configFileName);
                }
            }, _configFileName);
            routeListener.Start();

            _gameConfigSetting.Init(_configFileName);
        }

    }
}
