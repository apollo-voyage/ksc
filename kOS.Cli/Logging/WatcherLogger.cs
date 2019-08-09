using System.Collections.Generic;

namespace kOS.Cli.Logging
{
    /// <summary>
    /// Logger implementation for the watch action.
    /// </summary>
    public class WatcherLogger : Logger
    {
        /// <summary>
        /// Print start watcher bootup process messages.
        /// </summary>
        public void StartWatcherBooting()
        {
            Info("Starting watcher...");
            ResetAndStartWatch();
        }

        /// <summary>
        /// Print stop watcher bootup process messages.
        /// </summary>
        public void StopWatcherBooting()
        {
            StopWatch();
            Done("Watcher started in {0} ms", Elapsed);
        }

        /// <summary>
        /// Prints all paths for which the watcher is no watching.
        /// </summary>
        /// <param name="paths">Paths that are watched.</param>
        public void WatchingForPaths(List<string> paths)
        {
            NewLine();
            Info("Now watching for changes in path(s):");
            foreach(string path in paths)
            {
                Info(Draw.None, "    " + path);
            }
        }

        /// <summary>
        /// Print initial compilation info messages.
        /// </summary>
        public void InitialCompilation()
        {
            NewLine();
            Info("Performing initial compilation...");
        }
    }
}
