using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine.GameManager;
using TextBasedGameEngine.Engine.RenderSystem;
using TextBasedGameEngine.Tools;
using TextBasedGameEngineAlpha.Engine.GameManager.WorldObjects;

public enum ELifeState
{
    STATE_NULL,
    STATE_ALIVE,
    STATE_DEAD,
    STATE_SPAWNING,
    STATE_DYING,
    STATE_SPECTATOR,
    STATE_MAX,
}
public enum EEntityClass
{
    ENTITY_PLAYER,
    ENTITY_WEAPON,
    ENTITY_PROP,
    ENTITY_NPC,
    ENTITY_MAX
}

namespace TextBasedGameEngine.Engine.Classes
{
    public class HL_BaseEntityHeader
    {
        public EEntityClass eEntityClass;
        public int iEntityIndex = 0; //not implemented
    };

    public class HL_BaseEntity : HL_BaseEntityHeader
    {

        public void Spawn()
        {
            iHealth = iMaxHealth;
            iShield = iMaxShield;
            eLifeState = ELifeState.STATE_ALIVE;
        }
        public void KillPlayer()
        {
            eLifeState = ELifeState.STATE_DEAD;
        }
        public bool IsAlive()
        {
            return eLifeState == ELifeState.STATE_ALIVE;
        }
        public int GetHealth()
        {
            return iHealth;
        }
        public void SetHealth(int iNewHealth)
        {
            iHealth = iNewHealth;
        }
        public int GetMaxHealth()
        {
            return iMaxHealth;
        }
        public int GetMinHealth()
        {
            return iMinHealth;
        }
        public int GetShield()
        {
            return iShield;
        }
        public void SetShield(int iNewShield)
        {
            iShield = iNewShield;
        }
        public int GetMaxShield()
        {
            return iMaxShield;
        }
        public int GetMinShield()
        {
            return iMinShield;
        }

        public  int iMaxHealth = 100;
        public  int iMinHealth = 0;
        public  int iHealth = 100;
        public  int iMaxShield = 100;
        public int iMinShield = 0;
        public int iShield = 100;
        public ELifeState eLifeState = ELifeState.STATE_ALIVE;
        public string szSanitizedName;
    }

    public class HL_MapEntity
    {
        public HL_MapEntity(HL_BaseEntity _Entity, Vector2 _vecPositionOnMap, char _BaseChar)
        {
            Handle = _Entity;
            vecPositionOnMap = _vecPositionOnMap;
            vecPathToRestoreFromAttackAnimation = new Vector2();
            // Random random = new Random();

            iPlayerMoveTickWait = 25;
            iLastPlayerMoveTick = 0;
            BaseChar = _BaseChar;
        }
        public bool ShouldMove()
        {
            //already running inside OnTick so i removed TickCount check here

            if (iLastPlayerMoveTick == 0)
            {
                iLastPlayerMoveTick = iPlayerMoveTickWait;
                return true;
            }

            --iLastPlayerMoveTick;

            return false;
        }

