using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine.Classes;
using TextBasedGameEngine.Engine.RenderSystem;
using TextBasedGameEngine.Tools;
using TextBasedGameEngineAlpha.Engine.GameManager;
using TextBasedGameEngineAlpha.Engine.GameManager.WorldObjects;
using TextBasedGameEngineAlpha.Engine.RenderSystem;

namespace TextBasedGameEngine.Engine.GameManager
{
    public class HL_MapManager
    {
        public static HL_Engine OwningEngine = null;

        public static List<string> LoadedMap;
        public static List<string> OriginalMapStored;
        public static Vector2 vecMapRenderPositionStart;
        public static int iOriginalCoinsCount = 0;
        public void Initialize(HL_Engine _OwningEngine)
        {
            OwningEngine = _OwningEngine;
        }

        public static char LocalPlayerChar = 'P';
        public static void RestoreMap()
        {
            LoadedMap.Clear();
            LoadedMap.AddRange(OriginalMapStored);
        }

        public static bool Load(string Text)
        {
            LoadedMap = HL_FileSystem.ReadFileLineByLine(Text);

            if (LoadedMap.Count == 0)
                return false;

            int iLongestRowLength = HL_FileSystem.GetLengthOfTheLongestRow(ref LoadedMap);

            System.Console.Write('\u250C');

            for (int iBorderChar = 0; iBorderChar < iLongestRowLength; iBorderChar++)
                System.Console.Write('\u2500');

            System.Console.Write('\u2510');

            System.Console.WriteLine();

            int iLegendPadding = 0;

            for (int iCurrent = 0; iCurrent < LoadedMap.Count; iCurrent++)
            {
                System.Console.Write('\u2502');

                for (int iIndex = 0; iIndex < LoadedMap[iCurrent].Length; iIndex++)
                {
                     if (LoadedMap[iCurrent][iIndex] == 'P' || HL_TileManager.IsEnemyTile(LoadedMap[iCurrent][iIndex]))
                         HL_RenderSystem.WriteMapCharacter(LoadedMap[iCurrent][iIndex], ConsoleColor.DarkGreen);
                      else
                        HL_RenderSystem.WriteMapCharacter(LoadedMap[iCurrent][iIndex]);
                }

                System.Console.Write('\u2502');

                DrawLegend(ref iLegendPadding);

                System.Console.WriteLine();
            }

            System.Console.Write('\u2514');

            for (int iBorderChar = 0; iBorderChar < iLongestRowLength; iBorderChar++)
                System.Console.Write('\u2500');

            System.Console.Write('\u2518');

            System.Console.WriteLine();

            if (OriginalMapStored == null)
                OriginalMapStored = new List<string>();

            OriginalMapStored.Clear();
            OriginalMapStored.AddRange(LoadedMap);

            OwningEngine.GameManager.LocalPlayer.vecLocalPlayerPositionStart = HL_WorldEntityManager.FindLocalPlayerInMap(OriginalMapStored);
            OwningEngine.GameManager.LocalPlayer.vecLocalPlayerPosition = OwningEngine.GameManager.LocalPlayer.vecLocalPlayerPositionStart;
            LoadedMap[(int)OwningEngine.GameManager.LocalPlayer.vecLocalPlayerPosition.y] = HL_Console.ChangeStringElement(LoadedMap[(int)OwningEngine.GameManager.LocalPlayer.vecLocalPlayerPosition.y], (int)OwningEngine.GameManager.LocalPlayer.vecLocalPlayerPosition.x, 'G');
            vecMapRenderPositionStart = new Vector2(1, 1);

            HL_HudOverlay.iLives = 2;
            HL_HudOverlay.iCoinsCollected = 0;

            HL_WorldEntityManager.CoinsConsumed = new List<Vector2>();
            HL_WorldEntityManager.CoinsConsumed.Clear();

            HL_WorldEntityManager.WorldEntities = HL_WorldEntityManager.GenerateMapEntityList(OriginalMapStored);
         
            HL_EventLogger.InitializeEventLogger();
            iOriginalCoinsCount = GetCountOfCoinsOnMap(LoadedMap);

            HL_HudOverlay.InitHud(HL_HudOverlay.iLives, iOriginalCoinsCount);
  

            return true;
        }

        public static int GetCountOfCoinsOnMap(List<string> Map)
        {
            int iCount = 0;
            for (int iCurrentListIndex = 0; iCurrentListIndex < Map.Count; iCurrentListIndex++)
            {
                for (int iCurrentStringIndex = 0; iCurrentStringIndex <LoadedMap[iCurrentListIndex].Count(); iCurrentStringIndex++)
                {
                    if (LoadedMap[iCurrentListIndex][iCurrentStringIndex] == 'C')
                        ++iCount;
                }
            }

            return iCount;
        }
        static void DrawLegend(ref int iLegendPadding)
        {
            if (iLegendPadding <= 12)
            {
                Console.Write(" ");

                switch (iLegendPadding)
                {
                    case 0:
                        {
                            Console.Write("=======LEGEND======");
                            break;
                        }
                    case 1:
                        {
                            Console.Write("Mountains = ");
                            HL_RenderSystem.WriteMapCharacter('M');
                            break;
                        }
                    case 2:
                        {
                            Console.Write("Water = ");
                            HL_RenderSystem.WriteMapCharacter('W');
                            break;
                        }
                    case 3:
                        {
                            Console.Write("Grass = ");
                            HL_RenderSystem.WriteMapCharacter('G');
                            break;
                        }
                    case 4:
                        {
                            Console.Write("Trees = ");
                            HL_Console.SetColorAndDrawConsole('\u00A5', ConsoleColor.Green);
                            break;
                        }
                    case 5:
                        {
                            Console.Write("Lava = ");
                            HL_RenderSystem.WriteMapCharacter('L');
                            break;
                        }
                    case 6:
                        {
                            Console.Write("Coins = ");
                            HL_Console.SetColorAndDrawConsole('O', ConsoleColor.Yellow);
                            break;
                        }
                    case 7:
                        {
                            Console.Write("Heal Pickup = ");
                            HL_Console.SetColorAndDrawConsole('+', ConsoleColor.DarkRed);
                            break;
                        }
                    case 8:
                        {
                            Console.Write("Shield Pickup = ");
                            HL_Console.SetColorAndDrawConsole('+', ConsoleColor.Blue);
                            break;
                        }
                    case 9:
                        {
                            Console.Write("Enemy = ");
                            HL_Console.SetColorAndDrawConsole('E', ConsoleColor.Gray);
                            break;
                        }
                    case 10:
                        {
                            Console.Write("Standing Enemy = ");
                            HL_Console.SetColorAndDrawConsole('K', ConsoleColor.Gray);
                            break;
                        }
                    case 11:
                        {
                            Console.Write("Runner Enemy = ");
                            HL_Console.SetColorAndDrawConsole('B', ConsoleColor.Gray);
                            break;
                        }
                    case 12:
                        {
                            Console.Write("===================");
                            break;
                        }
                    default:
                        {
                            break;
                        }


                }

                iLegendPadding += 1;
            }

        }

    }
}
