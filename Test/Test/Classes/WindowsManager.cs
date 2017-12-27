using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Test.Classes
{
    class WindowsManager
    {
        public short delay;
        MenuWindow menu;
        PlayerWindow user, opponent;
        Window[] windows;
        Dictionary<Window, WindowSettings> settings = new Dictionary<Window, WindowSettings>();
        HashSet<String> windows_to_lay_over = new HashSet<String> { "Test", "Gwent" };

        public WindowsManager(MenuWindow menu, PlayerWindow user, PlayerWindow opponent)
        {
            this.menu = menu;
            this.user = user;
            this.opponent = opponent;            
            windows = new Window[3] { menu, user, opponent };
            foreach (Window w in windows)
            {
                w.Topmost = true; w.Top = 0;
            }
            settings.Add(menu, new WindowSettings());
            settings.Add(user, new WindowSettings());
            settings.Add(opponent, new WindowSettings());

            Thread windows_manager = new Thread(new ThreadStart(ManagerThread));
            windows_manager.IsBackground = true;
            windows_manager.Start();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();
        public void ManagerThread()
        {
            while (true)
            {
                Thread.Sleep(delay);
                var activatedHandle = GetForegroundWindow();
                Process[] processes = Process.GetProcesses();
                foreach (Process cls_process in processes)
                {
                    if (activatedHandle == cls_process.MainWindowHandle)
                    {
                        menu.Dispatcher.Invoke(() =>
                        {
                            if (!LayOver(cls_process.ProcessName)) HideAllWindows();
                            else ShowWindowsInUse(); 
                        });
                    }
                }
            }
        }
        private bool LayOver(String name) => windows_to_lay_over.Contains(name);
        private void HideAllWindows()
        {
            foreach (Window w in windows) w.Hide();
        }
        private void ShowWindowsInUse()
        {
            foreach (Window w in windows) if (settings[w].in_use) w.Show();
        }
        public void SetInUse(Window w, bool value)
        {
            settings[w].in_use = value;
        }

        private class WindowSettings
        {
            public bool in_use = true;
        }

    }    
}
