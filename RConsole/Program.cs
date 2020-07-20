using System;

namespace RConsole
{
    class Program
    {
        

        static void Main(string[] args)
        {
            RConsoleBase console = new RConsoleBase();

            if (args.Length > 0)
            {
                console.TempRun(args);
            }
            else
            {
                

                console.Run();
            }
        }
    }
}
