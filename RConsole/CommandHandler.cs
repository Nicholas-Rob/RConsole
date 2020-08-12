using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

namespace RConsole
{
    // Yeah, I know that this is messy code. Fuck off. This is messy so that the rest doesn't have to be.
    class CommandHandler
    {
        
        private static Dictionary<string, Command> commands = new Dictionary<string, Command>();
        private static List<MethodInfo> OnLoadList = new List<MethodInfo>();
        private static List<MethodInfo> OnUnloadList = new List<MethodInfo>();

        public CommandHandler()
        {
            // Initiates the CommandBase list that will eventually hold all classes that inherit CommandBase to eventually get "Command functions"
            CommandBase.InitCommandBaseList();

            // Loads all dll files from the "Plugins" folder that inherit CommandBase.
            PluginLoader.LoadPlugins();

            // Finally, register all commands from CommandBase classes by creating Command objects using their data.
            RegisterCommands();

            ExecuteOnLoadFunctions();
        }

        public static void Exit()
        {
            ExecuteOnUnloadFunctions();
            RConsoleBase.Running = false;
        }

        // Handles command inputs and makes sure everything gets handled properly.
        public bool Handle(string[] args)
        {

            // If no args, return false
            if (args.Length == 0) return false;


            bool result;

            //  Run commands for instance if current instance isn't base instance
            if (RConsoleBase.GetInstance() != "base")
            {
                if (args[0] == "quit")
                {
                    RConsoleBase.ResetInstance();
                    return true;
                }
                else
                {
                    result = commands[RConsoleBase.GetInstance()].Execute(args);

                    if (!result)
                    {
                        // Calls commands with "help" argument
                        commands[RConsoleBase.GetInstance()].Execute(new string[] { "help" });
                        return false;
                    }

                    return true;
                }

            }


            Command command;

            if (commands.ContainsKey(args[0]))
            {
                // Get Command object from dictionary by passing the "command name" by string.
                command = commands[args[0]];
            }
            else
            {
                // No command of name args[0]
                return false;
            }

            // Throwing out the command name from the input arguments
            args = args.Skip(1).ToArray();


            if (args.Length == 0)
            {
                // if command can be an instance and current instance is not command, set current instance to command
                if (command.IsInstance && RConsoleBase.GetInstance() != command.Name)
                {
                    RConsoleBase.SetInstance(command.Name);
                    return true;
                }
                else
                {
                    // place holder
                    args = new string[1];
                }

            }


            // finally, execute command with args in the normal fashion
            result = command.Execute(args);

            if (!result)
            {
                // Calls command with "help" argument
                command.Execute(new string[] { "help" });
                return false;
            }

            return true;

        }



        // Takes all of the classes that inherit CommandBase and takes the functions with Command Attributes attached, and turns them into Command objects.
        private void RegisterCommands()
        {
            foreach(object cbase in CommandBase.bases)
            {
                // Filter out any classes that are found that are just "CommandBase" and leaving the classes that inherit from it.
                if (cbase.GetType() != typeof(CommandBase))
                {
                    
                    Type type = cbase.GetType();


                    MethodInfo[] members = type.GetMethods();

                    foreach (MethodInfo info in members)
                    {
                        Attribute[] attrs = Attribute.GetCustomAttributes(info);

                        foreach (Attribute attr in attrs)
                        {
                            if (attr is OnLoadAttribute)
                            {
                                OnLoadList.Add(info);
                            }else if(attr is OnUnloadAttribute)
                            {
                                OnUnloadList.Add(info);
                            }
                            else if (attr is CommandAttribute)
                            {
                                if (!((CommandAttribute)attr).Hide)
                                {
                                    string name = ((CommandAttribute)attr).Name;

                                    // Deals with duplicates of attributes. I still haven't figured out what is causing it.
                                    if (!commands.ContainsKey(name))
                                    {
                                        commands.Add(name, new Command(name, info, ((CommandAttribute)attr).IsInstance));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static Dictionary<string, Command> GetCommandDictionary()
        {
            return commands;
        }

        public static Object[] GetCommands(bool ByNames)
        {
            if (ByNames)
            {
                return commands.Keys.ToArray();
            }
            else
            {
                return commands.Values.ToArray();
            }
        }

        public static Command GetCommand(string name)
        {
            if (commands.ContainsKey(name))
            {
                return commands[name];
            }

            return null;
        }

        private static void ExecuteOnLoadFunctions()
        {
            foreach(MethodInfo func in OnLoadList)
            {
                func.Invoke(null, null);
            }
        }

        private static void ExecuteOnUnloadFunctions()
        {
            foreach (MethodInfo func in OnUnloadList)
            {
                func.Invoke(null, null);
            }
        }

        // Structure of Command object
        public class Command
        {
            public MethodInfo Method { get; }

            public string Name { get; }

            public bool IsInstance { get; }

            public Command(string Name, MethodInfo Method, bool IsInstance)
            {
                this.Name = Name;
                this.Method = Method;
                this.IsInstance = IsInstance;
            }

            public bool Execute(string[] args)
            {
                // Invoke MethodInfo using this class as representation, and using a new object array containing the arguments for the invoked function.

                try
                {
                    bool result = (bool)Method.Invoke(this, new object[] { args });
                    return result;
                }catch(Exception e)
                {
                    Console.WriteLine(e);
                }

                return false;
            }
        }


    }
}
