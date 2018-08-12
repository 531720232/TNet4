﻿
using TNet.Model;

namespace TNet.Combat
{
    /// <summary>
    /// 多对一战斗
    /// </summary>
    public interface IManyOneCombat
    {
        /// <summary>
        /// 加入攻击方阵列
        /// </summary>
		/// <param name="combatGrid"></param>
        void AppendAttack(EmbattleQueue combatGrid);

        /// <summary>
        /// 加入防守方阵列
        /// </summary>
		/// <param name="combatGrid"></param>
        void SetDefend(EmbattleQueue combatGrid);

        /// <summary>
        /// 交战
        /// </summary>
        /// <returns>返回胜利或失败</returns>
        bool Doing();

        /// <summary>
        /// 交战过程
        /// </summary>
        /// <returns></returns>
        object GetProcessResult();

        /// <summary>
        /// 回合数
        /// </summary>
        int BoutNum { get; }
    }
}