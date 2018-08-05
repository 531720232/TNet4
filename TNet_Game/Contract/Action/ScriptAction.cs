using System;
using System.Collections.Generic;
using System.Text;
using TNet.Lang;
using TNet.Service;

namespace TNet.Contract.Action
{
    /// <summary>
    /// 
    /// </summary>
    public enum ScriptType
    {
        /// <summary>
        /// 
        /// </summary>
        Csharp,
        /// <summary>
        /// 
        /// </summary>
        Python,
        /// <summary>
        /// 
        /// </summary>
        Lua
    }
    /// <summary>
    /// 
    /// </summary>
    public class JsonScriptAction : ScriptAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptType"></param>
        /// <param name="aActionId"></param>
        /// <param name="actionGetter"></param>
        /// <param name="scriptScope"></param>
        /// <param name="ignoreAuthorize"></param>
        public JsonScriptAction(ScriptType scriptType, int aActionId, ActionGetter actionGetter, object scriptScope, bool ignoreAuthorize)
            : base(scriptType, aActionId, actionGetter, scriptScope, ignoreAuthorize)
        {
            EnableWebSocket = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string BuildJsonPack()
        {
            return _scriptScope.buildPacket(_urlParam, _actionResult);
        }
    }

    /// <summary>
    /// 提供脚本支持
    /// </summary>
    public class ScriptAction : AuthorizeAction
    {
        /// <summary>
        /// The _script scope.
        /// </summary>
        protected readonly dynamic _scriptScope;
        private readonly bool _ignoreAuthorize;
        /// <summary>
        /// The _url parameter.
        /// </summary>
        protected dynamic _urlParam;
        /// <summary>
        /// The _action result.
        /// </summary>
        protected dynamic _actionResult;
        private ScriptType _scriptType;

        /// <summary>
        /// /
        /// </summary>
        /// <param name="scriptType"></param>
        /// <param name="aActionId"></param>
        /// <param name="actionGetter"></param>
        /// <param name="scriptScope"></param>
        /// <param name="ignoreAuthorize">忽略授权</param>
        public ScriptAction(ScriptType scriptType, int aActionId, ActionGetter actionGetter, object scriptScope, bool ignoreAuthorize)
            : base(aActionId, actionGetter)
        {
            _scriptType = scriptType;
            _scriptScope = scriptScope;
            _ignoreAuthorize = ignoreAuthorize;
        }
        /// <summary>
        /// 子类实现
        /// </summary>
        protected override void InitChildAction()
        {
        }
        /// <summary>
        /// 接收用户请求的参数，并根据相应类进行检测
        /// </summary>
        /// <returns></returns>
        public override bool GetUrlElement()
        {
            _urlParam = _scriptScope.getUrlElement(actionGetter, this);
            return _urlParam != null && _urlParam.Result ? true : false;
        }
        /// <summary>
        /// 子类实现Action处理
        /// </summary>
        /// <returns></returns>
        public override bool TakeAction()
        {
            _actionResult = _scriptScope.takeAction(_urlParam, this);
            return _actionResult != null && _actionResult.Result ? true : false;
        }
        /// <summary>
        /// 创建返回协议内容输出栈
        /// </summary>
        public override void BuildPacket()
        {
            bool result = _scriptScope.buildPacket(dataStruct, _urlParam, _actionResult);
            if (!result)
            {
                ErrorCode = Language.Instance.ErrorCode;
                if (IsRealse)
                {
                    ErrorInfo = Language.Instance.ServerBusy;
                }
            }
        }
        /// <summary>
        /// 不检查的ActionID
        /// </summary>
        /// <value><c>true</c> if ignore action identifier; otherwise, <c>false</c>.</value>
        protected override bool IgnoreActionId
        {
            get { return _ignoreAuthorize; }
        }
    }
}
