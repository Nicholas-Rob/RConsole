using Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RConsole.Plugins
{
    class QuartusPlugin : CommandBase
    {

        private static string projectTop = null;
        private static string rev = null;

        [Command("q")]
        public static bool QuartusFunction(string[] args)
        {
            if(args[0] == "help")
            {
                return true;
            }

            if(args.Length >= 1)
            {
                switch (args[0])
                {
                    case "comp":
                        CompileProject(args.Skip(1).ToArray().ArrayString());
                        //Console.WriteLine("true");
                        break;

                    case "top":
                        projectTop = args[1].Trim();
                        Console.WriteLine(projectTop);
                        break;

                    case "rev":
                        rev = args[1].Trim();
                        Console.WriteLine(rev);
                        break;

                    default:
                        break;
                }
            }

            return true;
        }


        private static void CompileProject(string projectName)
        {

            if (projectTop == null)
            {
                Console.WriteLine("No top level chosen!");
                return;
            }
            if (rev == null)
            {
                Console.WriteLine("No rev chosen!");
                return;
            }

            Console.WriteLine("TOP: " + projectTop);
            Console.WriteLine("REV: " + rev);

            const string Q_LOC = @"C:\intelFPGA_lite\18.1\quartus\bin64\quartus_sh.exe";
            const string P_LOC = @"C:\intelFPGA_lite\18.1\";

            ProcessStartInfo startinfo = new ProcessStartInfo(Q_LOC);


            startinfo.Arguments = @"--flow compile " + P_LOC + projectTop + " -c " + rev;
            

            Process proc = new Process();
            proc.StartInfo = startinfo;
            startinfo.CreateNoWindow = false;
            proc.Start();

        }
    }
}
