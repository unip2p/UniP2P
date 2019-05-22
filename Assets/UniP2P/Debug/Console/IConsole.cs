namespace UniP2P.Debug
{
    public interface IConsole
    {
        void Log(object message);

        void Warning(object message);

        void Error(object message);
    }
}
