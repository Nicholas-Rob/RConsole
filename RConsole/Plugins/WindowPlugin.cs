using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace RConsole.Plugins
{
    class WindowPlugin : CommandBase
    {

        [Command("min")]
        public static bool MinimizeCommand(string[] args)
        {
            if (args[0] == null)
            {
                ChangeWindowView(6);
            }
            else
            {
                ChangeWindowView(GetHandleByName(ArrayToString(args)), 6);
                
            }
            return true;
        }

        [Command("max")]
        public static bool MaximizeCommand(string[] args)
        {
            if (args[0] == null)
            {
                ChangeWindowView(3);
            }
            else
            {
                IntPtr handle = GetHandleByName(ArrayToString(args));

                ChangeWindowView(handle, 3); 
                SetWindowToForeground(handle);
            }
            return true;
        }

        [Command("processes")]
        public static bool ProcessListCommand(string[] args)
        {
            DisplayProcesses();
            return true;
        }

        [Command("close")]
        public static bool CloseProcessCommand(string[] args)
        {
            if (args[0] != null) {
                CloseProcess(args[0]);
            }
            return true;
        }

        [Command("wmove")]
        public static bool MoveWindowCommand(string[] args)
        {
            if (args[0] == null) return false;

            if (args.Length == 1)
            {
                MoveForgroundWindow(Convert.ToInt32(args[0]));
            }
            else
            {
                MoveWindowByHandle(GetHandleByName(args[0]), Convert.ToInt32(args[1]));
            }
            return true;
        }

        [Command("wtile")]
        public static bool TileWindowsCommand(string[] args)
        {
            if (args[0] == null) TileWindowsByMainHandle(IntPtr.Zero);

            /*
            if (args.Length == 1)
            {
                TileWindowsByMainHandle(GetHandleByName(args[0]));
            } */
            return true;
        }



        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr handle);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr handle, int cmdShow);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr TileWindows(IntPtr parent, int wHow, IntPtr rect, int kids, IntPtr lpKids);
        private static void ChangeWindowView(int state)
        {
            IntPtr handle = GetForegroundWindow();

            ShowWindow(handle, state);

            
        }

        private static void ChangeWindowView(IntPtr handle, int state)
        {

            ShowWindow(handle, state);
            
        }

        private static IntPtr GetHandleByName(string name)
        {
            IntPtr handle = IntPtr.Zero;

            foreach(Process p in Process.GetProcesses())
            {
               if (p.ProcessName == name)
                {

                    handle = p.MainWindowHandle;
                    break;
                    
               } 
            }

            return handle;
        }

        private static void CloseProcess(string name)
        {
            foreach (Process p in Process.GetProcesses())
            {
                if(p.ProcessName == name)
                {
                    p.CloseMainWindow();
                }
            }
        }


        private static void MoveForgroundWindow(int screen)
        {
            IntPtr handle = GetForegroundWindow();
            SetWindowPos(handle, IntPtr.Zero, 2000 * (screen - 1), 50, 0, 0, 0x0001 | 0x0004);
            ChangeWindowView(handle, 3);
        }

        private static void SetWindowToForeground(IntPtr handle)
        {
            SetForegroundWindow(handle);
        }

        private static void MoveWindowByHandle(IntPtr handle, int screen)
        {
            SetWindowPos(handle, IntPtr.Zero, 2000 * (screen - 1), 50, 0, 0, 0x0001 | 0x0004);
            ChangeWindowView(handle, 3);
        }

        private static void TileWindowsByMainHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                TileWindows(IntPtr.Zero, 0x0000, IntPtr.Zero, 2, IntPtr.Zero);
            }
            /*else
            {
                TileWindows(GetForegroundWindow(), 0x0000, IntPtr.Zero, 2, IntPtr.Zero);
            } */
        }

        private static void DisplayProcesses()
        {  

            foreach (Process p in Process.GetProcesses())
            {
                          Console.WriteLine(p.ProcessName + ": " + p.Id);     
            }
 
        }

        private static string ArrayToString(string[] array)
        {
            string result = "";
            foreach(string s in array)
            {
                result += s + " ";
            }
            return result.Trim();

            
        }
    }
}
