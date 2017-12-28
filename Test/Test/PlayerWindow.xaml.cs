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
        ObservableCollection<Card> cards_list;

        public PlayerWindow()
        {
            InitializeComponent();
        }
        public void ShowPlayer(PlayerInfo player)
        {
            cards_list = new ObservableCollection<Card>(player.cards_list);            
            ICollectionView view = CollectionViewSource.GetDefaultView(cards_list);
            view.GroupDescriptions.Add(new PropertyGroupDescription("placement"));
            view.SortDescriptions.Add(new SortDescription("placement", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("power", ListSortDirection.Descending));
            CardsListBox.ItemsSource = view;
        }
    }
}
