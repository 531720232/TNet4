using System;
using System.Collections.Generic;
using System.Text;
using TNet.Lang;
using TNet.Service;

namespace TNet.Contract.Action
{
    class JsonLuaAction : LuaAction
    {
        public JsonLuaAction(int actionId, ActionGetter actionGetter, object scriptScope, bool ignoreAuthorize)
            : base(actionId, actionGetter, scriptScope, ignoreAuthorize)
        {
            EnableWebSocket = true;
        }

        protected override string BuildJsonPack()
        {
            return _scriptScope["buildPacket"].Call(_scriptScope, _urlParam, _actionResult)[0];
        }
    }
    class LuaAction : ScriptAction
    {
        public LuaAction(int actionId, ActionGetter actionGetter, object scriptScope, bool ignoreAuthorize)
            : base(ScriptType.Lua, actionId, actionGetter, scriptScope, ignoreAuthorize)
        {
            //ScriptEngines.LuaRegister(actionGetter);
        }

        public override bool GetUrlElement()
        {
            var func = _scriptScope["getUrlElement"];
            _urlParam = func.Call(_scriptScope, actionGetter)[0];
            return _urlParam != null && _urlParam["Result"] ? true : false;
        }

        public override bool DoAction()
        {
            _actionResult = _scriptScope["takeAction"].Call(_scriptScope, _urlParam)[0];
            return _actionResult != null && _actionResult["Result"] ? true : false;
        }

        public override void BuildPacket()
        {
            bool result = _scriptScope["buildPacket"].Call(_scriptScope, dataStruct, _urlParam, _actionResult)[0];
            if (!result)
            {
                ErrorCode = Language.Instance.ErrorCode;
                if (!IsRealse)
                {
                    ErrorInfo = Language.Instance.ServerBusy;
                }
            }
        }
    }
}
