using System;
using System.Collections.Generic;
using Mysterious_Dungeon.Items;
using Mysterious_Dungeon.Entities;

namespace Mysterious_Dungeon.Rooms
{
    class Room
    {
        public Room()
        {
            return;
        }

        public Room(string name, int LootCount, int EntityCount, bool IsDouble)
        {
            Name = name;
            Loot = new List<Item>(LootCount);
            Entities = new List<Entity>(EntityCount);
            if (IsDouble)
                SecretRoom = new Room();
        }

        protected string name;
        public List<Item> Loot { get; protected set; }//loot list
        public List<Entity> Entities { get; protected set; }//entity list
        public Room SecretRoom { get; protected set; }//if room has secret one(unused)
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
                if (value == "Mimic")
                {
                    Random rnd = new Random();
                    if (rnd.Next(0, 100) > 45 && rnd.Next(0, 100) < 55)
                        name = "Pretty girl";
                }
            }
        }

        public void GenerateLoot(int roomNum)
        {
            int i, chance;
            Random rnd = new Random();
            for (i = 0; i < Loot.Capacity; i++)
            {
                chance = rnd.Next(0, 100);
                if (chance >= 45 && chance <= 45 + roomNum && roomNum > 6)//1+roomNum% chance of purple rareness. generates from room 7
                    Loot.Add(MainGame.GetRandomItem("purple"));
                else if (chance > 24 - roomNum / 4 && chance < 29 + roomNum / 2 && roomNum > 4)//4+roomNum% chance of blue rareness. generates from room 5
                    Loot.Add(MainGame.GetRandomItem("blue"));
                else if (chance < 97 && chance < 66)//30% chance of green rareness.
                    Loot.Add(MainGame.GetRandomItem("green"));
                else//others white
                    Loot.Add(MainGame.GetRandomItem("white"));
            }
        }//loot generation
        public void GenerateEntity(int roomNum)
        {
            int i, chance;
            Random rnd = new Random();
            for (i = 0; i < Entities.Capacity; i++)
            {
                Entity enemy;
                chance = rnd.Next(0, 100);
                if (chance == 1)//1% chance of 5 lvl
                    enemy = MainGame.GetRandomEntity(5);
                else if (chance >= 45 && chance <= 45 + roomNum && roomNum > 11)//1+room number% chance of 4 lvl. generates from room 12
                    enemy = MainGame.GetRandomEntity(4);
                else if (chance > 24 - roomNum / 4 && chance < 29 + roomNum / 2 && roomNum > 6)//4+room number% chance of 3 lvl. generates from room 7
                    enemy = MainGame.GetRandomEntity(3);
                else if (chance < 100 && chance < 69)//30% chance of 2 lvl
                    enemy = MainGame.GetRandomEntity(2);
                else//others 1 lvl
                    enemy = MainGame.GetRandomEntity(1);
                //if we just pass entity as ready object there will be 2 same enemy both taking damage at one time
                //so we making one more object and add it
                Entities.Add(new Entity(enemy.Name, enemy.Damage, enemy.Hp, enemy.Lvl, enemy.Desc));
            }
        }//entity generation
        public void EraseNulls()
        {
            for (int i = 0; i < Loot.Count; i++)
            {
                if (Loot[i] == null)
                {
                    Loot.RemoveAt(i);
                    i--;//shift back in case of remove
                }
            }
        }//re-ordering loot to erase nulls
        public void ListItems()
        {
            int c = 0;
            if (Loot.Count != 0)
            {
                MainGame.Say("You found loot!\n", 25);
                MainGame.Say("Look what you've dug up: \n", 25);
                foreach (Item item in Loot)
                {
                    c++;
                    MainGame.Say(c + ". ", 25);
                    MainGame.Say(item.Name + "\n", MainGame.GetColor(item.Rareness), 25);
                }
            }
            else
                MainGame.Say("There's no loot in this room\n", 25);
        }//listing loot
        public void ListEntities()
        {
            int c = 0;
            foreach (Entity enemy in Entities)
            {
                c++;
                MainGame.Say(c + ". ", 25);
                MainGame.Say(enemy.Name + "\n", ConsoleColor.DarkRed, 25);
            }
        }//listing entities
        public int TotalDamage()
        {
            int dmg = 0;
            foreach (Entity enemy in Entities)
                dmg += enemy.Damage;
            return dmg;
        }//counting total damage of all entities in room
        public char Control(int maxSlots)
        {
            char pressedNum;
            int num = 0;
            int digitCount = 0;
            do
            {
                pressedNum = Console.ReadKey(true).KeyChar;
                if (pressedNum == 8 && digitCount != 0)
                {
                    digitCount--;
                    num /= 10;
                    Console.Write("\b \b");
                }
                else if (pressedNum - 48 >= 0 && pressedNum - 48 < 10)
                {
                    if ((maxSlots - pressedNum + 48) >= num)
                    {
                        digitCount++;
                        Console.Write(pressedNum);
                        num = num * 10 + pressedNum - 48;
                    }
                }
                else if (pressedNum == 'a' || pressedNum == 'A')
                    return 'a';
            } while (pressedNum != 13);
            return Convert.ToChar(num);
        }//entity num input control, works as InventoryControl() in player class
    }
}