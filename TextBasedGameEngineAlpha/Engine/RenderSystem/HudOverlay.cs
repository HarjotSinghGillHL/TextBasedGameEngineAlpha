using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Tools;

namespace TextBasedGameEngine.Engine.RenderSystem
{


    public class HL_HudOverlay
    {
        HL_Engine OwningEngine = null;

        public static int iLives = 1;
        public static int iCoinsCollected = 0;
        public static int iScore = 0;
        public static int iLastEnemyHealth = 0;
        public static int iLastEnemyShield = 0;
        public void Initialize(HL_Engine _OwningEngine)
        {
            OwningEngine = _OwningEngine;
        }

        static Vector2 vecStoredHealthBarStart;
        static Vector2 vecStoredShieldBarStart;
        static Vector2 vecStoredOxygenBarStart;
        static Vector2 vecStoredLivesStart;
        static Vector2 vecStoredCoinsStart;
        static Vector2 vecStoredScoreStart;
        static Vector2 vecEnemyStoredHealthBarStart;
        static Vector2 vecEnemyStoredShieldBarStart;
        static Vector2 vecFpsStart;
        static Vector2 vecFrametimeStart;
        static Vector2 vecRelativeFrametimeStart;
        public static void InitHud(int iLives, int iCoinsOnMap)
        {
            Console.Write("Health : ");

            vecStoredHealthBarStart = new Vector2(Console.CursorLeft, Console.CursorTop);

            for (int i = 0; i < 10; i++)
                HL_Console.SetColorAndDrawConsole("\u25A0", ConsoleColor.Red);

            Console.Write(" 100       FPS : ");

            vecFpsStart = new Vector2(Console.CursorLeft, Console.CursorTop);

            Console.Write("0");

            Console.WriteLine();
            Console.Write("Shield : ");

            vecStoredShieldBarStart = new Vector2(Console.CursorLeft, Console.CursorTop);

            for (int i = 0; i < 10; i++)
                HL_Console.SetColorAndDrawConsole("\u25A0", ConsoleColor.DarkBlue);

            Console.Write(" 100       Absolute FrameTime : ");

            vecFrametimeStart = new Vector2(Console.CursorLeft, Console.CursorTop);
           
            Console.Write("0.0");

            Console.WriteLine();
            Console.Write("Oxygen : ");

            vecStoredOxygenBarStart = new Vector2(Console.CursorLeft, Console.CursorTop);

            for (int i = 0; i < 10; i++)
                HL_Console.SetColorAndDrawConsole("\u25A0", ConsoleColor.Cyan);

            Console.Write(" 100       Relative FrameTime : ");
           
            vecRelativeFrametimeStart = new Vector2(Console.CursorLeft, Console.CursorTop);

            Console.Write("0.0");

            Console.WriteLine();
            Console.Write("Lives : ");

            vecStoredLivesStart = new Vector2(Console.CursorLeft, Console.CursorTop);

            Console.Write(iLives);

            Console.WriteLine();

            Console.Write("Coins (" + iCoinsOnMap + " on map) : ");

            vecStoredCoinsStart = new Vector2(Console.CursorLeft, Console.CursorTop);

            Console.Write(0);

            Console.WriteLine();

            Console.Write("Score : ");

            vecStoredScoreStart = new Vector2(Console.CursorLeft, Console.CursorTop);

            Console.Write(0);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("[Last Enemy State]");

            Console.Write("Health : ");

            vecEnemyStoredHealthBarStart = new Vector2(Console.CursorLeft, Console.CursorTop);

            Console.Write(" 0");

            Console.WriteLine();
            Console.Write("Shield : ");

            vecEnemyStoredShieldBarStart = new Vector2(Console.CursorLeft, Console.CursorTop);

            Console.Write(" 0");
        }
        public static void UpdateEngineState(int iFps,double dFrameTime,double dTotalFrameTime)
        {
            Console.SetCursorPosition((int)vecFpsStart.x, (int)vecFpsStart.y);
            Console.Write(iFps + "        ");

            Console.SetCursorPosition((int)vecFrametimeStart.x, (int)vecFrametimeStart.y);
            Console.Write(dFrameTime + "        ");

            Console.SetCursorPosition((int)vecRelativeFrametimeStart.x, (int)vecRelativeFrametimeStart.y);
            Console.Write(dTotalFrameTime + "        ");
            
        }

