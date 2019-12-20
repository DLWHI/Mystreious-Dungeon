using System;

namespace Mysterious_Dungeon.Items
{
    class Armor : Item
    {
        public Armor()
        {
            return;
        }
        public Armor(string slot)
        {
            Slot = slot;
        }//only slot constructor
        public Armor(string name, int protection, string slot, string rareness, string desc)
        {
            Name = name;
            Rareness = rareness;
            Protection = protection;
            Slot = slot;
            Desc = desc;
        }//full constructor
        protected int protection;//amount of armor pts added to player
        public int Protection
        {
            get
            {
                return protection;
            }
            set
            {
                if (value >= 0)
                    protection = value;
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
                if (value == null || String.IsNullOrWhiteSpace(value) || value == "\0" || value == "")
                    return;
                else if (value == "head" || value == "chest" || value == "legs")
                    slot = value;
            }
        }
        public override void ItemStats()
        {
            MainGame.Say(name + "\n", MainGame.GetColor(rareness), 25);
            MainGame.Say(desc + "\n", 25);
            MainGame.Say("Protection: " + Protection + "\n", 25);
            MainGame.Say("Slot: " + Slot + "\n", 25);
        }//list stats of item
        public override void ShortStats()
        {
            MainGame.Say(MainGame.FirstUpper(Slot) + ": ", 25);
            MainGame.Say(Name, MainGame.GetColor(Rareness), 25);
            MainGame.Say(". Protection: " + protection, 25);
        }
    }
}