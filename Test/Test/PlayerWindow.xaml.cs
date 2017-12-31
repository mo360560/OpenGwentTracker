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
        HashSet<CardPlacement> displayed;

        public PlayerWindow()
        {
            InitializeComponent();
            /*  To disable the ability to drag the window by clicking anywhere,
                1. Remove IsEnabled="False" from <ItemsPresenter IsEnabled="False" />
                2. Remove WindowStyle="None"
                3. Remove the next line */
            MouseLeftButtonDown += delegate { DragMove(); };
            displayed = new HashSet<CardPlacement> {
                //CardPlacement.HAND, CardPlacement.BOARD,
                CardPlacement.DECK, CardPlacement.GRAVEYARD, CardPlacement.BANISHED
            };
        }
        public void SetPlayer(PlayerInfo player)
        {
            this.player = player;
            UsernameBox.Text = player.player_info;
            CreateBorder(player.type);
        }
        public void SetDisplayed(HashSet<CardPlacement> displayed) => this.displayed = displayed;
        public void AddCard(CardPlacement dest, byte card_ID, int template_ID)
        {
            player.AddCard(dest, card_ID, template_ID);
            Update();
        }
        public void MoveCard(CardPlacement dest, byte card_ID)
        {
            player.MoveCard(dest, card_ID);
            Update();
        }
        public void TransformCard(byte card_ID, int template_ID)
        {
            player.TransformCard(card_ID, template_ID);
            Update();
        }
        public void ChangeCardStats(byte card_ID, PowerChangeType change_type, short value_difference, bool ignore_armor)
        {
            player.ChangeCardStats(card_ID, change_type, value_difference, ignore_armor);
            Update();
        }
        private void Update()
        {
            List<Card> all_cards = player.cards_list;
            ICollectionView view = CollectionViewSource.GetDefaultView(all_cards.Where(c => (displayed.Contains(c.placement))));
            view.GroupDescriptions.Add(new PropertyGroupDescription("placement"));
            view.SortDescriptions.Add(new SortDescription("placement", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("color", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("power", ListSortDirection.Descending));
            CardsListBox.ItemsSource = view;
        }
        private void CreateBorder(PlayerType type)
        {            
            String horizontal = "border_horizontal_";
            String LU = "border_corner_LU_";
            String vertical = "border_vertical_";
            String LL = "border_corner_LL_";
            AddImage(horizontal, 0, 1, false);
            AddImage(LU, 0, 0, false);
            AddImage(LU, 0, 2, true);
            AddImage(vertical, 1, 0, true);
            AddImage(vertical, 1, 2, false);
            AddImage(horizontal, 3, 1, false);
            AddImage(LL, 3, 0, false);
            AddImage(LL, 3, 2, true);

        }
        private void AddImage(String file, byte row, byte column, bool mirror)
        {
            Image i = new Image();
            String type = "red.png";
            if (player.type == PlayerType.BLUE) type = "blue.png";
            i.Source = new BitmapImage(new Uri(@"pack://application:,,,/Test;component/Images/"+file+type));

            i.Stretch = Stretch.Fill;
            Grid.SetRow(i, row);
            Grid.SetColumn(i, column);

            if (file == "border_horizontal_")
                i.VerticalAlignment = VerticalAlignment.Top;            
            if (mirror)
            {
                ScaleTransform scale = new ScaleTransform();
                scale.ScaleX = -1;
                i.RenderTransformOrigin = new Point(0.5, 0.5);
                i.RenderTransform = scale;
            }
            WindowGrid.Children.Add(i);
        }
    }
}