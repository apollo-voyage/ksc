using kOS.Cli.Models;

namespace kOS.Cli.Actions
{
    /// <summary>
    /// Common actio interface.
    /// </summary>
    internal interface IAction
    {
        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <returns>CLI return code.</returns>
        int Run();
    }
}
