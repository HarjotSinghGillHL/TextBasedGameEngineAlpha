using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine;
using TextBasedGameEngine.Engine.Classes;
using TextBasedGameEngine.Engine.GameManager;
using TextBasedGameEngine.Engine.RenderSystem;
using TextBasedGameEngine.Tools;
using TextBasedGameEngineAlpha.Engine.GameManager.WorldObjects;

namespace TextBasedGameEngineAlpha.Engine.GameManager
{
    public class HL_EntityHandler
    {
        public HL_Engine Engine = null;
        public void Initialize(HL_Engine _Engine)
        {
            Engine = _Engine;
        }
        public static bool IsCoinConsumed(Vector2 vecPosition)
        {
            if (HL_WorldEntityManager.CoinsConsumed.Count == 0)
                return false;

            for (int iCurrent = 0; iCurrent < HL_WorldEntityManager.CoinsConsumed.Count; iCurrent++)
            {
                if (HL_WorldEntityManager.CoinsConsumed[iCurrent] == vecPosition)
                    return true;
            }

            return false;
        }
        public void HandleEntities()
        {
            if (!Engine.GameManager.LocalPlayer.Handle.IsAlive())
                return;

            if (HL_WorldEntityManager.WorldEntities.Count == 0)
                return;

            for (int iCurrent = 0; iCurrent < HL_WorldEntityManager.WorldEntities.Count; iCurrent++)
            {
                HL_MapEntity MapEntity = HL_WorldEntityManager.WorldEntities[iCurrent];

                if (MapEntity.ShouldMove())
                {
                    // Here i am predicting the next position so the entity doesnt move but we can still check if it hit the local player or not

                    Vector2 vecPredictedPosition = MapEntity.PredictPosition(Engine.GameManager.LocalPlayer.vecLocalPlayerPosition, HL_MapManager.LoadedMap[(int)MapEntity.vecPositionOnMap.y][HL_Math.Clamp(MapEntity.vecPositionOnMap.x - 1, 0, HL_MapManager.LoadedMap[MapEntity.vecPositionOnMap.y].Length - 1)], HL_MapManager.LoadedMap[MapEntity.vecPositionOnMap.y][HL_Math.Clamp(MapEntity.vecPositionOnMap.x + 1, 0, HL_MapManager.LoadedMap[MapEntity.vecPositionOnMap.y].Length - 1)]
                        , HL_MapManager.LoadedMap[(int)MapEntity.vecPositionOnMap.y].Length);

                    if (vecPredictedPosition == Engine.GameManager.LocalPlayer.vecLocalPlayerPosition)
                    {
                        if (HL_DamageHandler.HurtEntity(50, Engine.GameManager.LocalPlayer.Handle))
                        {
                            HL_HudOverlay.iLives -= 1;

                            if (HL_HudOverlay.iLives != -1)
                            {
                                HL_RenderSystem.SetPositionAndDrawChar(HL_MapManager.LoadedMap[(int)Engine.GameManager.LocalPlayer.vecLocalPlayerPosition.y][(int)Engine.GameManager.LocalPlayer.vecLocalPlayerPosition.x], HL_MapManager.vecMapRenderPositionStart + Engine.GameManager.LocalPlayer.vecLocalPlayerPosition);
                                Engine.GameManager.LocalPlayer.vecLocalPlayerPositionStart = HL_WorldEntityManager.FindLocalPlayerInMap(HL_MapManager.OriginalMapStored);
                                Engine.GameManager.LocalPlayer.vecLocalPlayerPosition = Engine.GameManager.LocalPlayer.vecLocalPlayerPositionStart;
                             //  Engine.GameManager.LocalPlayer.Handle.Spawn();
                                HL_RenderSystem.SetPositionAndDrawChar(HL_MapManager.LocalPlayerChar, HL_MapManager.vecMapRenderPositionStart + Engine.GameManager.LocalPlayer.vecLocalPlayerPosition, ConsoleColor.DarkGreen);
                                HL_HudOverlay.iScore -= 300;
                            }
                        }
               
                        HL_HudOverlay.UpdateHud(Engine.GameManager.LocalPlayer.Handle.GetHealth(), Engine.GameManager.LocalPlayer.Handle.GetShield(), HL_HudOverlay.iLives, Engine.GameManager.LocalPlayer.iLocalOxygenAmount, HL_HudOverlay.iCoinsCollected, HL_HudOverlay.iScore, HL_HudOverlay.iLastEnemyHealth, HL_HudOverlay.iLastEnemyShield);
                    }
                    else
                    {
       
                        char ReplacementChar = HL_MapManager.OriginalMapStored[(int)MapEntity.vecPositionOnMap.y][(int)MapEntity.vecPositionOnMap.x];

                       if (HL_TileManager.IsEnemyTile(ReplacementChar) || (ReplacementChar == 'C' && IsCoinConsumed(MapEntity.vecPositionOnMap)))
                            ReplacementChar = 'G';

                        HL_MapManager.LoadedMap[(int)MapEntity.vecPositionOnMap.y] = HL_Console.ChangeStringElement(HL_MapManager.LoadedMap[(int)MapEntity.vecPositionOnMap.y], (int)MapEntity.vecPositionOnMap.x, ReplacementChar);

                        HL_RenderSystem.SetPositionAndDrawChar(HL_MapManager.LoadedMap, HL_MapManager.vecMapRenderPositionStart, MapEntity.vecPositionOnMap);

                        MapEntity.UpdatePosition(Engine, Engine.GameManager.LocalPlayer.vecLocalPlayerPosition, HL_MapManager.LoadedMap[(int)MapEntity.vecPositionOnMap.y][HL_Math.Clamp(MapEntity.vecPositionOnMap.x - 1, 0, HL_MapManager.LoadedMap[MapEntity.vecPositionOnMap.y].Length - 1)], HL_MapManager.LoadedMap[MapEntity.vecPositionOnMap.y][HL_Math.Clamp(MapEntity.vecPositionOnMap.x + 1, 0, HL_MapManager.LoadedMap[MapEntity.vecPositionOnMap.y].Length - 1)]
                            , HL_MapManager.LoadedMap[(int)MapEntity.vecPositionOnMap.y].Length);

                        HL_MapManager.LoadedMap[(int)MapEntity.vecPositionOnMap.y] = HL_Console.ChangeStringElement(HL_MapManager.LoadedMap[(int)MapEntity.vecPositionOnMap.y], (int)MapEntity.vecPositionOnMap.x, MapEntity.BaseChar);
                        HL_RenderSystem.SetPositionAndDrawChar(HL_MapManager.LoadedMap, HL_MapManager.vecMapRenderPositionStart, MapEntity.vecPositionOnMap, HL_RenderSystem.GetBkgColorBasedOfTile(HL_MapManager.OriginalMapStored[MapEntity.vecPositionOnMap.y][MapEntity.vecPositionOnMap.x]));
                    }
                }
            }
        }
    }
}
