using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedGameEngine.Tools
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
   

    public static class HL_System
    {

        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int nIndex);

        const int SM_CXSCREEN = 0;
        const int SM_CYSCREEN = 1;

        public static Vector2 GetScreenSize()
        {
            return new Vector2(GetSystemMetrics(SM_CXSCREEN), GetSystemMetrics(SM_CYSCREEN));
        }



        [DllImport("user32.dll")]
        static extern int GetDpiForWindow(IntPtr hWnd);

        [DllImport("User32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("User32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("Gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        private const int LOGPIXELSX = 88;
        private const int LOGPIXELSY = 90;

        public static float GetScreenDpiScale()
        {
            try
            {
                return GetDpiForWindow(IntPtr.Zero) / 96f;
            }
            catch
            {
                IntPtr desktopHwnd = IntPtr.Zero;
                IntPtr desktopDC = GetDC(desktopHwnd);
                int dpiX = GetDeviceCaps(desktopDC, LOGPIXELSX);
                int dpiY = GetDeviceCaps(desktopDC, LOGPIXELSY);
                ReleaseDC(desktopHwnd, desktopDC);
                return dpiY / 96f;
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_MAXIMIZE = 3; 

        public static void SetConsoleWindowToFullScreen()
        {
            IntPtr consoleWindow = GetConsoleWindow();

            if (consoleWindow != IntPtr.Zero)
            {
                ShowWindow(consoleWindow, SW_MAXIMIZE);
            }

        }
    }

}
