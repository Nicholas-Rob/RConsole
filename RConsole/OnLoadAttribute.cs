using System;
using System.Collections.Generic;
using System.Text;

namespace RConsole
{
    [AttributeUsage(AttributeTargets.Method)]
    class OnLoadAttribute : Attribute
    {

        public OnLoadAttribute()
        {

        }
    }
}
