using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Enums;

namespace Test.Classes
{
    class Card
    {
        private String name;
        private short base_strength, buff, armor;

        public Card(int template_ID)
        {
            //here we browse database for card with given template_ID
            name = "Test"; base_strength = buff = armor = 3;
        }
        public void ChangeStats(PowerChangeType change_type, short value_difference, bool ignore_armor)
        {
            switch (change_type)
            {
                case PowerChangeType.STRENGTHEN:    StrengthenOrWeaken(value_difference); break;
                case PowerChangeType.BUFF:          BuffOrDamage(value_difference, ignore_armor); break;
                case PowerChangeType.ARMOR:         GiveOrDestroyArmor(value_difference); break;                    
            }
        }
        private void StrengthenOrWeaken(short value)
        {
            base_strength += value;
            if (base_strength < 0) base_strength = 0;
        }
        private void BuffOrDamage(short value, bool ignore_armor)
        {
            if (!ignore_armor && value < 0)
            {
                armor -= value;
                if (armor < 0)
                {
                    buff += armor;
                    armor = 0;
                }
            }
            else buff += value;
            if (-buff > base_strength) buff = (short)-base_strength;

        }        
        private void GiveOrDestroyArmor(short value)
        {
            armor += value;
            if (armor < 0) armor = 0;
        }
        public void Reset() => buff = 0;
    }
}
