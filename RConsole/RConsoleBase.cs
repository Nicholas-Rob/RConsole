using System;
using System.Collections.Generic;
using Helper;

namespace RConsole
{
    class RConsoleBase
    {

        public static RConsoleBase console;
        public static CommandHandler cHandler;
        public static BluetoothManager bManager;
        public static SerialCommunication serial;
        

        public static bool Running = false;

        // Command instance currently running
        public static string instance = "base";

        public RConsoleBase()
        {
            console = this;

            cHandler = new CommandHandler();

            bManager = new BluetoothManager(console);

            serial = new SerialCommunication(console);

            
        }

        public void Run()
        {
            Running = true;

            

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
            //Console.Write(commandLine);

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

        public static bool TryBluetooth()
        {
            bManager.Stop();
           return bManager.Run();
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
