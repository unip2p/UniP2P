using System.Collections.Generic;

namespace UniP2P.Debug
{
    public static class Debugger
    {
        public static List<IConsole> Consoles = new List<IConsole>();

        public static void SetConsole(IConsole ic)
        {
            Consoles.Add(ic);
        }

        public static void Log(object message)
        {
            foreach (var console in Consoles)
            {
                console.Log(message);
            }
        }

        public static void Warning(object message)
        {
            foreach (var console in Consoles)
            {
                console.Warning(message);
            }
        }

        public static void Error(object message)
        {
            foreach (var console in Consoles)
            {
                console.Error(message);
            }
        }

    }

}