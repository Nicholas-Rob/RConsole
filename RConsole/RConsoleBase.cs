using System;
using System.Collections.Generic;
using System.Text;

namespace RConsole
{
    class RConsoleBase
    {

        static RConsoleBase console;
        static CommandHandler cHandler;
        static BluetoothManager bManager;
        

        public static bool Running = false;

        // Command instance currently running
        private static string instance = "base";

        public RConsoleBase()
        {
            console = this;

            cHandler = new CommandHandler();

            bManager = new BluetoothManager(console);
        }

        public void Run()
        {
            Running = true;

            bManager.Run();
            // Main console input loop

            Console.WriteLine();
            Console.Write(instance + "#: ");

            while (Running)
            {
                

                ExecuteCommand(Console.ReadLine());

            }
        }

        public void TempRun(string[] args)
        {
            ExecuteCommand(ArrayToString(args));
        }

        public void ExecuteCommand(string commandLine)
        {
            Console.Write(commandLine);

            cHandler.Handle(commandLine.Trim().Split(' '));

            Console.WriteLine();
            Console.Write(instance + "#: ");
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

        private static string ArrayToString(string[] array)
        {
            string result = "";
            foreach (string s in array)
            {
                result += s + " ";
            }
            return result.Trim();


        }
    }
}
