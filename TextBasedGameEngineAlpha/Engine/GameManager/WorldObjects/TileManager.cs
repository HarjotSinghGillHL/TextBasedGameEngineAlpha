using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedGameEngineAlpha.Engine.GameManager.WorldObjects
{
    public class HL_TileManager
    {
        public static bool bEnteredLava = false;
        public static bool bEnteredWater = false;
        public static bool IsEnemyTile(char Value)
        {
            return Value == 'E' || Value == 'K' || Value == 'B';
        }
        public static bool IsBlockedRegionChar(char Value)
        {
            return Value == 'M' || Value == 'T';
        }
        public static bool IsTireHarmful(char Value)
        {
            return Value == 'L';
        }
    }
}
