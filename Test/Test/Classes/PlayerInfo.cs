using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Enums;

namespace Test.Classes
{
    using Dict = Dictionary<byte, Card>;
    class PlayerInfo
    {        
        private String name;
        private byte level, rank;
        private int MMR;
        private Dictionary<byte, Card> hand, board, deck, graveyard, banished;
        private Dictionary<CardPlacement, Dict> placements;

        public PlayerInfo(String name, byte level, byte rank, int MMR)
        {
            this.name = name; this.level = level; this.rank = rank; this.MMR = MMR;
            placements.Add(CardPlacement.HAND, hand);
            placements.Add(CardPlacement.BOARD, board);
            placements.Add(CardPlacement.DECK, deck);
            placements.Add(CardPlacement.GRAVEYARD, graveyard);
            placements.Add(CardPlacement.BANISHED, banished);
        }
        public void AddCard(CardPlacement dest, byte card_ID, int template_ID)
        {
            placements[dest].Add(card_ID, new Card(template_ID));
        }
        public void MoveCard(CardPlacement from, CardPlacement to, byte card_ID)
        {
            placements[to].Add(card_ID, placements[from][card_ID]);
            placements[from].Remove(card_ID);            
        }
        public void ChangeStats(byte card_ID, PowerChangeType change_type, short value_difference, bool ignore_armor)
        {
            foreach (Dict placement in placements.Values)
            {
                if (placement.ContainsKey(card_ID))
                {
                    placement[card_ID].ChangeStats(change_type, value_difference, ignore_armor);
                }
            }
        }

    }
}
