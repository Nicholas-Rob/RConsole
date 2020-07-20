using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RConsole
{
    // Basic commands
    public class Commands : CommandBase
    {

        [Command("test", IsInstance = true)]
        public static bool TestCommand(string[] args) 
        {
            if (args[0] == "help")
            {
                Console.WriteLine("Usage: test [args...]");
                Console.WriteLine("Test function. Displays the last argument in [args...]");
                return true;
            }
            Console.WriteLine(args[args.Length - 1]);
            return true;

        }

        [Command("quit")]
        public static bool QuitCommand(string[] args)
        {
            if (args[0] == "help")
            {
                Console.WriteLine("Usage: quit");
                Console.WriteLine("Quits current instance. If current instance is 'base', then it will exit the console.");
                return true;
            }

            CommandHandler.Exit();
            
            return true;
        }

        [Command("clear")]
        public static bool ClearCommand(string[] args)
        {
            if (args[0] == "help")
            {
                Console.WriteLine("Usage: clear");
                Console.WriteLine("Clears console.");
                return true;
            }

            RConsoleBase.Clear();
            return true;
        }

        [Command("help")]
        public static bool HelpCommand(string[] args)
        {
            if (args[0] == null || !CommandHandler.GetCommandDictionary().ContainsKey(args[0]))
            {
                foreach (string name in CommandHandler.GetCommands(true))
                {
                    Console.WriteLine(name);
                }
            }
            else
            {
                CommandHandler.GetCommand(args[0]).Execute(args.Skip(1).Concat(new string[] { "help" }).ToArray());
            }
            return true;
        }
    }



    
}
