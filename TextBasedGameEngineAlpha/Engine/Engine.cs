using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using TextBasedGameEngine.Engine;
using TextBasedGameEngine.Engine.RenderSystem;
using TextBasedGameEngine.Tools;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using TextBasedGameEngine.Engine.GameManager;
using TextBasedGameEngineAlpha.Engine.GameManager.WorldEntities;
using TextBasedGameEngineAlpha.Engine.GameManager;
using TextBasedGameEngineAlpha.Engine.RenderSystem.Frames;

public struct HL_EngineInfo
{
    public double TickRate;
    public double MaxFrameRate;
    public Vector2 ScreenSize;
    public float DpiScaleFactor;
}

enum EEngineMode
{
    NONE = 0,
    GAME ,
    EDITOR,
    ENGINE_MODE_MAX,
}
namespace TextBasedGameEngine.Engine
{
    public class HL_Engine
    {
        public HL_GlobalVarsManager GlobalVarsMgr = null;
        public HL_RenderSystem RenderInstance = null;
        public HL_UserInterface UserInterface = null;
        public HL_GameManager GameManager = null;

        private static EEngineMode Mode;

        public bool DestructEngine = false;
        double LastUpdateTime = 0;
        bool bGameFinished = false;
        public void InitializeEngine(ref HL_EngineInfo InitInfo)
        {
            GlobalVarsMgr = new HL_GlobalVarsManager();
            RenderInstance = new HL_RenderSystem();
            UserInterface = new HL_UserInterface();
            GameManager = new HL_GameManager();
            GlobalVarsMgr.Initialize(InitInfo);
            RenderInstance.Initialize(this);
            UserInterface.Initialize(this);
            GameManager.Initialize(this);

            HL_System.SetConsoleWindowToFullScreen();
            Console.CursorVisible = false;
            Console.Title = "MiniGame : Hunt";
            HL_Console.SetConsoleFontSize(12, 25);

LABEL_SELECT_AGAIN:
            Console.WriteLine("Game : G");
            Console.WriteLine("Map Editor : M (Not Implemented Yet)");
            Console.WriteLine("Press a number key respective to mode");
            ConsoleKeyInfo KeyInfo = Console.ReadKey();
           
            Console.Clear();

            if (KeyInfo.Key == ConsoleKey.G)
            {
                Mode = EEngineMode.GAME;
                HL_MapManager.Load("Map.txt");
            }
            else if (KeyInfo.Key == ConsoleKey.M)
            {
                Mode = EEngineMode.EDITOR;
                Console.Write("Map Editor is not implemented yet!");
            }
            else
            {
                Console.Clear();
                goto LABEL_SELECT_AGAIN;
            }
        }

        public void FrameStart()
        {
            Console.CursorVisible = false;
            GlobalVarsMgr.OnFrameStart();

            if (Mode == EEngineMode.GAME)
            {
                if (!bGameFinished)
                    GameManager.LocalPlayer.HandleLocalPlayer(this);
            }
            else
            {

            }

            if ((GlobalVarsMgr.GVars.CurrentTime - LastUpdateTime) >= GlobalVarsMgr.GVars.IntervalPerTick)
            {
                OnTick();
                LastUpdateTime = GlobalVarsMgr.GVars.CurrentTime;
            }
        }

        public void OnTick()
        {
            if (Mode == EEngineMode.GAME)
            {
                if (!bGameFinished)
                {
                    GameManager.EntityHandler.HandleEntities();

                    if (HL_WorldEntityManager.WorldEntities.Count == 0 && HL_WorldEntityManager.CoinsConsumed.Count == HL_MapManager.iOriginalCoinsCount)
                    {
                        bGameFinished = true;
                        HL_GameFinishedScreen.FinishedScreen(this);
                    }
                }
            }

            GlobalVarsMgr.OnTick();
        }
        public void FrameEnd()
        {
            GlobalVarsMgr.OnFrameEnd();

            if (Mode == EEngineMode.GAME)
            {
                HL_HudOverlay.UpdateEngineState(GlobalVarsMgr.GVars.FrameRate, GlobalVarsMgr.GVars.FrameTime, GlobalVarsMgr.GVars.TotalFrameTime);
            }
        }

        public void RunEngine()
        {
            using (new HL_DevTime(1))
            {
                while (!DestructEngine)
                {
                    FrameStart();
                    FrameEnd();
                }
            }
        }
    }
}
