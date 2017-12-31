using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Enums;

namespace Test.Classes
{
    using Dict = Dictionary<byte, Card>;
    public class PlayerInfo
    {        
        private String name;        
        private byte level, rank;
        private int MMR;
        public PlayerType type { get; }
        private String deck_name;
        private Dictionary<byte, Card> cards;
        public List<Card> cards_list => cards.Values.ToList();
        public String player_info {
            get {
                if (type == PlayerType.RED)
                    return name;
                else return deck_name;
            }
        }

        public PlayerInfo(String name, byte level, byte rank, int MMR, PlayerType type, String deck_name)
        {
            this.name = name; this.level = level; this.rank = rank; this.MMR = MMR;
            this.type = type; this.deck_name = deck_name;

            //for testing:
            cards = new Dictionary<byte, Card> {
                { 1, new Card("Alzur's Thunder", 0, 0, CardColor.BRONZE, CardPlacement.HAND) },
                { 2, new Card("Shupe's Bizarre Adventure", 0, 0, CardColor.GOLD, CardPlacement.HAND) },
                { 3, new Card("Ciri: Nova", 1, 3, CardColor.GOLD,  CardPlacement.DECK) },
                { 4, new Card("Vrihedd Neophyte", 10, 0, CardColor.BRONZE, CardPlacement.DECK) },
                { 5, new Card("Reconaissance", 0, 0, CardColor.BRONZE, CardPlacement.DECK) },
                { 6, new Card("Elven Swordmaster", 5, 6, CardColor.BRONZE, CardPlacement.BOARD) },
                { 7, new Card("Mandrake", 0, 0, CardColor.SILVER, CardPlacement.GRAVEYARD) },
                { 8, new Card("Elven Mercenary", 0, -1, CardColor.BRONZE, CardPlacement.BANISHED) },
            };
        }  
        public void AddCard(CardPlacement dest, byte card_ID, int template_ID)
        {
            cards.Add(card_ID, new Card(template_ID, dest));
        }
        public void MoveCard(CardPlacement dest, byte card_ID)
        {
            cards[card_ID].placement = dest;     
        }
        public void ChangeStats(byte card_ID, PowerChangeType change_type, short value_difference, bool ignore_armor)
        {
            cards[card_ID].ChangeStats(change_type, value_difference, ignore_armor);
        }

    }
}
