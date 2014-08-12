using System;

namespace SendToProcess
{
    public class Program
    {
        static void Main(string[] args)
        {
            ProcessManager.SendKeysToWindow("jj", "Notepad", "Untitled - Notepad");
            //ProcessManager.SendTextToNotepad("kkk", "Notepad", "Untitled - Notepad");
            //ProcessManager.SendKeysToWindowByProcess("Hey there 2", "Notepad", "Untitled - Notepad");
            //ProcessManager.SendKeysTitleContaining("{ENTER}HAHA", "sublime_text", "untitled");
            //ProcessManager.PlayPauseSpotify();
            Console.Read();
        }
    }
}
