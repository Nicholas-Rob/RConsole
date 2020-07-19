using System;
using System.Collections.Generic;
using System.Text;

namespace RConsole.Plugins
{
    // Test plugin
    class TestPlugin : CommandBase
    {
        [Command("plugin")]
        public static bool TestPluginCommand(string[] args)
        {
            if (args[0] == "help")
            {
                Console.WriteLine("Usage: plugin");
                Console.WriteLine("Just a test command from a plugin.");
                return true;
            }

            Console.WriteLine("PLUGIN");
            return true;
        }
    }
}
