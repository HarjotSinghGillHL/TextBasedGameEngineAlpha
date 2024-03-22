using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine.Classes;
using TextBasedGameEngine.Tools;
using TextBasedGameEngineAlpha.Engine.Classes;
using TextBasedGameEngineAlpha.Engine.GameManager.WorldObjects;

namespace TextBasedGameEngine.Engine.GameManager
{
    public static class HL_WorldEntityManager
    {
        public static List<HL_MapEntity> WorldEntities;
        public static List<Vector2> CoinsConsumed;

        public static HL_MapEntity GetEntityFromListByPosition(ref List<HL_MapEntity> EntityList, Vector2 vecPosition)
        {
            for (int iCurrentIndex = 0; iCurrentIndex < EntityList.Count; iCurrentIndex++)
                if (EntityList[iCurrentIndex].vecPositionOnMap == vecPosition)
                    return EntityList[iCurrentIndex];

            return null;
        }
        public static bool RemoveEntityFromList(ref List<HL_MapEntity> EntityList, Vector2 vecPosition)
        {
            int iIndex = -1;

            for (int iCurrentIndex = 0; iCurrentIndex < EntityList.Count; iCurrentIndex++)
            {
                if (EntityList[iCurrentIndex].vecPositionOnMap == vecPosition)
                {
                    iIndex = iCurrentIndex;
                    break;
                }
            }

            if (iIndex != -1)
            {
                EntityList.RemoveAt(iIndex);
                return true;
            }

            return false;
        }
        public static List<HL_MapEntity> GenerateMapEntityList(List<string> Map)
        {
            List<HL_MapEntity> EntityList = new List<HL_MapEntity>();

            for (int iCurrentListIndex = 0; iCurrentListIndex < Map.Count; iCurrentListIndex++)
            {
                for (int iCurrentStringIndex = 0; iCurrentStringIndex < Map[iCurrentListIndex].Count(); iCurrentStringIndex++)
                {
                    if (HL_TileManager.IsEnemyTile(Map[iCurrentListIndex][iCurrentStringIndex]))
                    {
                        if (Map[iCurrentListIndex][iCurrentStringIndex] == 'E')
                            EntityList.Add(new HL_MapEntity( new HL_SmallEnemy("Enemy"), new Vector2(iCurrentStringIndex, iCurrentListIndex), Map[iCurrentListIndex][iCurrentStringIndex]));
                        else if (Map[iCurrentListIndex][iCurrentStringIndex] == 'K')
                            EntityList.Add(new HL_MapEntity(new HL_StandingEnemy("Enemy"), new Vector2(iCurrentStringIndex, iCurrentListIndex), Map[iCurrentListIndex][iCurrentStringIndex]));
                        else if (Map[iCurrentListIndex][iCurrentStringIndex] == 'B')
                            EntityList.Add(new HL_MapEntity(new HL_RunnerEnemy("Enemy"), new Vector2(iCurrentStringIndex, iCurrentListIndex), Map[iCurrentListIndex][iCurrentStringIndex]));
                    }
                }
            }

            return EntityList;
        }
        public static Vector2 FindLocalPlayerInMap(List<string> Map)
        {
            for (int iCurrentListIndex = 0; iCurrentListIndex < Map.Count; iCurrentListIndex++)
            {
                for (int iCurrentStringIndex = 0; iCurrentStringIndex < Map[iCurrentListIndex].Count(); iCurrentStringIndex++)
                {
                    if (Map[iCurrentListIndex][iCurrentStringIndex] == HL_MapManager.LocalPlayerChar)
                    {
                        return new Vector2(iCurrentStringIndex, iCurrentListIndex);
                    }
                }
            }

            return new Vector2();
        }
    }
}
