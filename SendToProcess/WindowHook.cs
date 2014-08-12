using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace SendToProcess
{
    public class ProcessManager
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
    
        private const uint WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const uint WM_SYSCOMMAND = 0x018;
        private const uint SC_CLOSE = 0x053;
        private const int WM_SETTEXT = 0x000C;

        public static void SendTextToNotepad(string keys, string processName, string title) {
            IntPtr handle = FindWindow(processName, title);

            if (!handle.Equals(IntPtr.Zero)) {
                 IntPtr child = FindWindowEx(handle, new IntPtr(0), "Edit", null);
                 int result = SendMessage(child, WM_SETTEXT, 0, "Tada");
            }
        }

        public static void SendKeysToWindow(string keys, string processName, string title)
        {
            IntPtr handle = FindWindow(processName, title);

            if (!handle.Equals(IntPtr.Zero))
            {
                SetForegroundWindow(handle);
                IntPtr result = SendMessage(handle, WM_KEYDOWN, (IntPtr)'k', IntPtr.Zero);
            }
        }

        public static void SendKeysToWindowByProcess(string keys, string processName, string title) {
            Process[] processes = Process.GetProcessesByName(processName);
            Process foundProcess = processes.FirstOrDefault(p => p.MainWindowTitle.ToLower() == title.ToLower());

            if (foundProcess == null) {
                Console.WriteLine(string.Format("Couldn't find process \"{0}\" with title containing \"{1}\"", processName, title));
                return;
            }

            IntPtr handle = foundProcess.MainWindowHandle;

            SetForegroundWindow(handle);
            SendKeys.SendWait(keys);
        }

        public static void SendKeysTitleContaining(string keys, string process, string titleContains) {
            Process[] processes = Process.GetProcessesByName(process);
            Process foundProcess = processes.FirstOrDefault(p => p.MainWindowTitle.ToLower().Contains(titleContains.ToLower()));

            if (foundProcess == null) {
                Console.WriteLine(string.Format("Couldn't find process \"{0}\" with title containing \"{1}\"", process, titleContains));
                return;
            }

            IntPtr handle = foundProcess.MainWindowHandle;

            SetForegroundWindow(handle);
            SendKeys.SendWait(keys);
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        private const int WM_APPCOMMAND = 0x319;
        private const int APPCOMMAND_MEDIA_PLAY_PAUSE = 0xE0000;
        public static void PlayPauseSpotify() {
            IntPtr handle = FindWindow("spotify", "spotify");

            if (handle.Equals(IntPtr.Zero)) {
                Process[] processes = Process.GetProcessesByName("spotify");
                Process foundProcess = processes.FirstOrDefault();

                if (foundProcess == null) {
                    Console.WriteLine("Couldn't find spotify process");
                    return;
                }

                handle = foundProcess.MainWindowHandle;
            }

            SendMessageW(handle, WM_APPCOMMAND, handle, (IntPtr)APPCOMMAND_MEDIA_PLAY_PAUSE);
        }
    }
}