using System;
using System.IO;
using System.Linq;
using System.Timers;
using System.Collections.Generic;
using kOS.Cli.IO;
using kOS.Cli.Models;
using kOS.Cli.Logging;
using kOS.Cli.Options;

namespace kOS.Cli.Actions
{
    /// <summary>
    /// Watch action. Runs when the user uses "ksc watch".
    /// </summary>
    class WatchAction : AbstractAction
    {
        /// <summary>
        /// Watch CLI options.
        /// </summary>
        private readonly WatchOptions _options;

        /// <summary>
        /// Compiler. Implements compile action, will be used to compile after file changes.
        /// </summary>
        private readonly CompileAction _compiler;

        /// <summary>
        /// Watcher logger.
        /// </summary>
        private readonly WatcherLogger _logger;

        /// <summary>
        /// All file system watchers.
        /// </summary>
        private List<FileSystemWatcher> _watchers;

        /// <summary>
        /// Timer to handle the change event double execution.
        /// </summary>
        private readonly Timer _timer;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">Watch CLI options.</param>
        public WatchAction(WatchOptions options)
        {
            _options = options;
            _compiler = new CompileAction(CompileOptions.FromWatchOptions(options), true);
            _logger = new WatcherLogger();

            _timer = new Timer
            {
                AutoReset = true,
                Interval = 200
            };
            _timer.Elapsed += OnRecompile;
        }

        /// <summary>
        /// Exectutes the watch action.
        /// </summary>
        /// <returns>CLI return code.</returns>
        public override int Run()
        {
            int result = 0;

            _logger.StartWatcherBooting();
            List<string> paths = GetWatchPaths();
            if (paths.Count > 0)
            {
                _watchers = CreateWatchers(paths);

                _logger.StopWatcherBooting();
                _logger.WatchingForPaths(paths);

                _logger.InitialCompilation();
                _compiler.Run();

                while (true) { };
            }
            else
            {
                _logger.NoWatchableFilesFound();
            }

            return result;
        }

        /// <summary>
        /// Loads the configuration from disk.
        /// </summary>
        /// <returns>Loaded configuration, if found on disk.</returns>
        protected override Configuration LoadConfiguration()
        {
            Configuration result = null;

            if (_options.Input == Constants.CurrentDirectory)
            {
                result = base.LoadConfiguration();
            }
            else
            {
                string fullPath = Path.GetFullPath(_options.Input);
                if (Directory.Exists(fullPath) == true)
                {
                    string configPath = Path.Combine(fullPath, Constants.ConfigFileName);
                    result = ConfigIO.ReadConfigFile(configPath);
                    if (result != null)
                    {
                        List<string> messages = result.IsValid();
                        if (messages.Count == 0)
                        {
                            foreach (Volume volume in result.Volumes)
                            {
                                volume.InputPath = volume.InputPath.Replace(".", _options.Input);
                                volume.OutputPath = volume.OutputPath.Replace(".", _options.Output);
                            }
                        }
                        else
                        {
                            _commonLogger.ConfigurationInvalid(messages);
                            result = null;
                        }
                    }
                    else
                    {
                        _commonLogger.NoConfigurationFound();
                    }
                }
            }
         
            return result;
        }

        /// <summary>
        /// Creates all needed watchers.
        /// </summary>
        /// <param name="paths">Paths to create watchers for.</param>
        /// <returns>List of created watchers.</returns>
        private List<FileSystemWatcher> CreateWatchers(List<string> paths)
        {
            List<FileSystemWatcher> result = new List<FileSystemWatcher>();
                 
            foreach(string path in paths)
            {
                FileSystemWatcher watcher = new FileSystemWatcher
                {
                    Path = path,
                    Filter = Constants.KerboscriptSearchPattern,
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    IncludeSubdirectories = true,
                    EnableRaisingEvents = true
                };

                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;
                watcher.Renamed += OnChanged;

                result.Add(watcher);
            }

            return result;
        }

        /// <summary>
        /// Will be called when a file change is observed.
        /// </summary>
        /// <param name="source">Source of the change.</param>
        /// <param name="e">Event parameters of the change.</param>
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            _timer.Start();
        }

        /// <summary>
        /// Will be called when a compilation should be triggered on a file change.
        /// </summary>
        /// <param name="source">Source of the change.</param>
        /// <param name="e">Event parameters of the change.</param>
        private void OnRecompile(Object source, ElapsedEventArgs e)
        {
            _timer.Stop();
            _compiler.Run();
        }

        /// <summary>
        /// Returns all paths that need to be watched.
        /// </summary>
        /// <returns>List of paths.</returns>
        private List<string> GetWatchPaths()
        {
            List<string> result = new List<string>();

            if ((_options.Input == Constants.CurrentDirectory &&
                _options.Output == Constants.CurrentDirectory) &&
                ConfigIO.IsDirectory(_options.Input) == true)
            {
                Configuration config = LoadConfiguration();
                if (config != null)
                {
                    if (_options.Volume == Constants.AllVolumes)
                    {
                        result = config.Volumes.Select((v) => v.InputPath).ToList();
                    }
                    else
                    {
                        Volume volume = null;
                        if (int.TryParse(_options.Volume, out int index) == true)
                        {
                            volume = config.Volumes.Find(v => v.Index == index);
                        }
                        else
                        {
                            volume = config.Volumes.Find(v => v.Name == _options.Volume);
                        }

                        if (volume != null)
                        {
                            result.Add(volume.InputPath);
                        }
                    }
                }
            }
            else
            {
                result.Add(_options.Input);
            }

            result = result.Select(p => Path.GetFullPath(p)).ToList();
            result = result.FindAll(p => File.Exists(p) || Directory.Exists(p));
            return result;
        }
    }
}
