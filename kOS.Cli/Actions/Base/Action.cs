using kOS.Cli.Models;

namespace kOS.Cli.Actions
{
    /// <summary>
    /// Common actio interface.
    /// </summary>
    internal interface IAction
    {
        int Run();
    }
}
