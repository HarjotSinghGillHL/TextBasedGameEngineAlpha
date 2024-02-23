using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine;
using TextBasedGameEngine.Engine.GameManager;
using TextBasedGameEngine.Engine.RenderSystem;
using TextBasedGameEngine.Tools;

namespace TextBasedGameEngineAlpha.Engine.RenderSystem.Frames
{
    public class HL_DeathScreen
    {
        static public bool bDiedScreenStart = false;
        static public int iDeathScreenTickCount = 0;
        static public int iOldDeathScreenTickCount = 0;
        public static void YouDiedScreen(HL_Engine Engine)
        {
            if (!bDiedScreenStart)
            {
                iDeathScreenTickCount = 0;
                HL_Console.SetCursorPos(new Vector2(HL_MapManager.vecMapRenderPositionStart.x, HL_MapManager.vecMapRenderPositionStart.y));

                decimal MiddleCount = HL_MapManager.LoadedMap.Count;
                MiddleCount /= (decimal)2.0;
                MiddleCount = Math.Ceiling(MiddleCount);

                string szDiedScreenText = "You Died! (";
                szDiedScreenText += "Respawning)";

                if (HL_HudOverlay.iLives == -1)
                    szDiedScreenText = "Game over (esc to quit)";

                decimal MiddleCountOfText = szDiedScreenText.Length;
                MiddleCountOfText /= (decimal)2.0;
                MiddleCountOfText = Math.Ceiling(MiddleCountOfText);

                for (int iCurrent = 0; iCurrent < HL_MapManager.LoadedMap.Count; iCurrent++)
                {
                    int iLengthToSubstract = 0;

                    if (iCurrent == MiddleCount)
                    {
                        iLengthToSubstract = szDiedScreenText.Length - 1;
                    }

                    decimal MiddleCountOfIndex = HL_MapManager.LoadedMap[iCurrent].Length;
                    MiddleCountOfIndex /= (decimal)2.0;
                    MiddleCountOfIndex = Math.Ceiling(MiddleCountOfIndex);
                    MiddleCountOfIndex -= Math.Ceiling(MiddleCountOfText);

                    for (int iIndex = 0; iIndex < HL_MapManager.LoadedMap[iCurrent].Length - iLengthToSubstract; iIndex++)
                    {
                        if (iLengthToSubstract != 0 && MiddleCountOfIndex == iIndex)
                            Console.Write(szDiedScreenText);
                        else
                            Console.Write(" ");
                    }

                    Console.WriteLine();
                    HL_Console.SetCursorPos(new Vector2(Console.CursorLeft + HL_MapManager.vecMapRenderPositionStart.x, Console.CursorTop));
                }

                bDiedScreenStart = true;
            }

            if (iOldDeathScreenTickCount != Environment.TickCount)
            {
                ++iDeathScreenTickCount;
                iOldDeathScreenTickCount = Environment.TickCount;
            }

            if (HL_HudOverlay.iLives == -1)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo KeyInfo = Console.ReadKey(true);

                    if (KeyInfo.Key == ConsoleKey.Escape)
                        Environment.Exit(0);

                }

                iDeathScreenTickCount = 0;
            }

            if (iDeathScreenTickCount >= 100)
            {
                //HL_MapManager.RestoreMap();
                HL_Console.SetCursorPos(new Vector2(0, 1));

                for (int iCurrent = 0; iCurrent < HL_MapManager.LoadedMap.Count; iCurrent++)
                {
                    Console.Write('\u2502');

                    for (int iIndex = 0; iIndex < HL_MapManager.LoadedMap[iCurrent].Length; iIndex++)
                    {
                        HL_RenderSystem.WriteMapCharacter(HL_MapManager.LoadedMap[iCurrent][iIndex]);
                    }

                    Console.Write('\u2502');

                    Console.WriteLine();
                }

                bDiedScreenStart = false;
                Engine.GameManager.LocalPlayer.Handle.Spawn();
                Engine.GameManager.LocalPlayer.iLocalOxygenAmount = 100;
                HL_HudOverlay.UpdateHud(Engine.GameManager.LocalPlayer.Handle.GetHealth(), Engine.GameManager.LocalPlayer.Handle.GetShield(), HL_HudOverlay.iLives, Engine.GameManager.LocalPlayer.iLocalOxygenAmount, HL_HudOverlay.iCoinsCollected, HL_HudOverlay.iScore, HL_HudOverlay.iLastEnemyHealth, HL_HudOverlay.iLastEnemyShield);
                Engine.GameManager.LocalPlayer.vecLocalPlayerPosition = Engine.GameManager.LocalPlayer.vecLocalPlayerPositionStart;

                HL_RenderSystem.SetPositionAndDrawChar(HL_MapManager.LocalPlayerChar, HL_MapManager.vecMapRenderPositionStart + Engine.GameManager.LocalPlayer.vecLocalPlayerPosition, ConsoleColor.DarkGreen);
            }
        }
    }
}
