using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RConsole.Plugins
{
    class MusicPlugin : CommandBase
    {

        [Command("music",IsInstance = true)]
        public static bool MusicCommand(string[] args)
        {
            if (args[0] == "help")
            {
                Console.WriteLine("Usage: music [toggle/next/prev]");
                Console.WriteLine("Can play/pause current music, start the next track, or start the previous track.");
                return true;
            }

            int argCount = args.Length;

            switch (args[0])
            {
                case "toggle":
                    
                    if(argCount > 1 && args[1] == "help")
                    {
                        Console.WriteLine("Usage: music toggle");
                        Console.WriteLine("Play/pause current music.");
                        return true;
                    }
                    ToggleMusic();
                    break;

                case "next":

                    NextTrack();
                    break;

                case "prev":

                    PrevTrack();
                    break;

                default:
                    return false;
            }

            return true;
            
        }

        private const int PLAY_PAUSE = 0xB3;
        private const int NEXT_TRACK = 0xB0;
        private const int PREV_TRACK = 0xB1;

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        private static void ToggleMusic()
        {
            keybd_event(PLAY_PAUSE, 0, 1, IntPtr.Zero);
        }

        private static void NextTrack()
        {
            keybd_event(NEXT_TRACK, 0, 1, IntPtr.Zero);
        }

        private static void PrevTrack()
        {
            keybd_event(PREV_TRACK, 0, 1, IntPtr.Zero);
        }
    }
}
