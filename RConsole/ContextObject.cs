using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RConsole
{
    class ContextObject
    {
        private string[] info = new string[2];
        public ContextObject(string command, string args)
        {
            info[0] = command;
            info[1] = args;
        }

        public ContextObject(string fullCommand)
        {
            string[] splitCommand = fullCommand.Split(' ');

            info[0] = splitCommand[0];
            info[1] = splitCommand.Skip(1).ToArray().ArrayString();
        }

        public string GetCommand()
        {
            return info[0];
        }

        public string GetArgs()
        {
            return info[1];
        }


        public override string ToString()
        {
            return info[0] + " " + info[1];
        }
    }
}
