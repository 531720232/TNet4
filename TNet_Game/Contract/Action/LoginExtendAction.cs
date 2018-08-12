using System;
using System.Collections.Generic;
using System.Text;
using TNet.Extend;
using TNet.Service;
using TNet.Sns;

namespace TNet.Contract.Action
{
    /// <summary>
    /// 提供扩展渠道登录
    /// </summary>
    public abstract class LoginExtendAction : LoginAction
    {
        /// <summary>
        /// The refesh token.
        /// </summary>
        protected string RefeshToken;
        /// <summary>
        /// The scope.
        /// </summary>
        protected string Scope;
        /// <summary>
        /// The qihoo user I.
        /// </summary>
        protected string QihooUserID;
        /// <summary>
        /// The access token360.
        /// </summary>
        protected string AccessToken360;
        /// <summary>
        /// Initializes a new instance of the <see cref="TNet.Contract.Action.LoginExtendAction"/> class.
        /// </summary>
        /// <param name="actionId">Action identifier.</param>
        /// <param name="httpGet">Http get.</param>
        protected LoginExtendAction(short actionId, ActionGetter httpGet)
            : base(actionId, httpGet)
        {
        }
        /// <summary>
        /// 创建返回协议内容输出栈
        /// </summary>
        public override void BuildPacket()
        {
            PushIntoStack(Current.SessionId);
            PushIntoStack(Current.UserId.ToNotNullString());
            PushIntoStack(UserType);
            PushIntoStack(MathUtils.Now.ToString("yyyy-MM-dd HH:mm"));
            PushIntoStack(GuideId);
            PushIntoStack(PassportId);
            PushIntoStack(AccessToken360);
            PushIntoStack(RefeshToken);
            PushIntoStack(QihooUserID);
            PushIntoStack(Scope);
        }
        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <param name="login">Login.</param>
        protected override void SetParameter(ILogin login)
        {
            AbstractLogin baseLogin = login as AbstractLogin;
            if (baseLogin != null)
            {
                //AccessToken = baseLogin.AccessToken;
                RefeshToken = baseLogin.RefeshToken;
                QihooUserID = baseLogin.QihooUserID;
                Scope = baseLogin.Scope;
                AccessToken360 = baseLogin.AccessToken;
            }
        }


    }
}
