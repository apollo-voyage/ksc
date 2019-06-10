using System;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using kOS.Cli.Models;
using kOS.Cli.Options;
using Pastel;

namespace kOS.Cli.Logging
{
    /// <summary>
    /// Drawing mode.
    /// </summary>
    public enum Draw
    {
        None,
        Prefix,
        Color,
        PrefixAndColor
    }

    /// <summary>
    /// Simple logger with log levels: Info, Done, Warn, Error
    /// </summary>
    public abstract class Logger
    {
        /// <summary>
        /// Color for info messages.
        /// </summary>
        private static readonly Color InfoColor = Color.DeepSkyBlue;

        /// <summary>
        /// Color for done messages.
        /// </summary>
        private static readonly Color DoneColor = Color.LimeGreen;

        /// <summary>
        /// Color for error messages.
        /// </summary>
        private static readonly Color ErrorColor = Color.IndianRed;

        /// <summary>
        /// Color for warning messages.
        /// </summary>
        private static readonly Color WarnColor = Color.Orange;

        /// <summary>
        /// Stopwatch to measure time of execution.
        /// </summary>
        private Stopwatch _watch;

        /// <summary>
        /// Elapsed time of the internal stopwatch.
        /// </summary>
        protected long Elapsed { get { return _watch.ElapsedMilliseconds; } }

        /// <summary>
        /// Prefix for info messages.
        /// </summary>
        private string InfoPrefix = " INFO ".Pastel(Color.Black).PastelBg(InfoColor);

        /// <summary>
        /// Prefix for done messages.
        /// </summary>
        private string DonePrefix = " DONE ".Pastel(Color.Black).PastelBg(DoneColor);

        /// <summary>
        /// Prefix for error messages.
        /// </summary>
        private string ErrorPrefix = " ERROR ".Pastel(Color.Black).PastelBg(ErrorColor);

        /// <summary>
        /// Prefix for warning messages.
        /// </summary>
        private string WarningPrefix = " WARN ".Pastel(Color.Black).PastelBg(WarnColor);

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Logger()
        {
            _watch = new Stopwatch();
        }

        /// <summary>
        /// Resets and starts the stopwatch.
        /// </summary>
        protected void ResetAndStartWatch()
        {
            _watch.Reset();
            _watch.Start();
        }

        /// <summary>
        /// Stops stopwatch.
        /// </summary>
        protected void StopWatch()
        {
            _watch.Stop();
        }

        /// <summary>
        /// Writes a info message to the console.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="args">Variable arguments for formatting.</param>
        protected void Info(string message, params object[] args)
        {
            WriteLine(InfoPrefix, Color.White, message, args);
        }

        /// <summary>
        /// Writes a info message to the console.
        /// </summary>
        /// <param name="draw">Draw type.</param>
        /// <param name="message">Message to write.</param>
        /// <param name="args">Variable arguments for formatting.</param>
        protected void Info(Draw draw, string message, params object[] args)
        {
            Color color = draw == Draw.Color || draw == Draw.PrefixAndColor ? InfoColor : Color.White;
            string prefix = draw == Draw.Prefix || draw == Draw.PrefixAndColor ? InfoPrefix : "";

            WriteLine(prefix, color, message, args);
        }

        /// <summary>
        /// Writes a done message to the console.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="args">Variable arguments for formatting.</param>
        protected void Done(string message, params object[] args)
        {
            WriteLine(DonePrefix, Color.White, message, args);
        }

        /// <summary>
        /// Writes a done message to the console.
        /// </summary>
        /// <param name="draw">Draw type.</param>
        /// <param name="message">Message to write.</param>
        /// <param name="args">Variable arguments for formatting.</param>
        protected void Done (Draw draw, string message, params object[] args)
        {
            Color color = draw == Draw.Color || draw == Draw.PrefixAndColor ? DoneColor : Color.White;
            string prefix = draw == Draw.Prefix || draw == Draw.PrefixAndColor ? DonePrefix : "";

            WriteLine(prefix, color, message, args);
        }

        /// <summary>
        /// Writes a warning message to the console.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="args">Variable arguments for formatting.</param>
        protected void Warn(string message, params object[] args)
        {
            WriteLine(WarningPrefix, Color.White, message, args);
        }

        /// <summary>
        /// Writes a warning message to the console.
        /// </summary>
        /// <param name="draw">Draw type.</param>
        /// <param name="message">Message to write.</param>
        /// <param name="args">Variable arguments for formatting.</param>
        protected void Warn(Draw draw, string message, params object[] args)
        {
            Color color = draw == Draw.Color || draw == Draw.PrefixAndColor ? WarnColor : Color.White;
            string prefix = draw == Draw.Prefix || draw == Draw.PrefixAndColor ? WarningPrefix : "";

            WriteLine(prefix, color, message, args);
        }

        /// <summary>
        /// Writes a error message to the console.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="args">Variable arguments for formatting.</param>
        protected void Error(string message, params object[] args)
        {
            WriteLine(ErrorPrefix, Color.White, message, args);
        }

        /// <summary>
        /// Writes a error message to the console.
        /// </summary>
        /// <param name="draw">Draw type.</param>
        /// <param name="message">Message to write.</param>
        /// <param name="args">Variable arguments for formatting.</param>
        protected void Error(Draw draw, string message, params object[] args)
        {
            Color color = draw == Draw.Color || draw == Draw.PrefixAndColor ? ErrorColor : Color.White;
            string prefix = draw == Draw.Prefix || draw == Draw.PrefixAndColor ? ErrorPrefix : "";

            WriteLine(prefix, color, message, args);
        }

        /// <summary>
        /// Writes a new empty line to the console.
        /// </summary>
        protected void NewLine()
        {
            Console.WriteLine("");
        }

        /// <summary>
        /// Writes a messager to the console.
        /// </summary>
        /// <param name="prefix">Message prefix.</param>
        /// <param name="color">Message color.</param>
        /// <param name="message">Message to write.</param>
        /// <param name="args">Variable arguments for formatting.</param>
        private void WriteLine(string prefix, Color color, string message, params object[] args)
        { 
            message = message.Pastel(color);
            message = prefix == string.Empty ? message : prefix + " " + message;
            Console.WriteLine(message, args);
        }
    }
}
