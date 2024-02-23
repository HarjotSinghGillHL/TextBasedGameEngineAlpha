using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedGameEngine.Tools
{
    public class HL_Math
    {
        public static int Clamp(int iValue, int iMinValue, int iMaxValue)
        {
            if (iValue < iMinValue)
                return iMinValue;

            if (iValue > iMaxValue)
                return iMaxValue;

            return iValue;
        }

        public static float Clamp(float flValue, float flMinValue, float flMaxValue)
        {
            if (flValue < flMinValue)
                return flMinValue;

            if (flValue > flMaxValue)
                return flMaxValue;

            return flValue;
        }
    }
}
