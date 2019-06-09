using System;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using kOS.Cli.Models;
using kOS.Cli.Options;
using Pastel;

namespace kOS.Cli.Logging
{
    public enum Draw
    {
        None,
        Prefix,
        Color,
        PrefixAndColor
    }

    public abstract class Logger
    {
        private Stopwatch _watch;

        protected long Elapsed { get { return _watch.ElapsedMilliseconds; } }

        private static readonly Color InfoColor = Color.DeepSkyBlue;
        private static readonly Color DoneColor = Color.LimeGreen;
        private static readonly Color ErrorColor = Color.IndianRed;
        private static readonly Color WarnColor = Color.Orange;

        private string InfoPrefix = " INFO ".Pastel(Color.Black).PastelBg(InfoColor);
        private string DonePrefix = " DONE ".Pastel(Color.Black).PastelBg(DoneColor);
        private string ErrorPrefix = " ERROR ".Pastel(Color.Black).PastelBg(ErrorColor);
        private string WarningPrefix = " WARN ".Pastel(Color.Black).PastelBg(WarnColor);

        public Logger()
        {
            _watch = new Stopwatch();
        }

        protected void ResetAndStartWatch()
        {
            _watch.Reset();
            _watch.Start();
        }

        protected void StopWatch()
        {
            _watch.Stop();
        }

        protected void Info(string message, params object[] args)
        {
            WriteLine(InfoPrefix, Color.White, message, args);
        }

        protected void Info(Draw type, string message, params object[] args)
        {
            Color color = type == Draw.Color || type == Draw.PrefixAndColor ? InfoColor : Color.White;
            string prefix = type == Draw.Prefix || type == Draw.PrefixAndColor ? InfoPrefix : "";

            WriteLine(prefix, color, message, args);
        }

        protected void Done(string message, params object[] args)
        {
            WriteLine(DonePrefix, Color.White, message, args);
        }

        protected void Done (Draw type, string message, params object[] args)
        {
            Color color = type == Draw.Color || type == Draw.PrefixAndColor ? DoneColor : Color.White;
            string prefix = type == Draw.Prefix || type == Draw.PrefixAndColor ? DonePrefix : "";

            WriteLine(prefix, color, message, args);
        }

        protected void Error(string message, params object[] args)
        {
            WriteLine(ErrorPrefix, Color.White, message, args);
        }

        protected void Error(Draw type, string message, params object[] args)
        {
            Color color = type == Draw.Color || type == Draw.PrefixAndColor ? ErrorColor : Color.White;
            string prefix = type == Draw.Prefix || type == Draw.PrefixAndColor ? ErrorPrefix : "";

            WriteLine(prefix, color, message, args);
        }

        protected void Warn(string message, params object[] args)
        {
            WriteLine(WarningPrefix, Color.White, message, args);
        }

        protected void Warn(Draw type, string message,  params object[] args)
        {
            Color color = type == Draw.Color || type == Draw.PrefixAndColor ? WarnColor : Color.White;
            string prefix = type == Draw.Prefix || type == Draw.PrefixAndColor ? WarningPrefix : "";
    
            WriteLine(prefix, color, message, args);
        }

        protected void NewLine()
        {
            Console.WriteLine("");
        }

        private void WriteLine(string prefix, Color color, string message, params object[] args)
        { 
            message = message.Pastel(color);
            if (prefix == string.Empty)
            {
                Console.WriteLine(message, args);
            }
            else
            {
                Console.WriteLine(prefix + " " + message, args);
            }
        }
    }
}
