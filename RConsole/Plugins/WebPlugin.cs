using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RConsole.Plugins
{
    class WebPlugin : CommandBase
    {

        [Command("web", IsInstance = true, Hide = true)]
        public static bool WebCommand(string[] args)
        {

            switch (args[0])
            {
                case "help":
                    Console.WriteLine("Usage: web [url]");
                    Console.WriteLine("Opens [url].");
                    break;

                default:

                    ProcessStartInfo process = new ProcessStartInfo("https://" + args[0]);
                    Process.Start(process);
                    break;
            }
            return true;
        }
    }
}
