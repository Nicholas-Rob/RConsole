using System;
using System.Collections.Generic;
using System.Text;

namespace RConsole.Plugins
{
    class BluetoothPlugin : CommandBase
    {
        [Command("bt")]
        public static bool BluetoothCommand(string[] args)
        {
            if (args[0] == "help")
            {
                Console.WriteLine("Usage: bt");
                Console.WriteLine("Starts listening for bluetooth device.");
                return true;
            }
            else
            {
                return RConsoleBase.TryBluetooth();
            }

        }
    }
}
