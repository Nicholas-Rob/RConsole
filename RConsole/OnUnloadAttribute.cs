using System;
using System.Collections.Generic;
using System.Text;

namespace RConsole
{
    [AttributeUsage(AttributeTargets.Method)]
    class OnUnloadAttribute : Attribute
    {

        public OnUnloadAttribute()
        {

        }
    }
}