        public static void UpdateHud(int iHealth, int iShield, int iLives,int iOxygen, int iCoinsCollected, int iScore,int iLastEnemyHealth,int iLastEnemyShield)
        {
            Console.SetCursorPosition((int)vecStoredHealthBarStart.x, (int)vecStoredHealthBarStart.y);

            for (int i = 0; i < 10; i++)
                Console.Write(" ");

            Console.Write("    ");

            Console.SetCursorPosition((int)vecStoredHealthBarStart.x, (int)vecStoredHealthBarStart.y);

            for (int i = 0; i < Math.Ceiling((decimal)(iHealth / 10.0)); i++)
                HL_Console.SetColorAndDrawConsole("\u25A0", ConsoleColor.Red);

            Console.Write(" " + iHealth);

            Console.WriteLine();

            Console.Write("Shield : ");

            Console.SetCursorPosition((int)vecStoredShieldBarStart.x, (int)vecStoredShieldBarStart.y);

            for (int i = 0; i < 10; i++)
                Console.Write(" ");

            Console.Write("    ");

            Console.SetCursorPosition((int)vecStoredShieldBarStart.x, (int)vecStoredShieldBarStart.y);

            for (int i = 0; i < Math.Ceiling((decimal)(iShield / 10.0)); i++)
                HL_Console.SetColorAndDrawConsole("\u25A0", ConsoleColor.DarkBlue);

            Console.Write(" " + iShield);

            Console.WriteLine();

            Console.Write("Oxygen : ");

            Console.SetCursorPosition((int)vecStoredOxygenBarStart.x, (int)vecStoredOxygenBarStart.y);

            for (int i = 0; i < 10; i++)
                Console.Write(" ");

            Console.Write("    ");

            Console.SetCursorPosition((int)vecStoredOxygenBarStart.x, (int)vecStoredOxygenBarStart.y);

            for (int i = 0; i < Math.Ceiling((decimal)(iOxygen / 10.0)); i++)
                HL_Console.SetColorAndDrawConsole("\u25A0", ConsoleColor.Cyan);

            Console.Write(" " + iOxygen);

            Console.SetCursorPosition((int)vecStoredLivesStart.x, (int)vecStoredLivesStart.y);
            Console.Write("            ");
            Console.SetCursorPosition((int)vecStoredLivesStart.x, (int)vecStoredLivesStart.y);
            Console.Write(iLives);

            Console.SetCursorPosition((int)vecStoredCoinsStart.x, (int)vecStoredCoinsStart.y);
            Console.Write("            ");
            Console.SetCursorPosition((int)vecStoredCoinsStart.x, (int)vecStoredCoinsStart.y);
            Console.Write(iCoinsCollected);

            Console.SetCursorPosition((int)vecStoredScoreStart.x, (int)vecStoredScoreStart.y);
            Console.Write("            ");
            Console.SetCursorPosition((int)vecStoredScoreStart.x, (int)vecStoredScoreStart.y);
            Console.Write(iScore);


            Console.SetCursorPosition((int)vecEnemyStoredHealthBarStart.x, (int)vecEnemyStoredHealthBarStart.y);

            for (int i = 0; i < 10; i++)
                Console.Write(" ");

            Console.Write("    ");

            Console.SetCursorPosition((int)vecEnemyStoredHealthBarStart.x, (int)vecEnemyStoredHealthBarStart.y);

            for (int i = 0; i < Math.Ceiling((decimal)(iLastEnemyHealth / 10.0)); i++)
                HL_Console.SetColorAndDrawConsole("\u25A0", ConsoleColor.Red);

            Console.Write(" " + iLastEnemyHealth);

            Console.WriteLine();

            Console.Write("Shield : ");

            Console.SetCursorPosition((int)vecEnemyStoredShieldBarStart.x, (int)vecEnemyStoredShieldBarStart.y);

            for (int i = 0; i < 10; i++)
                Console.Write(" ");

            Console.Write("    ");

            Console.SetCursorPosition((int)vecEnemyStoredShieldBarStart.x, (int)vecEnemyStoredShieldBarStart.y);

            for (int i = 0; i < Math.Ceiling((decimal)(iLastEnemyShield / 10.0)); i++)
                HL_Console.SetColorAndDrawConsole("\u25A0", ConsoleColor.DarkBlue);

            Console.Write(" " + iLastEnemyShield);

            Console.WriteLine();
        }

    }
}
