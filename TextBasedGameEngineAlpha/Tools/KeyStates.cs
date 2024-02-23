using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

enum EVirtualKeys
{

}
namespace TextBasedGameEngineAlpha.Tools
{
    public class HL_VirtualKeyState
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int key);


    }
}
