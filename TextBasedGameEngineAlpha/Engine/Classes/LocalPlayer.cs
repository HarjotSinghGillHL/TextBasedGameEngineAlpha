using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine.Classes;

namespace TextBasedGameEngineAlpha.Engine.Classes
{   
    public class HL_LocalPlayer : HL_BaseEntity
    {
        public HL_LocalPlayer(string _szSanitizedName)
        {
            szSanitizedName = _szSanitizedName;
            eEntityClass = EEntityClass.ENTITY_PLAYER;
            this.iMinHealth = 0;
            this.iMaxHealth = 100;
            this.iMinShield = 0;
            this.iMaxShield = 100;
            this.iHealth = iMaxHealth;
            this.iShield = iMaxShield;
        }

    }
}
