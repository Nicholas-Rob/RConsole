using Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RConsole.Plugins
{
    class WebPlugin : CommandBase
    {

        [Command("web", IsInstance = false, Hide = false)]
        public static bool WebCommand(string[] args)
        {

            switch (args[0])
            {
                case "help":
                    Console.WriteLine("Usage: web [url]");
                    Console.WriteLine("Opens [url].");
                    break;

                default:

                    var proc = new ProcessStartInfo
                    {
                        FileName = "https://" + args[0],
                        UseShellExecute = true
                    };
                    Process.Start(proc);
                    break;
            }
            return true;
        }

        [Command("google")]
        public static bool GoogleSearchCommand(string[] args)
        {

            string query = args.ArrayString();

            var proc = new ProcessStartInfo
            {
                FileName = "https://www.google.com/search?&q=" + query,
                UseShellExecute = true
            };

            Process.Start(proc);

            return true;
        }
    }
}
