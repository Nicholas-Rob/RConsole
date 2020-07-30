using System;
using System.Collections.Generic;
using System.Text;

namespace RConsole
{
    class Context
    {

        private BStack stack = new BStack(5, true);

        public Context()
        {
            stack.Add(new ContextObject("base", ""));
        }

        public void AddUsedCommand(string command)
        {
            stack.Add(new ContextObject(command));
            
        }

        public string GetFullCommandFromContext(int count)
        {
            ContextObject context = ((ContextObject)stack.Peek(count));
            return context.GetCommand() + " " + context.GetArgs();
        }

        public string GetCommandFromContext(int count)
        {
            return ((ContextObject)stack.Peek(count)).GetCommand();
        }

        public string GetArgsFromContext(int count)
        {
            return ((ContextObject)stack.Peek(count)).GetArgs();
        }

        public void DisplayContext()
        {
            stack.Display();
        }
    }
}
