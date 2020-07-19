using System;
using System.Collections.Generic;
using System.Text;

namespace RConsole
{
    // Command attribute for detecting Command functions.
    [AttributeUsage(AttributeTargets.Method)]
    class CommandAttribute : Attribute
    {

        public string Name { get; }
        public string[] Aliases { get; set; }


        // This is pretty neat. I can use this to "setup" an "instance" of this class if this value is true.
        // What can be done is that I can call up this command by inputting only the command name into the console
        // input, and then this command will be "in focus". By that I mean that any inputs after will be arguments for this command only, until "quit" is inputted.
        // If I don't want the command to be "in focus", then I can call the command the normal way and do a one-shot execution of it.

        // Logic for this is found in CommandHandler.Handle()
        public bool IsInstance;
        public CommandAttribute(string name)
        {
            Name = name;           
        }
        
        
    }
}
