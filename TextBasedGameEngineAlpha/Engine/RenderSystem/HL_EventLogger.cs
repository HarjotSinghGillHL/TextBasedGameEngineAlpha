using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Tools;

public struct EventLog
{
    public string Log;
    public Vector2 vecRenderPosition;
    public int RenderTick;
}

namespace TextBasedGameEngineAlpha.Engine.RenderSystem
{
    public class HL_EventLogger
    {
        public static Vector2 vecRenderStart;
        //a very basic event logger i made but i am sure i can improve it alot maybe you could give me feedback on it
        public static void InitializeEventLogger()
        {
            vecRenderStart = new Vector2(Console.CursorLeft, Console.CursorTop);
            Logs = new List<EventLog>();
        }

        private static List<EventLog> Logs;
        public static void PushLog(string _Log)
        {
           // EventLog Log = new EventLog();
           // Log.Log = _Log;
            //Log.vecRenderPosition = 
            //Logs.Add(Log);
        }
        public static void HandleEventLogger()
        {

        }

    }
}
