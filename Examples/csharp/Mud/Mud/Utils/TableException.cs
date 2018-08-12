using System;
using System.Collections.Generic;
using System.Text;

namespace Mud.Utils
{
 public   class TableException: Exception
    {
        #region Errors code

        public const int ErrorToConnect = 101;
        public const int SYNTAX_ERROR = 102;

        #endregion
        #region Ctor

        public int ErrorCode { get; private set; }
        public string Line { get; private set; }
        public int Position { get; private set; }

        public TableException(string message)
            : base(message)
        {
        }

        internal TableException(int code, string message, params object[] args)
            : base(string.Format(message, args))
        {
            this.ErrorCode = code;
        }

        internal TableException(int code, Exception inner, string message, params object[] args)
        : base(string.Format(message, args), inner)
        {
            this.ErrorCode = code;
        }

        #endregion

        internal static TableException DisConnect()
        {
            return new TableException(ErrorToConnect, "链接失败请重试");
        }

        internal static Exception SyntaxError(StringScanner s, string message = "Unexpected token")
        {
            return new TableException(SYNTAX_ERROR, message)
            {
                Line = s.Source,
                Position = s.Index
            };
        }
    }
}
