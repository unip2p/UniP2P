using System.Collections.Generic;

namespace UniP2P.Debug
{
    public static class DebbugerMessages
    {
        public static List<DebbugerMessage> Messages = new List<DebbugerMessage>();

        public static void AddMessage(object message, TypeDebugger type)
        {
            Messages.Add(new DebbugerMessage(type, message.ToString()));
        }

        public static void ClearMeesages()
        {
            Messages.Clear();
        }

        public static int GetMessagesCount()
        {
            return Messages.Count;
        }
    }

    public class DebbugerMessage
    {
        public DebbugerMessage(TypeDebugger type ,string mes)
        {
            Type = type;
            Message = mes;
        }
        public TypeDebugger Type;
        public string Message;
    }
}