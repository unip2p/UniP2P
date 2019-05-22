namespace UniP2P.Debug
{
    public class SystemConsole : IConsole
    {
        public void Error(object meesage)
        {
            System.Console.WriteLine("Error:" + meesage.ToString());
        }

        public void Log(object meesage)
        {
            System.Console.WriteLine("Log:" + meesage.ToString());
        }

        public void Warning(object meesage)
        {
            System.Console.WriteLine("Warning:" + meesage.ToString());
        }
    }
}