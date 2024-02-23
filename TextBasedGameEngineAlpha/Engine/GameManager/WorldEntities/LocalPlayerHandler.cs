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
using TextBasedGameEngineAlpha.Engine.RenderSystem.Frames;

namespace TextBasedGameEngineAlpha.Engine.GameManager.WorldEntities
{
    public class HL_LocalPlayerHandler
    {
        public HL_BaseEntity Handle;

        public HL_WaitState HurtState;
        public HL_WaitState OxygenState;

        public int iLocalOxygenAmount = 0;

        public Vector2 vecLocalPlayerPositionStart;
        public Vector2 vecLocalPlayerPosition;

        public HL_LocalPlayerHandler()
        {
            Handle = new HL_BaseEntity("LocalPlayer");
            iLocalOxygenAmount = 100;
        }

        public void HandleLocalPlayer(HL_Engine Engine)
        {
        LABEL_HANDLE_LOCAL_START:

            if (!Handle.IsAlive())
            {
                HL_DeathScreen.YouDiedScreen(Engine);
                return;
            }

            HL_DeathScreen.bDiedScreenStart = false;

            char OldMapChar = HL_MapManager.LoadedMap[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x];

            PredictedMovement moveData = Engine.GameManager.GameMovement.PredictMovement(Engine.GameManager.LocalPlayer.vecLocalPlayerPosition);
            Engine.GameManager.GameMovement.PushPredictedMovementDataToScreen(moveData);

            if (moveData.bNextPositionIsEnemy)
            {
                HL_MapEntity EntityInfo = HL_WorldEntityManager.GetEntityFromListByPosition(ref HL_WorldEntityManager.WorldEntities, moveData.vecPredictedPosition);

                HL_HudOverlay.iLastEnemyHealth = EntityInfo.Handle.GetHealth();
                HL_HudOverlay.iLastEnemyShield = EntityInfo.Handle.GetShield();
                HL_HudOverlay.UpdateHud(Engine.GameManager.LocalPlayer.Handle.GetHealth(), Engine.GameManager.LocalPlayer.Handle.GetShield(), HL_HudOverlay.iLives, Engine.GameManager.LocalPlayer.iLocalOxygenAmount, HL_HudOverlay.iCoinsCollected, HL_HudOverlay.iScore, HL_HudOverlay.iLastEnemyHealth, HL_HudOverlay.iLastEnemyShield);

                if (HL_DamageHandler.HurtEntity(67, EntityInfo.Handle))
                {
                    HL_HudOverlay.iLastEnemyHealth = EntityInfo.Handle.GetHealth();
                    HL_HudOverlay.iLastEnemyShield = EntityInfo.Handle.GetShield();
                    char ReplacementChar = HL_MapManager.OriginalMapStored[(int)moveData.vecPredictedPosition.y][(int)moveData.vecPredictedPosition.x];

                    if (HL_TileManager.IsEnemyTile(ReplacementChar) || (ReplacementChar == 'C' && HL_EntityHandler.IsCoinConsumed(moveData.vecPredictedPosition)))
                        ReplacementChar = 'G';

                    HL_HudOverlay.iScore += 500;
                    HL_WorldEntityManager.RemoveEntityFromList(ref HL_WorldEntityManager.WorldEntities, moveData.vecPredictedPosition);
                    HL_MapManager.LoadedMap[(int)moveData.vecPredictedPosition.y] = HL_Console.ChangeStringElement(HL_MapManager.LoadedMap[(int)moveData.vecPredictedPosition.y], (int)moveData.vecPredictedPosition.x, ReplacementChar);
                    HL_RenderSystem.SetPositionAndDrawChar(ReplacementChar, HL_MapManager.vecMapRenderPositionStart + moveData.vecPredictedPosition, ConsoleColor.DarkGreen);
                    HL_HudOverlay.UpdateHud(Engine.GameManager.LocalPlayer.Handle.GetHealth(), Engine.GameManager.LocalPlayer.Handle.GetShield(), HL_HudOverlay.iLives, Engine.GameManager.LocalPlayer.iLocalOxygenAmount, HL_HudOverlay.iCoinsCollected, HL_HudOverlay.iScore, HL_HudOverlay.iLastEnemyHealth, HL_HudOverlay.iLastEnemyShield);

                }
            }

            if (moveData.bPositionChange && HL_MapManager.LoadedMap[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x] == 'X')
            {
                //Checkpoint
            }
                

            if (HL_MapManager.LoadedMap[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x] == 'C')
            {
                HL_MapManager.LoadedMap[(int)vecLocalPlayerPosition.y] = HL_Console.ChangeStringElement(HL_MapManager.LoadedMap[(int)vecLocalPlayerPosition.y], (int)vecLocalPlayerPosition.x, 'G');
                HL_HudOverlay.iCoinsCollected++;
                HL_HudOverlay.iScore += 100;
                HL_HudOverlay.UpdateHud(Handle.GetHealth(), Handle.GetShield(), HL_HudOverlay.iLives, Engine.GameManager.LocalPlayer.iLocalOxygenAmount, HL_HudOverlay.iCoinsCollected, HL_HudOverlay.iScore, HL_HudOverlay.iLastEnemyHealth, HL_HudOverlay.iLastEnemyShield);
                HL_WorldEntityManager.CoinsConsumed.Add(vecLocalPlayerPosition);
            }

            if (HL_MapManager.LoadedMap[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x] == 'H')
            {
                if (HL_DamageHandler.HealEntity(50, Handle))
                {
                    HL_MapManager.LoadedMap[(int)vecLocalPlayerPosition.y] = HL_Console.ChangeStringElement(HL_MapManager.LoadedMap[(int)vecLocalPlayerPosition.y], (int)vecLocalPlayerPosition.x, 'G');
                    HL_HudOverlay.UpdateHud(Handle.GetHealth(), Handle.GetShield(), HL_HudOverlay.iLives, Engine.GameManager.LocalPlayer.iLocalOxygenAmount, HL_HudOverlay.iCoinsCollected, HL_HudOverlay.iScore, HL_HudOverlay.iLastEnemyHealth, HL_HudOverlay.iLastEnemyShield);
                }
            }

            if (HL_MapManager.LoadedMap[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x] == 'S')
            {
                if (HL_DamageHandler.GainShield(50, Handle))
                {
                    HL_MapManager.LoadedMap[(int)vecLocalPlayerPosition.y] = HL_Console.ChangeStringElement(HL_MapManager.LoadedMap[(int)vecLocalPlayerPosition.y], (int)vecLocalPlayerPosition.x, 'G');
                    HL_HudOverlay.UpdateHud(Handle.GetHealth(), Handle.GetShield(), HL_HudOverlay.iLives, Engine.GameManager.LocalPlayer.iLocalOxygenAmount, HL_HudOverlay.iCoinsCollected, HL_HudOverlay.iScore, HL_HudOverlay.iLastEnemyHealth, HL_HudOverlay.iLastEnemyShield);
                }
            }

            if (HL_MapManager.LoadedMap[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x] == 'W')
            {
                if (!HL_TileManager.bEnteredWater)
                {
                    OxygenState = new HL_WaitState(35);
                    HL_TileManager.bEnteredWater = true;
                }
                else
                {

                    if (OxygenState.ShouldUpdate())
                    {
                        if (iLocalOxygenAmount > 0)
                        {
                            iLocalOxygenAmount -= 20;
                        }
                        else
                        {
                            HL_HudOverlay.iScore -= 25;
                            if (HL_DamageHandler.HurtEntity(20, Handle, false))
                            {
                                iLocalOxygenAmount = 100;
                            }
                        }

                        HL_HudOverlay.UpdateHud(Handle.GetHealth(), Handle.GetShield(), HL_HudOverlay.iLives, Engine.GameManager.LocalPlayer.iLocalOxygenAmount, HL_HudOverlay.iCoinsCollected, HL_HudOverlay.iScore, HL_HudOverlay.iLastEnemyHealth, HL_HudOverlay.iLastEnemyShield);
                    }
                }
            }
            else
            {
                if (HL_TileManager.bEnteredWater)
                {
                    OxygenState = new HL_WaitState(30);
                    HL_TileManager.bEnteredWater = false;
                }
                else
                {
                    if (iLocalOxygenAmount < 100)
                    {
                        if (OxygenState.ShouldUpdate())
                        {
                            iLocalOxygenAmount += 20;
                            HL_HudOverlay.UpdateHud(Handle.GetHealth(), Handle.GetShield(), HL_HudOverlay.iLives, Engine.GameManager.LocalPlayer.iLocalOxygenAmount, HL_HudOverlay.iCoinsCollected, HL_HudOverlay.iScore, HL_HudOverlay.iLastEnemyHealth, HL_HudOverlay.iLastEnemyShield);
                        }
                    }
                }
           
            }

            if (HL_MapManager.LoadedMap[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x] == 'L')
            {
                if (!HL_TileManager.bEnteredLava)
                {
                    HurtState = new HL_WaitState(40);
                    HL_TileManager.bEnteredLava = true;
                }
                else
                {
                    if (HurtState.ShouldUpdate())
                    {
                       HL_HudOverlay.iScore -= 50;
                      
                        HL_DamageHandler.HurtEntity(25, Handle);
                        HL_HudOverlay.UpdateHud(Handle.GetHealth(), Handle.GetShield(), HL_HudOverlay.iLives, Engine.GameManager.LocalPlayer.iLocalOxygenAmount, HL_HudOverlay.iCoinsCollected, HL_HudOverlay.iScore, HL_HudOverlay.iLastEnemyHealth, HL_HudOverlay.iLastEnemyShield);

                        if (!Handle.IsAlive())
                        {
                            HL_HudOverlay.iLives -= 1;
                            if (HL_HudOverlay.iLives == -1)
                                goto LABEL_HANDLE_LOCAL_START;
                            else
                            {
                                HL_RenderSystem.SetPositionAndDrawChar(HL_MapManager.LoadedMap[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x], HL_MapManager.vecMapRenderPositionStart + vecLocalPlayerPosition);
                                vecLocalPlayerPosition = vecLocalPlayerPositionStart;
                              // Handle.Spawn();
                                HL_RenderSystem.SetPositionAndDrawChar(HL_MapManager.LocalPlayerChar, HL_MapManager.vecMapRenderPositionStart + vecLocalPlayerPosition, ConsoleColor.DarkGreen);
                                HL_HudOverlay.UpdateHud(Handle.GetHealth(), Handle.GetShield(), HL_HudOverlay.iLives, Engine.GameManager.LocalPlayer.iLocalOxygenAmount, HL_HudOverlay.iCoinsCollected, HL_HudOverlay.iScore, HL_HudOverlay.iLastEnemyHealth, HL_HudOverlay.iLastEnemyShield);

                            }
                        }
                    }
                }
            }
            else
            {
               HL_TileManager.bEnteredLava = false;
            }
        }

    }
}
