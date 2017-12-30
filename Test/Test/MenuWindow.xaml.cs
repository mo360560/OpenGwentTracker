using System;
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
using Test.Enums; //@TODO For testing, may not be needed later

namespace Test
{
    public partial class MenuWindow : Window
    {
        PlayerInfo user_info, opponent_info;
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

            log_parser = new LogParser();
            //log_parser.Parse();

            opponent_info = new PlayerInfo("Jane", 26, 18, 3999, PlayerType.BLUE, "Nova deck #1");
            opponent_window.SetPlayer(opponent_info);
            opponent_info.MoveCard(CardPlacement.BANISHED, 1);
            opponent_window.Update();
        }        
    }
}
