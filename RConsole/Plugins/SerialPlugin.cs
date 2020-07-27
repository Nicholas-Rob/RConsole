using System;
using System.Collections.Generic;
using System.Linq;
using Helper;

namespace RConsole.Plugins
{
    class SerialPlugin : CommandBase
    {
        [OnUnload]
        public static void OnUnloadFunction()
        {
            SerialCommunication.SaveFile();
        }

        [Command("serial")]
        public static bool SerialCommand(string[] args)
        {
            switch (args[0])
            {
                case "help":
                    Console.WriteLine("Usage: serial [set] [args...]");
                    Console.WriteLine("Sets serial configurations.");
                    break;

                case "set":

                    SetButtonCommand(args.Skip(1).ToArray());
                    break;

                case "create":
                    CreateButtonLayout(args.Skip(1).ToArray());
                    break;

                case "edit":
                    EditButtonLayout(args.Skip(1).ToArray());
                    break;
                case "layouts":
                    DisplayLayouts();
                    break;

                default:
                    return false;
                    break;
            }

            return true;
        }

        private static void SetButtonCommand(string[] args)
        {
            if(args.Length > 0)
            {
                SerialCommunication.SetLayout(args);
            }
        }

        private static void CreateButtonLayout(string[] args)
        {
            if (args.Length == 1)
            {
                if (!SerialCommunication.LayoutExists(args[0]))
                {
                    string name = args[0].ToLower().Trim();

                    Console.Write("Button A: ");
                    string command_a = Console.ReadLine().ToLower().Trim();

                    Console.Write("Button B: ");
                    string command_b = Console.ReadLine().ToLower().Trim();

                    SerialCommunication.CreateLayout(new string[] { name, command_a, command_b });
                }
                else
                {
                    Console.WriteLine("Layout already exists!");
                }
            }
        }

        private static void EditButtonLayout(string[] args)
        {
            if (args.Length >= 3)
            {
                if (SerialCommunication.LayoutExists(args[0]))
                {
                    switch (args[1])
                    {
                        case "a":
                            SerialCommunication.EditLayout(args[0].Trim(), 0, args.Skip(2).ToArray().ArrayString().Trim());
                            break;
                        case "b":
                            SerialCommunication.EditLayout(args[0].Trim(), 1, args.Skip(2).ToArray().ArrayString().Trim());
                            break;
                        default:
                            Console.WriteLine("Invalid Button");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Layout does not exist!");
                }
            }
            
        }

        private static void DisplayLayouts()
        {
            SerialCommunication.DisplayLayouts();
        }
    }
}
