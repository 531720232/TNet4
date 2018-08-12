using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Pay
{
    /// <summary>
    /// 游戏服信息
    /// </summary>
    public class ServerInfo
    {
        /// <summary>
        /// Gets or sets the I.
        /// </summary>
        /// <value>The I.</value>
        public int ID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the game I.
        /// </summary>
        /// <value>The game I.</value>
        public int GameID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the name of the game.
        /// </summary>
        /// <value>The name of the game.</value>
        public string GameName
        {
            get;
            set;
        }
    }
}
