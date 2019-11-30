using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GetTopMostWindow
{
	class ProcessHelper
	{
		[DllImport("user32.dll")]
		static extern int GetForegroundWindow();

		[DllImport("user32")]
		private static extern UInt32 GetWindowThreadProcessId(Int32 hWnd, out Int32 lpdwProcessId);


		private static Int32 GetWindowProcessID(Int32 hwnd)
		{
			Int32 pid = 1;
			GetWindowThreadProcessId(hwnd, out pid);
			return pid;
		}
		public static void PrintProcessInformation()
		{
			Int32 hwnd = 0;
			try
			{
				hwnd = GetForegroundWindow();
				var processId = GetWindowProcessID(hwnd);
				string appProcessName = Process.GetProcessById(processId).ProcessName;
				string appExePath = Process.GetProcessById(processId).MainModule.FileName;
				string appExeName = appExePath.Substring(appExePath.LastIndexOf(@"\") + 1);

                string selectedText = RemoteTextHelper.ReadText();
               // string selectedText ="";

                RemoteTextHelper.SetText(selectedText + "Hi");

                Console.WriteLine(processId + "|" + selectedText  + "|" + appProcessName + " | " + appExePath + " | " + appExeName);
			}
			catch (Exception ex)
			{
                Console.WriteLine(ex.Message);
				//Do Nothing. With process switch sometimes you get exception
			}
		}
	}
}
