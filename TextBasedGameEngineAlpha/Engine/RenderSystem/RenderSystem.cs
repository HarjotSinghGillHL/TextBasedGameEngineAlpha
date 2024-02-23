using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine.GameManager;
using TextBasedGameEngine.Tools;
using TextBasedGameEngineAlpha.Engine.GameManager.WorldObjects;

namespace TextBasedGameEngine.Engine.RenderSystem
{
    public class HL_RenderSystem
    {
        HL_HudOverlay HudOverlay = null;
        HL_UserInterface UserInterface = null;
        HL_Engine OwningEngine = null;
        public void Initialize(HL_Engine _OwningEngine)
        {
            HudOverlay = new HL_HudOverlay();
            OwningEngine = _OwningEngine;
            HudOverlay.Initialize(_OwningEngine);
        }
        public static void WriteMapCharacter(char MapChar, ConsoleColor BkgColor = ConsoleColor.Black)
        {

            switch (MapChar)
            {
                case 'E':  // Enemy 
                    {
                        HL_Console.SetColorAndDrawConsole('E', ConsoleColor.Gray, BkgColor);
                        return;
                    }
                case 'B':  // Big Enemy 
                    {
                        HL_Console.SetColorAndDrawConsole('B', ConsoleColor.Gray, BkgColor);
                        return;
                    }
                case 'K':  // Motionless Enemy 
                    {
                        HL_Console.SetColorAndDrawConsole('K', ConsoleColor.Gray, BkgColor);
                        return;
                    }
                case 'P':  // local player
                    {
                        HL_Console.SetColorAndDrawConsole('P', ConsoleColor.Gray, BkgColor);
                        return;
                    }
                case 'M':  // mountains
                    {
                        HL_Console.SetColorAndDrawConsole(' ', ConsoleColor.DarkGray, ConsoleColor.DarkGray);
                        return;
                    }
                case 'W': // water
                    {
                        HL_Console.SetColorAndDrawConsole(' ', ConsoleColor.Blue, ConsoleColor.Blue);
                        return;
                    }
                case 'G':   // grass
                    {
                        HL_Console.SetColorAndDrawConsole(' ', ConsoleColor.DarkGreen, ConsoleColor.DarkGreen);
                        break;
                    }
                case 'T':  // trees
                    {
                        HL_Console.SetColorAndDrawConsole('\u00A5', ConsoleColor.Green, ConsoleColor.DarkGreen);
                        return;
                    }
                case 'L':   // lava
                    {
                        HL_Console.SetColorAndDrawConsole('\u2592', ConsoleColor.Red, ConsoleColor.DarkRed);
                        return;
                    }
                case 'C':   // coins
                    {
                         HL_Console.SetColorAndDrawConsole('O', ConsoleColor.Yellow, ConsoleColor.DarkGreen);
                        return;
                    }
                case 'H':   // health pickup
                    {
                        HL_Console.SetColorAndDrawConsole('+', ConsoleColor.DarkRed, ConsoleColor.DarkGreen);
                        // HL_Console.SetColorAndDrawConsole('\u2665', ConsoleColor.DarkRed, ConsoleColor.DarkGreen);
                        return;
                    }
                case 'S':   // shield pickup
                    {
                        HL_Console.SetColorAndDrawConsole('+', ConsoleColor.Blue, ConsoleColor.DarkGreen);
                        //  HL_Console.SetColorAndDrawConsole('\u2665', ConsoleColor.Blue, ConsoleColor.DarkGreen);

                        return;
                    }
                default:
                    {
               
                        break;
                    }

            }

        }
        public static ConsoleColor GetBkgColorBasedOfTile(char Tile)
        {
            switch (Tile)
            {

                case 'M':
                    {
                        return ConsoleColor.DarkGray;
                    }
                case 'W':
                    {
                        return ConsoleColor.Blue;
                    }

                case 'L':
                    {
                        return ConsoleColor.DarkRed;
                    }
                case 'P':
                case 'G':
                case 'C':
                case 'B':
                case 'K':
                case 'E':
                case 'H':
                case 'S':
                    {
                        return ConsoleColor.DarkGreen;
                    }
                default:
                    {
                        return Console.BackgroundColor;
                    }

            }
        }
        public static void SetPositionAndDrawChar(List<string> Map, Vector2 vecRenderPositionStart, Vector2 vecElementPosition, ConsoleColor BkgColor = ConsoleColor.Black)
        {
            HL_Console.SetCursorPos(vecRenderPositionStart + vecElementPosition);

            if (Map[(int)vecElementPosition.y][(int)vecElementPosition.x] == HL_MapManager.LocalPlayerChar)
                HL_Console.SetColorAndDrawConsole(' ', ConsoleColor.White, BkgColor);
            else
                WriteMapCharacter(Map[(int)vecElementPosition.y][(int)vecElementPosition.x], BkgColor);
        }

        public static void SetPositionAndDrawChar(char Char, Vector2 vecPosition, ConsoleColor BkgColor = ConsoleColor.Black)
        {
            HL_Console.SetCursorPos(vecPosition);
            WriteMapCharacter(Char, BkgColor);
        }

    }
}
