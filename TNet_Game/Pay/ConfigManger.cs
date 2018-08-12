using System;
using System.Collections.Generic;
using System.Text;
using TNet.Data;

namespace TNet.Pay
{
    internal class ConfigManger
    {
        private static readonly DbBaseProvider _dbBaseProvider;
        internal const string ConnectKey = "PayCenter";

        static ConfigManger()
        {
            _dbBaseProvider = DbConnectionProvider.CreateDbProvider(ConnectKey);

        }

        public static DbBaseProvider Provider
        {
            get { return _dbBaseProvider; }
        }
    }
}
