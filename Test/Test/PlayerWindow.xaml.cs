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
            background.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Test;component/Images/bg1.png"));
            CreateBorder(player.type);
        }

        public void Update()
        {            
            ICollectionView view = CollectionViewSource.GetDefaultView(player.cards_list);
            view.GroupDescriptions.Add(new PropertyGroupDescription("placement"));
            view.SortDescriptions.Add(new SortDescription("placement", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("power", ListSortDirection.Descending));
            CardsListBox.ItemsSource = view;
        }

        private void CreateBorder(PlayerType type)
        {
            ScaleTransform scale = new ScaleTransform();
            scale.ScaleX = -1;
            String horizontal = "border_horizontal_";
            String LU = "border_corner_LU_";
            String vertical = "border_vertical_";
            String LL = "border_corner_LL_";

            Image i = SetImage(horizontal, 0, 1);
            i.VerticalAlignment = VerticalAlignment.Top;
            WindowGrid.Children.Add(i);

            i = SetImage(LU, 0, 0);
            WindowGrid.Children.Add(i);

            i = SetImage(LU, 0, 2);
            i.RenderTransformOrigin = new Point(0.5, 0.5);
            i.RenderTransform = scale;
            WindowGrid.Children.Add(i);

            i = SetImage(vertical, 1, 0);
            i.RenderTransformOrigin = new Point(0.5, 0.5);
            i.RenderTransform = scale;
            WindowGrid.Children.Add(i);

            i = SetImage(vertical, 1, 2);
            WindowGrid.Children.Add(i);

            i = SetImage(horizontal, 3, 1);
            WindowGrid.Children.Add(i);

            i = SetImage(LL, 3, 0);
            WindowGrid.Children.Add(i);

            i = SetImage(LL, 3, 2);
            i.RenderTransformOrigin = new Point(0.5, 0.5);
            i.RenderTransform = scale;
            WindowGrid.Children.Add(i);
        }

        private Image SetImage(String file, byte row, byte column)
        {
            Image i = new Image();
            String type = "red.png";
            if (player.type == PlayerType.BLUE) type = "blue.png";
            i.Source = new BitmapImage(new Uri(@"pack://application:,,,/Test;component/Images/"+file+type));
            i.Stretch = Stretch.Fill;
            Grid.SetRow(i, row);
            Grid.SetColumn(i, column);
            return i;
        }
    }
}