namespace UniP2P.Debug
{
    public class ExConsole : IConsole
    {
        public void Log(object meesage)
        {
            DebbugerMessages.AddMessage(meesage, TypeDebugger.Log);
        }

        public void Warning(object meesage)
        {
            DebbugerMessages.AddMessage(meesage, TypeDebugger.Warning);
        }

        public void Error(object meesage)
        {
            DebbugerMessages.AddMessage(meesage, TypeDebugger.Error);
        }
    }

    public enum TypeDebugger
    {
        Log = 0,
        Warning = 1,
        Error = 2
    }
}
