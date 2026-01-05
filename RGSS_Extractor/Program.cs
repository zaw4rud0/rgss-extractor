using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RGSS_Extractor
{
    internal static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeConsole();

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var foregroundWindow = GetForegroundWindow();
                int processId;
                GetWindowThreadProcessId(foregroundWindow, out processId);
                var processById = Process.GetProcessById(processId);
                if (processById.ProcessName == "cmd")
                {
                    AttachConsole(processById.Id);
                }
                else
                {
                    AllocConsole();
                }

                var mainParser = new MainParser();
                mainParser.ParseFile(args[0]);
                mainParser.ExportArchive(args[1]);
                FreeConsole();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}