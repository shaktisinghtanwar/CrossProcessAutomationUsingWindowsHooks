using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace GetTopMostWindow
{
    public class RemoteTextHelper
    {

        // declarations required for Win32 functions

        [DllImport("user32.dll")]

        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]

        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]

        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]

        static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]

        static extern bool AttachThreadInput(uint idAttach, uint idAttachTo,

        bool fAttach);

        [DllImport("user32.dll")]

        static extern IntPtr GetFocus();

        [DllImport("user32.dll")]

        static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);

        // second overload of SendMessage

        [DllImport("user32.dll")]

        static extern int SendMessage(IntPtr hWnd, uint Msg, out int wParam, out int lParam);

        const uint WM_GETTEXT = 0x0D;

        const uint WM_GETTEXTLENGTH = 0x0E;

        const uint EM_GETSEL = 0xB0;
        const int WM_SETTEXT = 0x000C;
        public static string ReadText()
        {
            IntPtr hWnd = GetForegroundWindow();

            uint processId;

            uint activeThreadId = GetWindowThreadProcessId(hWnd, out processId);

            uint currentThreadId = GetCurrentThreadId();

            AttachThreadInput(activeThreadId, currentThreadId, true);

            IntPtr focusedHandle = GetFocus();

            AttachThreadInput(activeThreadId, currentThreadId, false);

            int len = SendMessage(focusedHandle, WM_GETTEXTLENGTH, 0, null);

            StringBuilder sb = new StringBuilder(len);

            int numChars = SendMessage(focusedHandle, WM_GETTEXT, len + 1, sb);

            int start, next;

            SendMessage(focusedHandle, EM_GETSEL, out start, out next);

            string selectedText = sb.ToString().Substring(start, next - start);
            return selectedText;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern string SendMessage(IntPtr hWnd, int msg, string wParam, IntPtr lParam);
        public static void SetText(string text)
        {
            IntPtr hWnd = GetForegroundWindow();

            uint processId;

            uint activeThreadId = GetWindowThreadProcessId(hWnd, out processId);

            uint currentThreadId = GetCurrentThreadId();

            AttachThreadInput(activeThreadId, currentThreadId, true);

            IntPtr focusedHandle = GetFocus();

            AttachThreadInput(activeThreadId, currentThreadId, false);

            SendMessage(  focusedHandle  , WM_SETTEXT,  0,new StringBuilder( text));
        }
    }
}
