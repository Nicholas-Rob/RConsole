using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RConsole.Plugins
{
    class SchedulePlugin : CommandBase
    {
        private static Scheduler scheduler = new Scheduler();

        [Command("schedule")]
        public static bool ScheduleFunction(string[] args)
        {
            if(args[0] == "help")
            {
                return true;
            }

            if(args.Length < 3)
            {
                return false;
            }

            CreateSchedule(Convert.ToDouble(args[0]), Convert.ToDouble(args[1]), args.Skip(2).ToArray());

            return true;
        }


        private static void CreateSchedule(double hour, double minute, string[] command)
        {
            
            scheduler.Set(command.ArrayString(), new DateTime().AddHours(hour).AddMinutes(minute));
        }
    }
}
