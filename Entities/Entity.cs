using System;

namespace Mysterious_Dungeon.Entities
{
    class Entity
    {
        public Entity()
        {
            return;
        }
        public Entity(string name, int dmg, int hp, int lvl, string desc)
        {
            Name = name;
            Damage = dmg;
            Hp = hp;
            Desc = desc;
            Lvl = lvl;
            return;
        }
        private string name;
        protected string desc;//description
        private int damage;
        private int lvl;//level
        private int hp;
        public string Name
        {
            get
            {
                return name;
            }
            protected set
            {
                if (value == null || String.IsNullOrWhiteSpace(value) || value == "\0" || value == "")
                    return;
                else
                    name = value;
            }

        }
        public int Hp
        {
            get
            {
                return hp;
            }
            protected set
            {
                if (value >= 0)
                    hp = value;
                else
                    hp = 0;
            }
        }
        public int Lvl
        {
            get
            {
                return lvl;
            }
            protected set
            {
                if (value >= 0)
                    lvl = value;
            }
        }
        public int Damage
        {
            get
            {
                return damage;
            }
            protected set
            {
                if (value >= 0)
                    damage = value;
            }
        }
        public string Desc
        {
            get
            {
                return desc;
            }
            protected set
            {
                if (value == "\0" || value == "" || value == null || String.IsNullOrWhiteSpace(value))
                    return;
                else
                    desc = value;
            }
        }

        public void EntityStat()
        {
            MainGame.Say("Level: " + lvl + "\n", 25);
            MainGame.Say(name + "\n", ConsoleColor.DarkRed, 25);
            MainGame.Say(desc + "\n", 25);
            MainGame.Say("Deals " + damage + " damage\n", 25);
            MainGame.Say("Has " + hp + " hp\n", 25);
        }//listin entity stats
        public void TakeDamage(int dmg)
        {
            Hp -= dmg;
        }//recieve damage
    }
}
