
using System;
using TNet.Configuration;

namespace TNet.Combat
{
    /// <summary>
    /// 控制器工厂
    /// </summary>
    public static class CombatControllerFactory
    {
        private static object lockThis = new object();
        private static ICombatController controller = null;
		/// <summary>
		/// Create this instance.
		/// </summary>
        public static ICombatController Create()
        {
            if (controller == null)
            {
                lock (lockThis)
                {
                    if (controller == null)
                    {
                        controller = (ICombatController)Activator.CreateInstance(Type.GetType(TNGameBaseConfigManager.GetCombat().TypeName));
                    }
                }
            }
            return controller;
        }
    }
}