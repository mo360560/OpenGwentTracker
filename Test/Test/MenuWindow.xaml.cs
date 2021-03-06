﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Test.Classes;
using System.Runtime.InteropServices;
using System.Threading;

namespace Test
{
    public partial class MenuWindow : Window
    {
        PlayerWindow user_window, opponent_window;
        WindowsManager windows_manager;
        LogParser log_parser;

        public MenuWindow()
        {
            InitializeComponent();
            user_window = new PlayerWindow();
            opponent_window = new PlayerWindow();
            windows_manager = new WindowsManager(this, user_window, opponent_window);
            windows_manager.SetInUse(this, true);
            log_parser = new LogParser(user_window, opponent_window);
        }        
    }
}
