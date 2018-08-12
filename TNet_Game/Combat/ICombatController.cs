
namespace TNet.Combat
{
    /// <summary>
    /// 战斗控制器接口
    /// </summary>
    public interface ICombatController
    {
		/// <summary>
		/// Gets the single combat.
		/// </summary>
		/// <returns>The single combat.</returns>
		/// <param name="args">Arguments.</param>
        ISingleCombat GetSingleCombat(object args);

		/// <summary>
		/// Gets the many one combat.
		/// </summary>
		/// <returns>The many one combat.</returns>
		/// <param name="args">Arguments.</param>
        IManyOneCombat GetManyOneCombat(object args);

		/// <summary>
		/// Gets the multi combat.
		/// </summary>
		/// <returns>The multi combat.</returns>
		/// <param name="args">Arguments.</param>
        IMultiCombat GetMultiCombat(object args);
    }
}