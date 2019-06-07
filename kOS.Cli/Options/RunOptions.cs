﻿using CommandLine;

namespace kOS.Cli.Options
{
    [Verb("run", HelpText = "Runs a specified Kerboscript or project script (use 'ksc init' to initialize a project).")]
    public class RunOptions
    {
        [Value(0, MetaName = "script", Required = true, HelpText = "[Kerbo]script to run.")]
        public string Script { get; set; }
    }
}