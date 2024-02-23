using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine;
using TextBasedGameEngine.Engine.GameManager;
using TextBasedGameEngine.Engine.RenderSystem;
using TextBasedGameEngine.Tools;
using TextBasedGameEngineAlpha.Engine.GameManager.WorldObjects;
public struct PredictedMovement
{
    public bool bNextPositionIsEnemy;
    public Vector2 vecPredictedPosition;
    public Vector2 vecOldPosition;
    public bool bPositionChange;
}

namespace TextBasedGameEngineAlpha.Engine.GameManager
{
    public class HL_GameMovement
    {
        public HL_Engine Engine = null;
        public void Initialize(HL_Engine _Engine)
        {
            Engine = _Engine;
        }

        public void PushPredictedMovementDataToScreen(PredictedMovement moveData)
        {
            if (!moveData.bPositionChange)
                return;
            
            if (!HL_TileManager.IsBlockedRegionChar(HL_MapManager.LoadedMap[(int)moveData.vecPredictedPosition.y][(int)moveData.vecPredictedPosition.x]))
            {
                HL_RenderSystem.SetPositionAndDrawChar(HL_MapManager.LoadedMap, HL_MapManager.vecMapRenderPositionStart, moveData.vecOldPosition);

                Engine.GameManager.LocalPlayer.vecLocalPlayerPosition = new Vector2(moveData.vecPredictedPosition.x, moveData.vecPredictedPosition.y);
              
                HL_RenderSystem.SetPositionAndDrawChar(HL_MapManager.LocalPlayerChar, HL_MapManager.vecMapRenderPositionStart + Engine.GameManager.LocalPlayer.vecLocalPlayerPosition, HL_RenderSystem.GetBkgColorBasedOfTile(HL_MapManager.OriginalMapStored[(int)Engine.GameManager.LocalPlayer.vecLocalPlayerPosition.y][(int)Engine.GameManager.LocalPlayer.vecLocalPlayerPosition.x]));
            }
        }
        public PredictedMovement PredictMovement(Vector2 vecLocalPosition)
        {
            PredictedMovement moveData = new PredictedMovement();

            moveData.vecOldPosition = new Vector2(vecLocalPosition.x, vecLocalPosition.y);
            moveData.vecPredictedPosition = new Vector2(moveData.vecOldPosition.x, moveData.vecOldPosition.y);

            bool bMoveRequested = false;

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo KeyInfo = Console.ReadKey(true);

                switch (KeyInfo.Key)
                {
                    case ConsoleKey.W:
                        {
                            if (moveData.vecOldPosition.y != 0)
                            {
                                moveData.vecPredictedPosition.y -= 1;
                                bMoveRequested = true;
                            }
                            break;
                        }
                    case ConsoleKey.A:
                        {
                            if (moveData.vecOldPosition.x != 0)
                            {
                                moveData.vecPredictedPosition.x -= 1;
                                bMoveRequested = true;
                            }

                            break;
                        }
                    case ConsoleKey.S:
                        {

                            if (moveData.vecOldPosition.y < HL_MapManager.LoadedMap.Count - 1)
                            {
                                moveData.vecPredictedPosition.y += 1;
                                bMoveRequested = true;

                            }

                            break;
                        }
                    case ConsoleKey.D:
                        {
                            if (moveData.vecOldPosition.x < HL_MapManager.LoadedMap[(int)moveData.vecOldPosition.y].Length - 1)
                            {
                                moveData.vecPredictedPosition.x += 1;
                                bMoveRequested = true;
                            }

                            break;
                        }
                    default:
                        break;
                }
            }

            if (bMoveRequested)
            {
                if (HL_TileManager.IsEnemyTile(HL_MapManager.LoadedMap[(int)moveData.vecPredictedPosition.y][(int)moveData.vecPredictedPosition.x]) && bMoveRequested)
                    moveData.bNextPositionIsEnemy = true;
                else
                    moveData.bPositionChange = true;
            }

            return moveData;
        }

    }
}
