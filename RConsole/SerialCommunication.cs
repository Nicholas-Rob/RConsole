using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using Helper;
using Newtonsoft.Json;

namespace RConsole
{
    class SerialCommunication
    {

        public static string COMMAND_A = "music prev";
        public static string COMMAND_B = "music next";

        const string LAYOUTS = "layouts.txt";
        private static Dictionary<string, string[]> layouts;

        RConsoleBase console;
        SerialPort port;
        bool read;
        bool write;

        public SerialCommunication(RConsoleBase console)
        {
            string portName = SearchPorts();

            if (portName != null)
            {
                this.console = console;

                port = new SerialPort(portName, 9600);
                port.RtsEnable = true;

                port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);

                port.Open();

                Console.WriteLine("Serial Communicaion Established - " + port.PortName);

                if (File.Exists(LAYOUTS))
                {
                    layouts = ReadFile();
                }
                else
                {
                    layouts = new Dictionary<string, string[]>();
                }
            }

            
        }

        ~SerialCommunication()
        {
            port.Close();

            SaveFile();
        }

        private string SearchPorts()
        {
            using(var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
            {
                
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();  

                foreach (var p in ports)
                {
                    if (p["Caption"].ToString().Contains("Arduino Micro"))
                    {
                        return p["DeviceID"].ToString();
                    }
                }
                
                return null;
            }
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int data = port.ReadByte();

            switch (data)
            {
                case 1:
                    console.ExecuteCommand(COMMAND_A);
                    break;
                case 2:
                    console.ExecuteCommand(COMMAND_B);
                    break;
                default:
                    break;
            }
        }

        public static void SetLayout(string[] args)
        {
            if (layouts.ContainsKey(args[0]))
            {
                COMMAND_A = layouts[args[0]][0];
                COMMAND_B = layouts[args[0]][1];
            }
            else if (args.Length > 1 && args[0] == "a" || args[0] == "b")
            {
                switch (args[0].ToLower())
                {

                    case "a":

                        COMMAND_A = args.Skip(1).ToArray().ArrayString().ToLower();
                        break;
                    case "b":
                        COMMAND_B = args.Skip(1).ToArray().ArrayString().ToLower();
                        break;
                    default:
                        break;
                }
            }
            
        }

        public static void CreateLayout(string[] args)
        {
            if (!layouts.ContainsKey(args[0]))
            {
                layouts.Add(args[0], args.Skip(1).ToArray());
            }
            else
            {
                Console.WriteLine("Layout already exists!");
            }
        }

        public static void EditLayout(string name, int button, string command)
        {
            if (layouts.ContainsKey(name))
            {
                layouts[name][button] = command;
            }
            else
            {
                Console.WriteLine("Layout does not exist!");
            }
        }

        public static bool LayoutExists(string name)
        {
            return layouts.ContainsKey(name);
        }

        public static void DisplayLayouts()
        {
            foreach(string layout in layouts.Keys)
            {
                Console.WriteLine("\nName: " + layout);
                Console.WriteLine("Button A: " + layouts[layout][0]);
                Console.WriteLine("Button B: " + layouts[layout][1] + "\n");
                
            }
        }

        private static Dictionary<string, string[]> ReadFile()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string[]>>(File.ReadAllText(LAYOUTS));
        }

        public static void SaveFile()
        {
            string json = JsonConvert.SerializeObject(layouts);
            File.WriteAllText(LAYOUTS, json);
        }
    }
}
