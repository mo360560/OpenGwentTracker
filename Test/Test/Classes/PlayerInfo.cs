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
                String info;
                if (type == PlayerType.RED)
                    info = name;
                else info = deck_name;
                if (info.Length > 16) info = info.Substring(0, 14) + "...";
                return info;
            }
        }

        public PlayerInfo(String name, byte level, byte rank, int MMR, PlayerType type, String deck_name)
        {
            this.name = name; this.level = level; this.rank = rank; this.MMR = MMR;
            this.type = type; this.deck_name = deck_name;
        }

        public PlayerInfo(String name, byte level, byte rank, int MMR, PlayerType type)
        {
            this.name = name; this.level = level; this.rank = rank; this.MMR = MMR;
            this.type = type;
        }

        public void AddCard(CardPlacement dest, byte card_ID, int template_ID)
        {
            cards.Add(card_ID, new Card(template_ID, dest));
        }
        public void MoveCard(CardPlacement dest, byte card_ID)
        {
            cards[card_ID].placement = dest;     
        }
        public void TransformCard(byte card_ID, int template_ID)
        {
            cards[card_ID] = new Card(template_ID, cards[card_ID].placement);
        }
        public void ChangeCardStats(byte card_ID, PowerChangeType change_type, short value_difference, bool ignore_armor)
        {
            cards[card_ID].ChangeStats(change_type, value_difference, ignore_armor);
        }

    }
}
