using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedGameEngine.Tools
{
    public class HL_Console
    {
        private const int STD_OUTPUT_HANDLE = -11;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct CONSOLE_FONT_INFOEX
        {
            internal uint cbSize;
            internal uint nFont;
            internal COORD dwFontSize;
            internal int FontFamily;
            internal int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            internal string FaceName;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct COORD
        {
            internal short X;
            internal short Y;

            internal COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetCurrentConsoleFontEx(
            IntPtr consoleOutput,
            bool maximumWindow,
            ref CONSOLE_FONT_INFOEX consoleCurrentFontEx);
        public static void SetCursorPos(Vector2 vecPosition)
        {
            Console.SetCursorPosition((int)vecPosition.x, (int)vecPosition.y);
        }
        public static string ChangeStringElement(string sValue, int iIndex, char charValue)
        {
            string strValueOut = "";

            for (int iCurrentElement = 0; iCurrentElement < sValue.Length; iCurrentElement++)
            {
                if (iCurrentElement == iIndex)
                    strValueOut += charValue;
                else
                    strValueOut += sValue[iCurrentElement];
            }

            return strValueOut;
        }

        public static void SetColorAndDrawConsole(char cInput, ConsoleColor Color, ConsoleColor ColorBkg = ConsoleColor.Black)
        {
            ConsoleColor colOriginal = Console.ForegroundColor;
            ConsoleColor colOriginalBkg = Console.BackgroundColor;
            Console.ForegroundColor = Color;
            Console.BackgroundColor = ColorBkg;
            Console.Write(cInput);
            Console.ForegroundColor = colOriginal;
            Console.BackgroundColor = colOriginalBkg;
        }


        public static void SetConsoleFontSize(short width, short height)
        {
            IntPtr hnd = GetStdHandle(STD_OUTPUT_HANDLE);
            if (hnd != IntPtr.Zero)
            {
                CONSOLE_FONT_INFOEX fontInfo = new CONSOLE_FONT_INFOEX();
                fontInfo.cbSize = (uint)Marshal.SizeOf(fontInfo);

                fontInfo.dwFontSize = new COORD(width, height);

                SetCurrentConsoleFontEx(hnd, false, ref fontInfo);
            }
        }
        public static void SetColorAndDrawConsole(string szInput, ConsoleColor Color, ConsoleColor ColorBkg = ConsoleColor.Black)
        {
            ConsoleColor colOriginal = Console.ForegroundColor;
            ConsoleColor colOriginalBkg = Console.BackgroundColor;
            Console.ForegroundColor = Color;
            Console.BackgroundColor = ColorBkg;
            Console.Write(szInput);
            Console.ForegroundColor = colOriginal;
            Console.BackgroundColor = colOriginalBkg;
        }

        public static void DrawError(string szString, [CallerMemberName] string szMemberName = "", [CallerLineNumber] int iLineNumber = 0)
        {
            SetColorAndDrawConsole("[ERROR MESSAGE from " + $"{szMemberName} at line " + $"{iLineNumber}" + "] " + szString + "\n", ConsoleColor.DarkRed);
        }
    }
}
