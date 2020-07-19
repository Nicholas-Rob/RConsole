using System;
using System.Collections.Generic;
using System.Text;

namespace RConsole
{
    class RConsoleBase
    {

        CommandHandler handler;

        public static bool Running = false;

        // Command instance currently running
        private static string instance = "base";

        public RConsoleBase()
        {

            handler = new CommandHandler();
   
        }

        public void Run()
        {
            Running = true;

            // Main console input loop
            while (Running)
            {
                Console.WriteLine();
                Console.Write(instance+"#: ");

                ExecuteCommand(Console.ReadLine());

            }
        }


        private void ExecuteCommand(string commandLine)
        {
            handler.Handle(commandLine.Trim().Split(' '));
        }

        public static void SetInstance(string instance)
        {
            RConsoleBase.instance = instance;
        }

        public static string GetInstance()
        {
            return instance;
        }

        public static void ResetInstance()
        {
            instance = "base";
        }

        public static void Clear()
        {
            Console.Clear();
        }
    }
}
