using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine.Classes;
using TextBasedGameEngine.Tools;

namespace TextBasedGameEngine.Engine.GameManager
{
    public class HL_DamageHandler
    {

        public HL_DamageHandler()
        {

        }

        public static bool HealEntity(int iAmountOfHeal, HL_BaseEntity Player)
        {
            if (Player.GetHealth() >= Player.GetMaxHealth())
                return false;

            Player.SetHealth(HL_Math.Clamp(Player.GetHealth() + iAmountOfHeal, 0, Player.GetMaxHealth()));

            return true;
        }

        public static bool GainShield(int iAmountOfGain, HL_BaseEntity Player)
        {
            if (Player.GetShield() >= Player.GetMaxShield())
                return false;

            Player.SetShield(HL_Math.Clamp(Player.GetShield() + iAmountOfGain, 0, Player.GetMaxShield()));

            return true;
        }
        public static bool HurtEntity(int iDamageDealt, HL_BaseEntity Victim,bool bHitShield = true)
        {
            if (iDamageDealt <= 0)
                return false;

            int iNewHealth = Victim.GetHealth();

     
            if (Victim.GetShield() > 0 && bHitShield)
            {
                int iNewShield = Victim.GetShield() - iDamageDealt;

                if (iNewShield < 0)
                {
                    iNewHealth += iNewShield;
                    iNewShield = 0;
                }

                Victim.SetShield(iNewShield);
            }
            else
            {
                iNewHealth = Victim.GetHealth() - iDamageDealt;
            }

            if (iNewHealth < 0)
                iNewHealth = 0;

            Victim.SetHealth(iNewHealth);

            if (iNewHealth == 0)
            {
                Victim.KillPlayer();
                return true;
            }

            return false;
        }
    }
}