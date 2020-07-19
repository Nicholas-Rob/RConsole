using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RConsole
{
    public static class PluginLoader
    {
        public static void LoadPlugins()
        {
            if (Directory.Exists("Plugins"))
            {
                string[] files = Directory.GetFiles("Plugins");

                foreach(string file in files)
                {
                    if (file.EndsWith(".dll"))
                    {
                        Assembly.LoadFile(Path.GetFullPath(file));
                    }
                }
            }

            Type fileType = typeof(CommandBase);

            Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(p => fileType.IsAssignableFrom(p) && p.IsClass).ToArray();

            foreach(Type type in types)
            {
                CommandBase.bases.Add((CommandBase)Activator.CreateInstance(type));
            }
        }
    }
}
