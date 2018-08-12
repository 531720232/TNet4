using System;
using System.Collections.Generic;
using System.Text;
using TNet.Context_;
using TNet.Lang;
using TNet.Service;

namespace TNet.Contract.Action
{
    /// <summary>
    /// Register action.
    /// </summary>
    public abstract class RegisterAction : BaseStruct
    {
        /// <summary>
        /// The name of the user.
        /// </summary>
        protected string UserName;
        /// <summary>
        /// The sex.
        /// </summary>
        protected byte Sex;
        /// <summary>
        /// The head I.
        /// </summary>
        protected string HeadID;
        /// <summary>
        /// The retail I.
        /// </summary>
        protected string RetailID;
        /// <summary>
        /// The pid.
        /// </summary>
        protected string Pid;
        /// <summary>
        /// The type of the mobile.
        /// </summary>
        protected MobileType MobileType;
        /// <summary>
        /// The screen x.
        /// </summary>
        protected short ScreenX;
        /// <summary>
        /// The screen y.
        /// </summary>
        protected short ScreenY;
        /// <summary>
        /// The req app version.
        /// </summary>
        protected short ReqAppVersion;
        /// <summary>
        /// The game I.
        /// </summary>
        protected int GameID;
        /// <summary>
        /// The server I.
        /// </summary>
        protected int ServerID;
        /// <summary>
        /// The device I.
        /// </summary>
        protected string DeviceID;
        /// <summary>
        /// Gets or sets the guide identifier.
        /// </summary>
        /// <value>The guide identifier.</value>
        public int GuideId { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="TNet.Contract.Action.RegisterAction"/> class.
        /// </summary>
        /// <param name="aActionId">A action identifier.</param>
        /// <param name="httpGet">Http get.</param>
        protected RegisterAction(short aActionId, ActionGetter httpGet)
            : base(aActionId, httpGet)
        {
        }
        /// <summary>
        /// 创建返回协议内容输出栈
        /// </summary>
        public override void BuildPacket()
        {
            PushIntoStack(GuideId);
        }
        /// <summary>
        /// 接收用户请求的参数，并根据相应类进行检测
        /// </summary>
        /// <returns></returns>
        public override bool GetUrlElement()
        {
            if (actionGetter.GetString("UserName", ref UserName) &&
                actionGetter.GetByte("Sex", ref Sex) &&
                actionGetter.GetString("HeadID", ref HeadID) &&
                actionGetter.GetString("RetailID", ref RetailID) &&
                actionGetter.GetString("Pid", ref Pid, 1, int.MaxValue) &&
                actionGetter.GetEnum("MobileType", ref MobileType)
                )
            {
                UserName = UserName.Trim();
                actionGetter.GetWord("ScreenX", ref ScreenX);
                actionGetter.GetWord("ScreenY", ref ScreenY);
                actionGetter.GetWord("ClientAppVersion", ref ReqAppVersion);
                actionGetter.GetString("DeviceID", ref DeviceID);
                actionGetter.GetInt("GameID", ref GameID);
                actionGetter.GetInt("ServerID", ref ServerID);
                return GetActionParam();
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CheckAction()
        {
            if (!Runtime.GameZone.IsRunning)
            {
                ErrorCode = Language.Instance.ErrorCode;
                ErrorInfo = Language.Instance.ServerLoading;
                return false;
            }
            if (Current.UserId <= 0)
            {
                ErrorCode = Language.Instance.ErrorCode;
                ErrorInfo = Language.Instance.UrlElement;
                return false;
            }

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool TakeAction()
        {
            IUser user;
            if (CreateUserRole(out user) && Current != null && user != null)
            {
                Current.Bind(user);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 处理结束执行
        /// </summary>
        /// <param name="state">If set to <c>true</c> state.</param>
        public override void TakeActionAffter(bool state)
        {
        }

        /// <summary>
        /// Gets the action parameter.
        /// </summary>
        /// <returns><c>true</c>, if action parameter was gotten, <c>false</c> otherwise.</returns>
        protected abstract bool GetActionParam();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract bool CreateUserRole(out IUser user);

    }
}
