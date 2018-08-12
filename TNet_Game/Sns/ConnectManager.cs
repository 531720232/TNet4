using System;
using System.Collections.Generic;
using System.Text;
using TNet.Data;

namespace TNet.Sns
{
    /// <summary>
    /// Config.
    /// </summary>
    internal class ConnectManager
    {
        private static readonly DbBaseProvider _dbBaseProvider;
        private const string ConnectKey = "SnsCenter";

        static ConnectManager()
        {
            _dbBaseProvider = DbConnectionProvider.CreateDbProvider(ConnectKey);

        }

        public static DbBaseProvider Provider
        {
            get { return _dbBaseProvider; }
        }
    }
}
