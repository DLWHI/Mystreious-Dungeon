namespace Mysterious_Dungeon.Items
{
    class Consumable : Item
    {
        public Consumable()
        {
            return;
        }
        public Consumable(string name, int hpRest, int manaRest, string rareness, string desc)
        {
            Name = name;
            HpRest = hpRest;
            ManaRest = manaRest;
            Rareness = rareness;
            Desc = desc;
        }

        private int hpRest;//amount of restoring hp
        private int manaRest;//amount of restoring mana

        public int HpRest
        {
            get
            {
                return hpRest;
            }
            protected set
            {
                if (value >= 0)
                    hpRest = value;
            }
        }
        public int ManaRest
        {
            get
            {
                return manaRest;
            }
            protected set
            {
                if (value >= 0)
                    manaRest = value;
            }
        }

        public override void ItemStats() 
        {
            MainGame.Say(name + "\n", MainGame.GetColor(rareness), 25);
            MainGame.Say(desc + "\n", 25);
        }//all consumable stas are in description
        public override void ShortStats(){ }
    }
}
