namespace UniP2P.Debug
{
    public class UnityConsole : IConsole
    {
        public void Error(object meesage)
        {
            UnityEngine.Debug.LogError(meesage);
        }

        public void Log(object meesage)
        {
            UnityEngine.Debug.Log(meesage);
        }

        public void Warning(object meesage)
        {
            UnityEngine.Debug.LogWarning(meesage);
        }
    }
}
