using System;
using System.Collections.Generic;
using System.Text;

namespace RConsole
{
    // This class was made so that any command functions can be put in a class that inherits this class so that they can automatically be put
    // into a list to be registered into Command objects.
    public class CommandBase
    {
        public static List<object> bases;
        public CommandBase()
        {
            if (bases != null && !bases.Contains(this))
            {
                bases.Add(this);
            }
        }

        public static void InitCommandBaseList()
        {
            bases = new List<object>();
        }

        
    }
}
