using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace RConsole
{
    class Scheduler
    {

        private static Dictionary<Timer, string> timers = new Dictionary<Timer, string>();

        public void Set(string command, DateTime time)
        {
            TimeSpan timeSpan = time.TimeOfDay.Subtract(DateTime.Now.TimeOfDay);

            if (timeSpan.TotalMilliseconds < 0)
            {
                
                timeSpan = (DateTime.MaxValue.TimeOfDay.Subtract(DateTime.Now.TimeOfDay)).Add(time.TimeOfDay);
            }

            Console.WriteLine(timeSpan.ToString());

            Timer timer = new Timer(timeSpan.TotalMilliseconds);
            timer.AutoReset = false;

            timers.Add(timer, command);

            timer.Elapsed += Execute_Command;

            timer.Start();
        }

        private void Execute_Command(object sender, ElapsedEventArgs e )
        {
            Timer tSender = (Timer)sender;

            
            RConsoleBaseHelper.console.ExecuteCommand(timers[tSender]);
            

            timers.Remove(tSender);

            tSender.Dispose();

            
        }
    }
}
