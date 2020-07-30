using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Helper;

namespace RConsole
{
    class RConsoleBase
    {

        public static RConsoleBase console;
        public static CommandHandler cHandler;
        public static BluetoothManager bManager;
        public static SerialCommunication serial;

        private Context context;

        public static bool Running = false;

        // Command instance currently running
        public static string instance = "base";
        private static string processInstance = "base";

        private static Dictionary<string, Process> ActiveProcesses = new Dictionary<string, Process>();

        /*
        private static TextWriter writer;
        private static TextReader reader;
        */
        
        ///////////////////////////////////////
        

        public RConsoleBase()
        {
            console = this;

            
            cHandler = new CommandHandler();

            bManager = new BluetoothManager(console);

            serial = new SerialCommunication(console);

            context = new Context();
            
        }

        ~RConsoleBase()
        {
           // writer.Dispose();
        }

        public void Run()
        {
            Running = true;

            /*
            writer = Console.Out;
            reader = Console.In;
            ReadStream(); */
            /////////////////////////////////////////////////////////////////
            // Main console input loop

            Console.WriteLine();
            Console.Write(instance + "#: ");

            while (Running)
            {
                string input = Console.ReadLine();
                ExecuteCommand(input);

                /* if (input.Trim().Split(' ')[0] == "sp")
                 {
                     SwitchCurrentProcess(input.Trim().Split(' ').Skip(1).ToArray().ArrayString());
                 } 
                 else
                 {
                if (processInstance == "base")
                    {
                        ExecuteCommand(input);
                    }
                    else
                    {
                        //writer.Write(input);
                    }
                } */

            }
        }

        public void TempRun(string[] args)
        {
            ExecuteCommand(ArrayToString(args));
        }

        public void ExecuteCommand(string commandLine)
        {
            //Console.Write(commandLine);

            

            if (commandLine.Trim() == "context")
            {
                context.DisplayContext();
            }
            else
            {
                

                bool result = cHandler.Handle(commandLine.Trim().Split(' '));

                if (result)
                {
                    context.AddUsedCommand(commandLine.Trim());
                }

                Console.WriteLine();
                Console.Write(instance + "#: ");
            }
        }

        public static void SetInstance(string instance)
        {
            RConsoleBase.instance = instance;
        }

        public static string GetInstance()
        {
            return instance;
        }

        public static void AddActiveProcess(string name, Process proc)
        {
            if (!ActiveProcesses.ContainsKey(name))
            {
                ActiveProcesses.Add(name, proc);

            }
        }

        public static void SwitchCurrentProcess(string procName)
        {
            if (procName == "base")
            {
              //  writer.Close();
                processInstance = "base";
            }
            else
            {
                
                if (ActiveProcesses.ContainsKey(procName))
                {
                    
                    processInstance = procName;
                    
                  //  writer = ActiveProcesses[procName].StandardInput;
                  //  reader = ActiveProcesses[procName].StandardOutput;
                    //ReadStream();

                }
            }
        }

        /*
        private static void ReadStream()
        {
            new Thread(() =>
            {
                while (true)
                {
                    int current;
                    
                    while((current = reader.Read()) >= 0)
                    {
                        Console.Write((char)current);
                    }
                }
            }).Start();
        } *////

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
