using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
enum EUIScreen
{
    UI_MAINMENU = 0,
    UI_SCREE_MAX,
}

namespace TextBasedGameEngine.Engine.RenderSystem
{
    public class HL_UserInterface
    {
        HL_Engine OwningEngine = null;
        public void Initialize(HL_Engine _OwningEngine)
        {
            OwningEngine = _OwningEngine;
        }
        void LoadUI(EUIScreen Screen = EUIScreen.UI_MAINMENU)
        {

        }
    }
}
