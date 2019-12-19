using System;

namespace Mysterious_Dungeon.Items
{
    class Weapon : Item
    {
        public Weapon()
        {
            slot = "weapon";
            return;
        }
        public Weapon(string name, int damage, int manaCost, string rareness, string desc)
        {
            Name = name;
            Dmg = damage;
            DmgType = "magical";
            Rareness = rareness;
            Desc = desc;
            ManaCost = manaCost;
            slot = "weapon";
        }
        public Weapon(string name, int damage, string rareness, string desc)
        {
            Name = name;
            Dmg = damage;
            DmgType = "physical";
            Rareness = rareness;
            Desc = desc;
            ManaCost = 0;
            slot = "weapon";
        }

        private int dmg;//damage
        private int manaCost;//if weapon is magic spell it has manacost
        private string dmgType;//type of damage

        public int Dmg
        {
            get
            {
                return dmg;
            }
            private set
            {
                if (value > 0)
                {
                    dmg = value;
                }
                else
                {
                    return;
                }
            }
        }
        public int ManaCost
        {
            get
            {
                return manaCost;
            }
            protected set
            {
                if (dmgType == "physical")
                {
                    manaCost = 0;
                    return;
                }
                else if (value >= 0)
                    manaCost = value;
            }
        }
        public string DmgType
        {
            get
            {
                return dmgType;
            }
            private set
            {
                if (value == "physical" || value == "magical")
                    dmgType = value;
                else
                    return;
            }
        }
        public override string Slot
        {
            get
            {
                return slot;
            }
            protected set
            {
                slot = "weapon";
            }
        }//should be in weapon slot so we force it
        public override void ItemStats()
        {
            MainGame.Say(name + "\n", MainGame.GetColor(rareness), 25);
            MainGame.Say(desc + "\n", 25);
            MainGame.Say("Damage type: " + DmgType + "\n", 25);
            MainGame.Say("Damage: " + Dmg + "\n", 25);
            if (DmgType == "magical")
                MainGame.Say("Manacost: " + ManaCost + "\n", 25);
        }//list item stats
        public override void ShortStats()
        {
            MainGame.Say(MainGame.FirstUpper(Slot) + ": ", 25);
            MainGame.Say(Name, MainGame.GetColor(Rareness), 25);
            if(dmgType == "phyical")
                MainGame.Say(". Damage type: " + dmgType, ConsoleColor.Red, 25);
            else if (dmgType == "magical")
                MainGame.Say(". Damage type: " + dmgType, ConsoleColor.Blue, 25);
            MainGame.Say(". Damage: " + dmg + "\n", 25);
        }
    }
}
