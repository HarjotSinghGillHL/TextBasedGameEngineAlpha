using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine.Classes;

namespace TextBasedGameEngineAlpha.Engine.Classes
{
    public class HL_StandingEnemy : HL_BaseEntity
    {
        public HL_StandingEnemy(string _szSanitizedName)
        {
            szSanitizedName = _szSanitizedName;
            eEntityClass = EEntityClass.ENTITY_PLAYER;
            this.iMinHealth = 0;
            this.iMaxHealth = 25;
            this.iMinShield = 0;
            this.iMaxShield = 25;
            this.iHealth = iMaxHealth;
            this.iShield = iMaxShield;
        }


    }
}
