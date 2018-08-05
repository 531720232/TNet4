using System;
using System.Collections.Generic;
using System.Text;
using TNet.Context_;
using TNet.Extend;
using TNet.Lang;
using TNet.RPC.Sockets;
using TNet.Service;

namespace TNet.Contract.Action
{
    /// <summary>
    /// 授权访问的Action
    /// </summary>
    public abstract class AuthorizeAction : BaseStruct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Contract.Action.AuthorizeAction"/> class.
        /// </summary>
        /// <param name="actionId">Action I.</param>
        /// <param name="actionGetter">Http get.</param>
        protected AuthorizeAction(int actionId, ActionGetter actionGetter)
            : base(actionId, actionGetter)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool EnableWebSocket
        {
            set
            {
                if (value)
                {
                    IsWebSocket = true;
                    actionGetter.OpCode = OpCode.Text;
                }
            }
        }
        /// <summary>
        /// 开启支付通知
        /// </summary>
        public string EnablePayNotify
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool ValidateElement()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected bool CheckValidityPeriod(TimeSpan minInterval, TimeSpan maxInterval)
        {
            string st = actionGetter.GetSt();
            RefleshSt();
            if (IgnoreActionId)
            {
                return true;
            }
            if (!string.IsNullOrEmpty(st) && !string.Equals(st, "st", StringComparison.OrdinalIgnoreCase))
            {
                long time;
                if (long.TryParse(st, out time))
                {
                    return maxInterval == TimeSpan.Zero ||
                        (time > MathUtils.UnixEpochTimeSpan.Add(minInterval).TotalSeconds && time < MathUtils.UnixEpochTimeSpan.Add(maxInterval).TotalSeconds);
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CheckAction()
        {
            bool result = false;

            if (!Runtime.GameZone.IsRunning)
            {
                ErrorCode = Language.Instance.ErrorCode;
                ErrorInfo = Language.Instance.ServerLoading;
                return false;
            }
            if (IgnoreActionId || ActionFactory.IsIgnoreAction(actionId))
            {
                return true;
            }
            IUser user;
            LoginStatus status = CheckUser(out user);

            if (IsRunLoader)
            {
                status = LoginStatus.Success;
            }
            switch (status)
            {
                case LoginStatus.NoLogin:
                case LoginStatus.Timeout:
                    ErrorCode = Language.Instance.TimeoutCode;
                    ErrorInfo = Language.Instance.AcountNoLogin;
                    break;
                case LoginStatus.Logined:
                    ErrorCode = Language.Instance.DuplicateCode;
                    ErrorInfo = Language.Instance.AcountLogined;
                    break;
                case LoginStatus.Exit:
                    ErrorCode = Language.Instance.KickedOutCode;
                    ErrorInfo = Language.Instance.AcountIsLocked;
                    break;
                case LoginStatus.Success:
                    result = true;
                    break;
                default:
                    break;
            }
            if (CheckUserIsLocked(user))
            {
                ErrorCode = Language.Instance.KickedOutCode;
                ErrorInfo = Language.Instance.AcountIsLocked;
                result = false;
            }
            if (result && IsRefresh)
            {
                DoRefresh(actionId, user);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual bool CheckUserIsLocked(IUser user)
        {
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is refresh.
        /// </summary>
        /// <value><c>true</c> if this instance is refresh; otherwise, <c>false</c>.</value>
        protected virtual bool IsRefresh
        {
            get { return true; }
        }
        /// <summary>
        /// 不检查的ActionID
        /// </summary>
        protected virtual bool IgnoreActionId
        {
            get { return false; }
        }

        /// <summary>
        /// Checks the user.
        /// </summary>
        /// <returns>The user.</returns>
        /// <param name="user">Game user.</param>
        protected LoginStatus CheckUser(out IUser user)
        {
            user = null;
            if (Current != null)
            {
                if (Current.IsReplaced)
                {
                    return LoginStatus.Logined;
                }
                user = Current.User;
                return Current.IsAuthorized
                    ? LoginStatus.Success
                    : Current.IsTimeout
                        ? LoginStatus.Timeout
                        : LoginStatus.NoLogin;
            }
            return LoginStatus.NoLogin;
        }
        /// <summary>
        /// Dos the refresh.
        /// </summary>
        /// <param name="actionId">Action identifier.</param>
        /// <param name="gameUser">Game user.</param>
        protected void DoRefresh(int actionId, IUser gameUser)
        {
            if (EnablePayNotify != null)
            {
             //   EnablePayNotify.Notify(gameUser);
            }
            if (gameUser != null)
            {
                gameUser.RefleshOnlineDate();
            }
        }
    }

    /// <summary>
    /// Websocket use.
    /// </summary>
    public abstract class JsonAuthorizeAction : AuthorizeAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="actionGetter"></param>
        protected JsonAuthorizeAction(int actionId, ActionGetter actionGetter)
            : base(actionId, actionGetter)
        {
            EnableWebSocket = true;
        }
    }
}
