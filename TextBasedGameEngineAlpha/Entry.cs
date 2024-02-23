using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine;
using TextBasedGameEngine.Tools;

namespace TextBasedGameEngine
{
    internal class HL_Entry
    {
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(System.IntPtr hWnd, int cmdShow);
        const int SW_MAXIMIZE = 3;


        static void Main(string[] args)
        {

            HL_Engine Engine = new HL_Engine();

            HL_EngineInfo Info;
            Info.TickRate = 128;
            Info.MaxFrameRate = 170.0;

            //Engine and client both combined since its a single player game i dont find any use of multiple libraries
        /*
            Info.OnFrameStart = HL_GameClient.OnFrameStart;
            Info.OnTick = HL_GameClient.OnTick;
            Info.OnFrameEnd = HL_GameClient.OnFrameEnd;
            Info.OnEngineInitialize = HL_GameClient.OnEngineInitialize;
        */

            Info.ScreenSize = HL_System.GetScreenSize();
            Info.DpiScaleFactor = HL_System.GetScreenDpiScale();

            Engine.InitializeEngine(ref Info);
            Engine.RunEngine();
        }
    }
}