        public Vector2 PredictPosition(Vector2 vecLocalPlayerPosition, char LeftTile, char RightTile, int iLengthOfCurrentLine)
        {

            Vector2 _vecPositionOnMap = new Vector2(vecPositionOnMap.x, vecPositionOnMap.y);
            Vector2 _vecPathToRestoreFromAttackAnimation = new Vector2(vecPathToRestoreFromAttackAnimation.x, vecPathToRestoreFromAttackAnimation.y);

            Vector2 vecDirection = _vecPositionOnMap.GetDifference(vecLocalPlayerPosition);

            double DistanceFromLocalPlayer = vecLocalPlayerPosition.DistTo(_vecPositionOnMap);

            //   HL_Console.SetCursorPos(new Vector2(0,21));

            //check if the distance from local player is < 5 units so it attacks local player otherwise keep the patrol animation active

            if (DistanceFromLocalPlayer < 5)
            {
                Vector2 vecNextPositionOnMap = new Vector2(_vecPositionOnMap.x, _vecPositionOnMap.y);

                if (BaseChar == 'B')
                {
                    vecNextPositionOnMap.x -= HL_Math.Clamp((int)vecDirection.x, -1, 1);
                    vecNextPositionOnMap.y -= HL_Math.Clamp((int)vecDirection.y, -1, 1);
                }
                else
                {
                    vecNextPositionOnMap.x += HL_Math.Clamp((int)vecDirection.x, -1, 1);
                    vecNextPositionOnMap.y += HL_Math.Clamp((int)vecDirection.y, -1, 1);
                }

                if (vecNextPositionOnMap.x >= iLengthOfCurrentLine && vecNextPositionOnMap.y < HL_MapManager.LoadedMap.Count)
                {
                    vecNextPositionOnMap.x -= 1;
                    vecDirection.x = 0;
                }

                if (vecNextPositionOnMap.y >= HL_MapManager.LoadedMap.Count)
                {
                    vecNextPositionOnMap.y -= 1;
                    vecDirection.y = 0;
                }

                if (vecNextPositionOnMap.x  >= 0 && vecNextPositionOnMap.x < iLengthOfCurrentLine && vecNextPositionOnMap.y >= 0 && vecNextPositionOnMap.y < HL_MapManager.LoadedMap.Count)
                {
                    if (!HL_TileManager.IsBlockedRegionChar(HL_MapManager.LoadedMap[vecNextPositionOnMap.y][vecNextPositionOnMap.x]) && !HL_TileManager.IsTireHarmful(HL_MapManager.LoadedMap[vecNextPositionOnMap.y][vecNextPositionOnMap.x])
                    && !HL_TileManager.IsEnemyTile(HL_MapManager.LoadedMap[vecNextPositionOnMap.y][vecNextPositionOnMap.x]))
                    {
                        if (BaseChar == 'B')
                        {
                            _vecPathToRestoreFromAttackAnimation.x -= HL_Math.Clamp((int)vecDirection.x, -1, 1);
                            _vecPathToRestoreFromAttackAnimation.y -= HL_Math.Clamp((int)vecDirection.y, -1, 1);
                        }
                        else
                        {
                            _vecPathToRestoreFromAttackAnimation.x += HL_Math.Clamp((int)vecDirection.x, -1, 1);
                            _vecPathToRestoreFromAttackAnimation.y += HL_Math.Clamp((int)vecDirection.y, -1, 1);
                        }
                        _vecPositionOnMap.x = vecNextPositionOnMap.x;
                        _vecPositionOnMap.y = vecNextPositionOnMap.y;
                    }
                }
            }
            else if ((_vecPathToRestoreFromAttackAnimation.x != 0 || _vecPathToRestoreFromAttackAnimation.y != 0))
            {
                if (DistanceFromLocalPlayer > 6)
                {
                    Vector2 vecNextPositionOnMap = new Vector2(_vecPositionOnMap.x, _vecPositionOnMap.y);

                    if (BaseChar == 'B')
                    {
                        vecNextPositionOnMap.x -= HL_Math.Clamp((int)_vecPathToRestoreFromAttackAnimation.x, -1, 1);
                        vecNextPositionOnMap.y -= HL_Math.Clamp((int)_vecPathToRestoreFromAttackAnimation.y, -1, 1);
                    }
                    else
                    {
                        vecNextPositionOnMap.x -= HL_Math.Clamp((int)_vecPathToRestoreFromAttackAnimation.x, -1, 1);
                        vecNextPositionOnMap.y -= HL_Math.Clamp((int)_vecPathToRestoreFromAttackAnimation.y, -1, 1);
                    }

                    if (!HL_TileManager.IsBlockedRegionChar(HL_MapManager.LoadedMap[vecNextPositionOnMap.y][vecNextPositionOnMap.x]) && !HL_TileManager.IsTireHarmful(HL_MapManager.LoadedMap[vecNextPositionOnMap.y][vecNextPositionOnMap.x])
                        && !HL_TileManager.IsEnemyTile(HL_MapManager.LoadedMap[vecNextPositionOnMap.y][vecNextPositionOnMap.x]))
                    {
                        _vecPositionOnMap.x = vecNextPositionOnMap.x;
                        _vecPositionOnMap.y = vecNextPositionOnMap.y;
                    }
                    else
                    {
                        // I have to implement a path finder mechanism yet which will find a shortest path back to patrol position avoiding obstacles
                        _vecPathToRestoreFromAttackAnimation.x = 0;
                        _vecPathToRestoreFromAttackAnimation.y = 0;
                    }

                    if (_vecPathToRestoreFromAttackAnimation.x != 0)
                    {
                        if (_vecPathToRestoreFromAttackAnimation.x < 0)
                            _vecPathToRestoreFromAttackAnimation.x += 1;
                        else
                            _vecPathToRestoreFromAttackAnimation.x -= 1;
                    }

                    if (_vecPathToRestoreFromAttackAnimation.y != 0)
                    {
                        if (_vecPathToRestoreFromAttackAnimation.y < 0)
                            _vecPathToRestoreFromAttackAnimation.y += 1;
                        else
                            _vecPathToRestoreFromAttackAnimation.y -= 1;
                    }
                }
            }
            else
            {
                int _iCurrentPosition = iCurrentPosition;
                int _iMoveSide = iMoveSide;

                int iDelay = 5;

                if (BaseChar == 'E')
                {
                    if (_iCurrentPosition == iDelay)
                    {
                        _iMoveSide = 0;
                    }
                    else if (_iCurrentPosition == 0)
                        _iMoveSide = 1;

                    if (_iMoveSide == 1)
                    {
                        ++_iCurrentPosition;
                        if (!HL_TileManager.IsBlockedRegionChar(RightTile) && !HL_TileManager.IsTireHarmful(RightTile) && !HL_TileManager.IsEnemyTile(RightTile) && _vecPositionOnMap.x < iLengthOfCurrentLine - 1)
                            _vecPositionOnMap.x += 1;
                        else
                            _iCurrentPosition = iDelay;
                    }
                    else
                    {
                        --_iCurrentPosition;
                        if (!HL_TileManager.IsBlockedRegionChar(LeftTile) && !HL_TileManager.IsTireHarmful(LeftTile) && !HL_TileManager.IsEnemyTile(LeftTile) && _vecPositionOnMap.x > 0)
                            _vecPositionOnMap.x -= 1;
                        else
                            _iCurrentPosition = 0;
                    }
                }
            }

            return _vecPositionOnMap;
        }
        public void UpdatePosition(HL_Engine Engine, Vector2 vecLocalPlayerPosition, char LeftTile, char RightTile, int iLengthOfCurrentLine)
        {
            Vector2 vecDirection = vecPositionOnMap.GetDifference(vecLocalPlayerPosition);

            double DistanceFromLocalPlayer = vecLocalPlayerPosition.DistTo(vecPositionOnMap);

            HL_Console.SetCursorPos(new Vector2(0, 21));

            //check if the distance from local player is < 5 units so it attacks local player otherwise keep the patrol animation active

            if (DistanceFromLocalPlayer < 5)
            {
                HL_HudOverlay.iLastEnemyHealth = Handle.GetHealth();
                HL_HudOverlay.iLastEnemyShield = Handle.GetShield();
                HL_HudOverlay.UpdateHud(Engine.GameManager.LocalPlayer.Handle.GetHealth(), Engine.GameManager.LocalPlayer.Handle.GetShield(), HL_HudOverlay.iLives, Engine.GameManager.LocalPlayer.iLocalOxygenAmount, HL_HudOverlay.iCoinsCollected, HL_HudOverlay.iScore, HL_HudOverlay.iLastEnemyHealth, HL_HudOverlay.iLastEnemyShield);
              
                Vector2 vecNextPositionOnMap = new Vector2(vecPositionOnMap.x, vecPositionOnMap.y);
                if (BaseChar == 'B')

                {
                    vecNextPositionOnMap.x -= HL_Math.Clamp((int)vecDirection.x, -1, 1);
                    vecNextPositionOnMap.y -= HL_Math.Clamp((int)vecDirection.y, -1, 1);
                }
                else
                {
                    vecNextPositionOnMap.x += HL_Math.Clamp((int)vecDirection.x, -1, 1);
                    vecNextPositionOnMap.y += HL_Math.Clamp((int)vecDirection.y, -1, 1);
                }

                if (vecNextPositionOnMap.x >= iLengthOfCurrentLine && vecNextPositionOnMap.y < HL_MapManager.LoadedMap.Count)
                {
                    vecNextPositionOnMap.x -= 1;
                    vecDirection.x = 0;
                }

                if (vecNextPositionOnMap.y >= HL_MapManager.LoadedMap.Count)
                {
                    vecNextPositionOnMap.y -= 1;
                    vecDirection.y = 0;
                }

                if (vecNextPositionOnMap.x >= 0 && vecNextPositionOnMap.x < iLengthOfCurrentLine && vecNextPositionOnMap.y >= 0 && vecNextPositionOnMap.y < HL_MapManager.LoadedMap.Count)
                { 
                    if (!HL_TileManager.IsBlockedRegionChar(HL_MapManager.LoadedMap[vecNextPositionOnMap.y][vecNextPositionOnMap.x]) && !HL_TileManager.IsTireHarmful(HL_MapManager.LoadedMap[vecNextPositionOnMap.y][vecNextPositionOnMap.x])
                        && !HL_TileManager.IsEnemyTile(HL_MapManager.LoadedMap[vecNextPositionOnMap.y][vecNextPositionOnMap.x]))
                    {
                        if (BaseChar == 'B')
                        {
                            vecPathToRestoreFromAttackAnimation.x -= HL_Math.Clamp((int)vecDirection.x, -1, 1);
                            vecPathToRestoreFromAttackAnimation.y -= HL_Math.Clamp((int)vecDirection.y, -1, 1);
                        }
                        else
                        {
                            vecPathToRestoreFromAttackAnimation.x += HL_Math.Clamp((int)vecDirection.x, -1, 1);
                            vecPathToRestoreFromAttackAnimation.y += HL_Math.Clamp((int)vecDirection.y, -1, 1);
                        }

                        vecPositionOnMap.x = vecNextPositionOnMap.x;
                        vecPositionOnMap.y = vecNextPositionOnMap.y;
                    }
                }
            }
            else if ((vecPathToRestoreFromAttackAnimation.x != 0 || vecPathToRestoreFromAttackAnimation.y != 0) )
            {
                if (DistanceFromLocalPlayer > 6)
                {
                    Vector2 vecNextPositionOnMap = new Vector2(vecPositionOnMap.x, vecPositionOnMap.y);

                    if (BaseChar == 'B')
                    {
                        vecNextPositionOnMap.x -= HL_Math.Clamp((int)vecPathToRestoreFromAttackAnimation.x, -1, 1);
                        vecNextPositionOnMap.y -= HL_Math.Clamp((int)vecPathToRestoreFromAttackAnimation.y, -1, 1);
                    }
                    else
                    {
                        vecNextPositionOnMap.x -= HL_Math.Clamp((int)vecPathToRestoreFromAttackAnimation.x, -1, 1);
                        vecNextPositionOnMap.y -= HL_Math.Clamp((int)vecPathToRestoreFromAttackAnimation.y, -1, 1);
                    }
                    if (!HL_TileManager.IsBlockedRegionChar(HL_MapManager.LoadedMap[vecNextPositionOnMap.y][vecNextPositionOnMap.x]) && !HL_TileManager.IsTireHarmful(HL_MapManager.LoadedMap[vecNextPositionOnMap.y][vecNextPositionOnMap.x])
                        && !HL_TileManager.IsEnemyTile(HL_MapManager.LoadedMap[vecNextPositionOnMap.y][vecNextPositionOnMap.x]))
                    {
                        vecPositionOnMap.x = vecNextPositionOnMap.x;
                        vecPositionOnMap.y = vecNextPositionOnMap.y;
                    }
                    else
                    {
                        // I have to implement a path finder mechanism yet which will find a shortest path back to patrol position avoiding obstacles
                        vecPathToRestoreFromAttackAnimation.x = 0;
                        vecPathToRestoreFromAttackAnimation.y = 0;
                    }

                    if (vecPathToRestoreFromAttackAnimation.x != 0)
                    {
                        if (vecPathToRestoreFromAttackAnimation.x < 0)
                            vecPathToRestoreFromAttackAnimation.x += 1;
                        else
                            vecPathToRestoreFromAttackAnimation.x -= 1;
                    }

                    if (vecPathToRestoreFromAttackAnimation.y != 0)
                    {
                        if (vecPathToRestoreFromAttackAnimation.y < 0)
                            vecPathToRestoreFromAttackAnimation.y += 1;
                        else
                            vecPathToRestoreFromAttackAnimation.y -= 1;
                    }
                }
            }
            else
            {

                int iDelay = 5;

                if (BaseChar == 'E')
                {
                    if (iCurrentPosition == iDelay)
                    {
                        iMoveSide = 0;
                    }
                    else if (iCurrentPosition == 0)
                        iMoveSide = 1;

                    if (iMoveSide == 1)
                    {
                        ++iCurrentPosition;
                        if (!HL_TileManager.IsBlockedRegionChar(RightTile) && !HL_TileManager.IsTireHarmful(RightTile) && !HL_TileManager.IsEnemyTile(RightTile) && vecPositionOnMap.x < iLengthOfCurrentLine - 1)
                            vecPositionOnMap.x += 1;
                        else
                            iCurrentPosition = iDelay;
                    }
                    else
                    {
                        --iCurrentPosition;
                        if (!HL_TileManager.IsBlockedRegionChar(LeftTile) && !HL_TileManager.IsTireHarmful(LeftTile) && !HL_TileManager.IsEnemyTile(LeftTile) && vecPositionOnMap.x > 0)
                            vecPositionOnMap.x -= 1;
                        else
                            iCurrentPosition = 0;
                    }
                }
            }
        }

        public HL_BaseEntity Handle;
        public Vector2 vecPositionOnMap;
        public char BaseChar = ' ';

        private Vector2 vecPathToRestoreFromAttackAnimation;
        private int iCurrentPosition = 0;
        private int iMoveSide = 1;
        private int iLastPlayerMoveTick;
        private int iPlayerMoveTickWait;
    }

    public struct HL_WaitState
    {
        public HL_WaitState(int _iPlayerHurtTickWait)
        {
            iLastTickCount = 0;
            iLastPlayerHurtTick = 0;
            iPlayerHurtTickWait = _iPlayerHurtTickWait;
        }

        public bool ShouldUpdate()
        {
            if (iLastPlayerHurtTick == 0)
            {
                iLastPlayerHurtTick = iPlayerHurtTickWait;
                return true;
            }

            if (Environment.TickCount != iLastTickCount)
            {
                --iLastPlayerHurtTick;
                iLastTickCount = Environment.TickCount;
            }

            return false;
        }

        private int iLastTickCount;
        public int iLastPlayerHurtTick;
        public int iPlayerHurtTickWait;
    }

}
