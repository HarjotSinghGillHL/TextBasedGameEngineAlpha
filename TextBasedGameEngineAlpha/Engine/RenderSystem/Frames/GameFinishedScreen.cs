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
    public class HL_GameFinishedScreen
    {
        static public bool bGameFInishedScreen = false;
        public static void FinishedScreen(HL_Engine Engine)
        {
            if (!bGameFInishedScreen)
            {
                HL_Console.SetCursorPos(new Vector2(HL_MapManager.vecMapRenderPositionStart.x, HL_MapManager.vecMapRenderPositionStart.y));

                decimal MiddleCount = HL_MapManager.LoadedMap.Count;
                MiddleCount /= (decimal)2.0;
                MiddleCount = Math.Ceiling(MiddleCount);

                string szFinishText = "Game Finished! (esc to quit)";

                decimal MiddleCountOfText = szFinishText.Length;
                MiddleCountOfText /= (decimal)2.0;
                MiddleCountOfText = Math.Ceiling(MiddleCountOfText);

                for (int iCurrent = 0; iCurrent < HL_MapManager.LoadedMap.Count; iCurrent++)
                {
                    int iLengthToSubstract = 0;

                    if (iCurrent == MiddleCount)
                    {
                        iLengthToSubstract = szFinishText.Length - 1;
                    }

                    decimal MiddleCountOfIndex = HL_MapManager.LoadedMap[iCurrent].Length;
                    MiddleCountOfIndex /= (decimal)2.0;
                    MiddleCountOfIndex = Math.Ceiling(MiddleCountOfIndex);
                    MiddleCountOfIndex -= Math.Ceiling(MiddleCountOfText);

                    for (int iIndex = 0; iIndex < HL_MapManager.LoadedMap[iCurrent].Length - iLengthToSubstract; iIndex++)
                    {
                        if (iLengthToSubstract != 0 && MiddleCountOfIndex == iIndex)
                            Console.Write(szFinishText);
                        else
                            Console.Write(" ");
                    }

                    Console.WriteLine();
                    HL_Console.SetCursorPos(new Vector2(Console.CursorLeft + HL_MapManager.vecMapRenderPositionStart.x, Console.CursorTop));
                }

                bGameFInishedScreen = true;
            }
        }
    }
}
