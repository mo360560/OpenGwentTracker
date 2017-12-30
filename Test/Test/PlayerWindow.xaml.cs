using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Shapes;
using Test.Classes;
using Test.Enums;

namespace Test
{
    public partial class PlayerWindow : Window
    {
        PlayerInfo player;

        public PlayerWindow()
        {
            InitializeComponent();
            /*  To disable the ability to drag the window by clicking anywhere,
                1. Remove IsEnable="False" from <ItemsPresenter IsEnabled="False" />
                2. Remove WindowStyle="None"
                3. Remove the next line */               
            MouseLeftButtonDown += delegate { DragMove(); };
        }
        public void SetPlayer(PlayerInfo player)
        {
            this.player = player;
            UsernameBox.Text = player.player_info;
            if (player.type == PlayerType.RED)
                background.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Test;component/Images/bg_red.png"));
            else background.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Test;component/Images/bg_blue.png"));

        }

        public void Update()
        {            
            ICollectionView view = CollectionViewSource.GetDefaultView(player.cards_list);
            view.GroupDescriptions.Add(new PropertyGroupDescription("placement"));
            view.SortDescriptions.Add(new SortDescription("placement", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("power", ListSortDirection.Descending));
            CardsListBox.ItemsSource = view;
        }
    }
}
