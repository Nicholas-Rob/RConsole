using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace RConsole.Plugins
{
    class WindowPlugin : CommandBase
    {
        const string KNOWN_PROGRAM_LOCS = "known_program_locs.txt";
        private static Dictionary<string, string> knownProgramLocations;

        static bool DictionaryChanged = false;

        

        [OnLoad()]
        public static void OnLoadFunction()
        {


            if (File.Exists(KNOWN_PROGRAM_LOCS))
            {
                knownProgramLocations = ReadFile();
            }
            else
            {
                knownProgramLocations = new Dictionary<string, string>();
            }

            GetWindowFileLocations();
        }

        [OnUnload()]
        public static void OnUnloadFunction()
        {
            

            GetWindowFileLocations();

            if (knownProgramLocations.Count > 1 && DictionaryChanged)
            {
                SaveFile();
            }

        }

        [Command("min")]
        public static bool MinimizeCommand(string[] args)
        {
            if (args[0] == null)
            {
                ChangeWindowView(GetForegroundWindow(), 6);
            }
            else
            {
                IntPtr handle = GetHandleByDesc(ArrayToString(args));

                if (!ChangeWindowView(handle, 6))
                {
                    handle = GetHandleByTitle(ArrayToString(args));
                    ChangeWindowView(handle, 6);
                }

            }
            return true;
        }

        [Command("max")]
        public static bool MaximizeCommand(string[] args)
        {
            if (args[0] == null)
            {
                ChangeWindowView(GetForegroundWindow(),3);
            }
            else
            {

                IntPtr handle = GetHandleByDesc(ArrayToString(args));

                if (!ChangeWindowView(handle, 3))
                {
                    handle = GetHandleByTitle(ArrayToString(args));
                    ChangeWindowView(handle, 3);
                }
                //SetWindowToForeground(handle);
            }
            return true;
        }

        [Command("red")]
        public static bool ReduceCommand(string[] args)
        {
            if (args[0] == null)
            {
                ChangeWindowView(GetForegroundWindow(),1);
            }
            else
            {

                IntPtr handle = GetHandleByDesc(ArrayToString(args));

                if (!ChangeWindowView(handle, 1))
                {
                    handle = GetHandleByTitle(ArrayToString(args));
                    ChangeWindowView(handle, 1);
                }
                
            }
            return true;
        }

        [Command("size")]
        public static bool ResizeCommand(string[] args)
        {
            if (args.Length == 2) {
                ResizeWindowByHandle(GetForegroundWindow(), Convert.ToInt32(args[0]), Convert.ToInt32(args[1]));
            }else if (args.Length > 2)
            {
                IntPtr handle = GetHandleByDesc(ArrayToString(args.Take(args.Length - 2).ToArray()));
                ResizeWindowByHandle(handle, Convert.ToInt32(args[args.Length-2]), Convert.ToInt32(args[args.Length-1]));
            }
            else
            {
                return false;
            }

            return true;

        }

        [Command("move")]
        public static bool MoveWindowCommand(string[] args)
        {
            if (args.Length == 2)
            {
                RepositionWindowByHandle(GetForegroundWindow(), Convert.ToInt32(args[0]), Convert.ToInt32(args[1]));
            }
            else if (args.Length > 2)
            {
                IntPtr handle = GetHandleByDesc(ArrayToString(args.Take(args.Length - 2).ToArray()));
                RepositionWindowByHandle(handle, Convert.ToInt32(args[args.Length - 2]), Convert.ToInt32(args[args.Length - 1]));
            }
            else
            {
                return false;
            }

            return true;

        }

        [Command("processes")]
        public static bool ProcessListCommand(string[] args)
        {
            DisplayProcesses(false);
            return true;
        }
        [Command("windows")]
        public static bool OpenWindowsListCommand(string[] args)
        {
            DisplayProcesses(true);
            return true;
        }

        [Command("close")]
        public static bool CloseProcessCommand(string[] args)
        {
            if (args[0] != null) {
                CloseAppWindow(args[0]);
            }
            return true;
        }

        [Command("switch")]
        public static bool SwitchMonitorCommand(string[] args)
        {
            if (args[0] == null) return false;

            try
            {
                int screen = Convert.ToInt32(args[args.Length - 1]);
                if (screen > 0 && screen < 3)
                {
                    if (args.Length == 1)
                    {
                        IntPtr handle = GetForegroundWindow();
                        MoveWindowByHandle(handle, screen);
                        ChangeWindowView(handle, 3);
                    }
                    else
                    {

                        IntPtr handle = GetHandleByDesc(ArrayToString(args.Take(args.Length - 1).ToArray()));
                        MoveWindowByHandle(handle, Convert.ToInt32(args[args.Length - 1]));
                        ChangeWindowView(handle, 3);


                    }
                }
            }catch(Exception e)
            {

            }

            return true;
        }

        [Command("tile")]
        public static bool TileWindowsCommand(string[] args)
        {
            if (args[0] == null) TileWindowsByMainHandle(IntPtr.Zero);

            return true;
        }

        [Command("wr")]
        public static bool RefreshWindowData(string[] args)
        {
            GetWindowFileLocations();
            if (DictionaryChanged)
            {
                SaveFile();
            }
            return true;
        }

        [Command("run")]
        public static bool AppRunCommand(string[] args) {
            if (args[0] != "")
            {
                
                RunApp(ArrayToString(args));
            }

            return true;
        }

        [Command("wd")]
        public static bool DebugCommand(string[] args)
        {
            foreach (string k in knownProgramLocations.Keys)
            {
                Console.WriteLine(k + ": " + knownProgramLocations[k]);
            }
            return true;
        }

        [Command("use")]
        public static bool UseWindowCommand(string[] args)
        {
            if (args[0] != "")
            {
                SetForegroundWindow(GetHandleByDesc(ArrayToString(args)));
            }
            return true;
        }

        [Command("alpha")]
        public static bool SetAlphaCommand(string[] args)
        {
            if (args.Length == 0)
            {
                return false;
            }

            // Used for "toggle" argument
            if (args[0] == "toggle")
            {

                try
                {

                    // Window handle.
                    IntPtr handle = IntPtr.Zero;

                    // Default alpha value to toggle a window between 255 and itself.
                    byte nAlpha = 128;

                    // If window name specified, try to find the handle.
                    if (args.Length == 2)
                    {


                        // If the parameter after "toggle" is not a number, accept it as the name
                        // of a window.

                        // If it is a number, then nAlpha will equal the argument, and the handle will be to the foreground window.
                        if (!byte.TryParse(args[1], out nAlpha))
                        {
                            handle = GetHandleByProcessName(ArrayToString(args.Skip(1).ToArray()));
                        }
                        else
                        {
                            handle = GetForegroundWindow();
                        }
                        
                        
                    }
                    else if(args.Length == 3)// If no name specified, try to get handle for foreground window.
                    {
                        handle = GetHandleByProcessName(ArrayToString(args.Skip(2).ToArray()));
                        nAlpha = Convert.ToByte(args[1]);
                    }
                    else
                    {
                        handle = GetForegroundWindow();
                    }

                    ToggleAlpha(handle, nAlpha);

                    return true;

                }catch(Exception e)
                {
                    Console.WriteLine("Could not toggle alpha of window.");
                    return false;
                }

            }
            else
            {

                try
                {
                    byte alpha = Convert.ToByte(args[0]);


                    IntPtr handle = IntPtr.Zero;

                    if (args.Length == 2)
                    {
                        handle = GetHandleByProcessName(ArrayToString(args.Skip(1).ToArray()));
                    }
                    else
                    {
                        handle = GetForegroundWindow();
                    }

                    SetAlpha(handle, alpha);

                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Could not change alpha value of window.");
                    
                    return false;
                }
            }
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

        

        private static bool ChangeWindowView(IntPtr handle, int state)
        {

            return ShowWindow(handle, state);

        }

        

        private static IntPtr GetHandleByProcessName(string name)
        {
            IntPtr handle = IntPtr.Zero;

            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName.ToLower() == name.ToLower() && p.MainWindowTitle != "")
                {
                    
                    handle = p.MainWindowHandle;
                    break;

                }
            }

            if (handle == IntPtr.Zero)
            {
                return GetHandleByTitle(name);
            }

            return handle;
        }

        private static IntPtr GetHandleByTitle(string title)
        {
            IntPtr handle = IntPtr.Zero;

            foreach (Process p in Process.GetProcesses())
            {
                if (p.MainWindowTitle.ToLower() == title.ToLower())
                {

                    handle = p.MainWindowHandle;
                    break;

                }
            }

            return handle;
        }

        private static IntPtr GetHandleByDesc(string name)
        {
            IntPtr handle = IntPtr.Zero;

            foreach (Process p in Process.GetProcesses())
            {
                if (p.MainWindowTitle != "") {
                    try
                    {
                        string desc = p.MainModule.FileVersionInfo.FileDescription;
                        if (desc != null)
                        {
                            if (p.MainModule.FileVersionInfo.FileDescription.ToLower() == name.ToLower())
                            {
                                handle = p.MainWindowHandle;
                                break;
                            }
                        }
                        else
                        {
                            return GetHandleByProcessName(name);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

            }

            return handle;
        }


        

        private static void CloseAppWindow(string title)
        {
            foreach (Process p in Process.GetProcesses())
            {
                try
                {
                    if (p.MainWindowTitle != "")
                    {
                        ProcessModule m = p.MainModule;
                        string desc = m.FileVersionInfo.FileDescription;
                        if (desc != null) {
                            if (desc.ToLower() == title.ToLower())
                            {
                                p.CloseMainWindow();
                                break;

                            }
                        }
                        else
                        {
                            if (p.ProcessName.ToLower() == title)
                            {

                                p.CloseMainWindow();
                                break;
                            }
                            if (p.MainWindowTitle == title)
                            {
                                p.CloseMainWindow();
                                break;
                            }
                        }
                    }
                } catch (Exception e) { }

            }
        }

        private static void RepositionWindowByHandle(IntPtr handle, int x, int y)
        {
            SetWindowPos(handle, IntPtr.Zero, x, y, 0, 0, 0x0001 | 0x0004);
        }

        private static void ResizeWindowByHandle(IntPtr handle, int width, int height)
        {
            SetWindowPos(handle, IntPtr.Zero, 0, 0, width, height, 0x0002 | 0x0004);
        }

        private static void MoveWindowByHandle(IntPtr handle, int screen)
        {
            ChangeWindowView(handle, 1);
            SetWindowPos(handle, IntPtr.Zero, 2000 * (screen - 1), 200, 0, 0, 0x0001 | 0x0004);
            //ChangeWindowView(handle, 3);
        }

        private static void TileWindowsByMainHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                TileWindows(IntPtr.Zero, 0x0000, IntPtr.Zero, 2, IntPtr.Zero);
            }
            
        }

        private static void DisplayProcesses(bool windows)
        {
            if (windows)
            {
                foreach (Process p in Process.GetProcesses())
                {
                    if (p.MainWindowTitle != "")
                    {
                        try
                        {
                            ProcessModule m = p.MainModule;
                            Console.WriteLine(p.MainWindowTitle + ": " + p.ProcessName + ": " + p.Id + "::: " + m.FileVersionInfo.FileDescription);
                        } catch (Exception e) { }

                    }
                }
            }
            else
            {


                foreach (Process p in Process.GetProcesses())
                {
                    Console.WriteLine(p.ProcessName + ": " + p.Id);
                }
            }

        }

        private static void GetWindowFileLocations()
        {
            foreach (Process p in Process.GetProcesses())
            {
                if (p.MainWindowTitle != "")
                {
                    try
                    {

                        ProcessModule m = p.MainModule;
                        string name = m.FileVersionInfo.FileDescription;
                        if (name != null)
                        {
                            name = name.ToLower();
                        }
                        else
                        {
                            // If no file description, get program name without file type at the end.
                            string[] loc = m.FileVersionInfo.FileName.Split("\\");
                            name = loc[loc.Length - 1].Split('.')[0];
                        }

                        if (!knownProgramLocations.ContainsKey(name))
                        {
                            // Adds file location to loaded dictionary, and then overwrites known program locations json file with updated dictionary.
                            knownProgramLocations.Add(name, m.FileName);
                            DictionaryChanged = true;

                        }

                    }
                    catch (Exception e) { }

                }
            }
        }

        private static void RunApp(string name)
        {
            try
            {
                string[] nameArgs;

                if (name.Contains(';'))
                {
                    nameArgs = name.Split(';');

                }
                else
                {
                    nameArgs = new string[] { name };
                }
                ProcessStartInfo startinfo = new ProcessStartInfo(knownProgramLocations[nameArgs[0].Trim()]);

                if (nameArgs.Length > 1) {
                    startinfo.Arguments = nameArgs[1].Trim();
                }

                //startinfo.RedirectStandardInput = true;
                //startinfo.RedirectStandardOutput = true;

                Process proc = new Process();
                proc.StartInfo = startinfo;
                startinfo.CreateNoWindow = true;
                proc.Start();
                
                //RConsoleBase.AddActiveProcess(name, proc);
                //RConsoleBase.SwitchCurrentProcess(name);
            }
            catch (Exception e)
            {

            }
        }


        [DllImport("Libs\\CHelper.dll")]
        public static extern bool SetWindowAlphaHwnd(IntPtr win, byte alpha);

        // Set alpha of window chosen by handle.
        private static bool SetAlpha(IntPtr handle, byte alpha)
        {
            return SetWindowAlphaHwnd(handle, alpha);
        }


        [DllImport("Libs\\CHelper.dll")]
        public static extern bool ToggleWindowAlphaHwnd(IntPtr win, byte nAlpha);

        // Toggle window alpha between 255 and nAlpha.
        private static bool ToggleAlpha(IntPtr handle, byte nAlpha)
        {
            return ToggleWindowAlphaHwnd(handle, nAlpha);
        }

        private static Dictionary<string, string> ReadFile()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(KNOWN_PROGRAM_LOCS));
        }

        private static void SaveFile()
        {
            string json = JsonConvert.SerializeObject(knownProgramLocations);
            File.WriteAllText(KNOWN_PROGRAM_LOCS, json);
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
