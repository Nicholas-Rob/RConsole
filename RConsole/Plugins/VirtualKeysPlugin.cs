using AngleSharp.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace RConsole.Plugins
{
    class VirtualKeysPlugin : CommandBase
    {

        [Command("vk")]
        public static bool VirtualKeysCommand(string[] args)    // Input key code values must be hex format.
        {
            if(args.Length == 0 || args[0] == "help") 
            {
                Console.WriteLine("Usage: vk [keys]");
                Console.WriteLine("activates the given virtual keys.");
                return true;
            }
            else
            {
                Queue<int> codes = new Queue<int>();

                foreach(string code in args){
                    try
                    {
                        int code_int = (int)UInt16.Parse(code, NumberStyles.HexNumber);

                        codes.Enqueue(code_int);

                    }catch(Exception e)
                    {
                        Console.WriteLine("Could not parse key code.");
                        return false;
                    }
                }

                return UseKeys(codes);
            }
        }

       // [DllImport("user32.dll")]
       // public static extern IntPtr GetForegroundWindow(); // Gets the foreground window to send virtual key press message to.

       // [DllImport("user32.dll")]
       // static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);


        const UInt32 WM_KEYDOWN = 0x0100;


        private static bool UseKeys(Queue<int> keys)
        {
            foreach (int key in keys)
            {
                try
                {
                    
                    keybd_event((byte)key, 0, 1, IntPtr.Zero);


                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to use virtual key.");
                    return false;
                }
            }

            foreach (int key in keys)
            {
                try
                {
                    
                    keybd_event((byte)key, 0, 3, IntPtr.Zero); // bring keys back up.


                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to bring virtual key back up.");
                    return false;
                }
            }

            return true;
        }
    }
}
