using System;
using System.Collections.Generic;
using Mysterious_Dungeon.Items;
using Mysterious_Dungeon.Entities;
using ExtendedConsoleInput;

namespace Mysterious_Dungeon.Rooms
{
    class Player
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value) || value == "\0" || value == "\n" || value == null)
                    return;
                else
                    name = value;
            }
        }

        //sail stats
        private int strength;
        private int agility;
        private int intellect;
        //stats
        private int dmg;
        private int magicDmg;
        private int hp;
        private int mana;
        private int armor;
        private int hpRegen;
        private int manaRegen;
        private int maxHp;
        private int maxMana;
        public int Hp
        {
            get
            {
                return hp;
            }
            private set
            {
                hp = value;
                if (value > maxHp)
                    hp = maxHp;
                if (value < 0)
                    hp = 0;
            }
        }
        public int Mana
        {
            get
            {
                return mana;
            }
            private set
            {
                mana = value;
                if (value > maxMana)
                    mana = maxMana;
                if (value < 0)
                    mana = 0;
            }
        }
        //base stats
        private const int baseDmg = 1;
        private const int baseMagicDmg = 0;
        private const int baseHp = 100;
        private const int baseMana = 10;
        private const int baseArmor = 0;
        private const int baseHpRegen = 10;
        private const int baseManaRegen = 3;
        private const int maxStat = 9;
        //inv
        private List<Item> inventory = new List<Item>(15);
        private Dictionary<string, Item> slots = new Dictionary<string, Item>(4);
        public Room currentRoom { get; set; }
        public bool InventoryIsFull { get; private set; }
        //----------------------------------NAME----------------------------------------
        public void GetName()//name check
        {
            MainGame.Say("\n-So what you're callin yourself?\n", ConsoleColor.Yellow, 25);
            do
            {
                name = StrInput.CleanInput();
                if (string.IsNullOrWhiteSpace(name))
                    MainGame.Say("-Speak louder, I can't hear!\n", ConsoleColor.Yellow, 25);
            } while (string.IsNullOrWhiteSpace(name));
            name = name.Trim();//deleting space
        }
        //----------------------------------STATS---------------------------------------
        public void GetStats()//askin about stats
        {
            int pts = 15;
            MainGame.Say("-A'right, " + name + ", now I wanna test your skills, wanna se what you're capable of.\n", ConsoleColor.Yellow, 25);
            MainGame.Say("You have 15 point that you can spend in 3 stats: ", 25);
            MainGame.Say("strength", ConsoleColor.Red, 25);
            MainGame.Say(", ", 25);
            MainGame.Say("agility", ConsoleColor.Green, 25);
            MainGame.Say(" and ", 25);
            MainGame.Say("intellect", ConsoleColor.Blue, 25);
            MainGame.Say(".\n", 25);
            MainGame.Say("Each stat affects some of your stats and can maximum have 9 points.\n\n", 25);
            StatField(ref strength, ref pts, ConsoleColor.Red);
            StatField(ref agility, ref pts, ConsoleColor.Green);
            StatField(ref intellect, ref pts, ConsoleColor.Blue);
            StatCalc();
            do
            {
                MainGame.Say("\nSo you have:\n", 25);
                StatOut();
                if (pts != 0)
                    MainGame.Say("You have " + pts + " unused points. ", 25);
                MainGame.Say("Are you sure you want to continue?(y/n)", 25);
                if (StrInput.YNInput())
                    return;
                else
                {
                    Console.Write("\n");
                    StatField(ref intellect, ref pts, ConsoleColor.Blue);
                    StatCalc();
                }
            } while (true);
        }
        private void StatCalc()//stat calc
        {
            //dmg
            dmg = baseDmg + strength / 4 + agility / 5;
            magicDmg = baseMagicDmg + intellect / 4;
            //armor
            armor = baseArmor + agility / 3;
            //hp/mana/regen
            hp = baseHp + (strength * 55 + 9) / 10;
            mana = baseMana + (intellect * 10);
            hpRegen = baseHpRegen + (strength * 18 + 9) / 10;
            manaRegen = baseManaRegen + (intellect * 52 + 9) / 10;
            maxHp = hp;
            maxMana = mana;
            //weap
            if (slots.ContainsKey("weapon"))
                if ((slots["weapon"] as Weapon).DmgType == "magical")
                    magicDmg += (slots["weapon"] as Weapon).Dmg;
                else if ((slots["weapon"] as Weapon).DmgType == "physical")
                    dmg += (slots["weapon"] as Weapon).Dmg;
            if (slots.ContainsKey("head"))
                armor += (slots["head"] as Armor).Protection;
            if (slots.ContainsKey("chest"))
                armor += (slots["chest"] as Armor).Protection;
            if (slots.ContainsKey("legs"))
                armor += (slots["legs"] as Armor).Protection;
        }
        private void StatField(ref int stat, ref int pts, ConsoleColor color)//specail field for stat choose
        {
            //specified stat is being submitted as ref so we could change it directly as we need
            ConsoleKey pressedKey;
            WriteField(pts, color);
            do
            {
                MainGame.Say(stat.ToString(), 25);
                pressedKey = Console.ReadKey(true).Key;
                Console.Write("\b");
                if (pressedKey == ConsoleKey.UpArrow && stat < maxStat && pts != 0)
                {
                    stat++;
                    pts--;
                    Console.Write(" \b");
                    continue;
                }
                else if (pressedKey == ConsoleKey.DownArrow && stat > 0)
                {
                    stat--;
                    pts++;
                    Console.Write(" \b");
                    continue;
                }
                else if (pressedKey == ConsoleKey.Backspace && color != ConsoleColor.Red)
                {
                    Console.Write('\n');
                    if (color == ConsoleColor.Green)
                        StatField(ref strength, ref pts, ConsoleColor.Red);
                    else if (color == ConsoleColor.Blue)
                        StatField(ref agility, ref pts, ConsoleColor.Green);
                }
                else if (pressedKey == ConsoleKey.Enter)
                    break;
                else
                    continue;
                WriteField(pts, color);
            } while (pressedKey != ConsoleKey.Enter);
            Console.Write('\n');
        }
        private void WriteField(int pts, ConsoleColor color)//just some writing
        {
            MainGame.Say("Remaining points: " + pts + "\n", 25);
            switch (color)
            {
                case ConsoleColor.Red:
                    MainGame.Say("Strength", color, 25);
                    MainGame.Say(", affects your hp and physical damage: ", 25);
                    break;
                case ConsoleColor.Green:
                    MainGame.Say("Agility", color, 25);
                    MainGame.Say(", affects your armor and slightly affects your physical damage: ", 25);
                    break;
                case ConsoleColor.Blue:
                    MainGame.Say("Intellect", color, 25);
                    MainGame.Say(", affects your mana and magical damage: ", 25);
                    break;
            }
        }
        private void StatOut()//same writing
        {
            MainGame.Say("Strength", ConsoleColor.Red, 25);
            MainGame.Say(": " + strength + ". You've got ", 25);
            MainGame.Say(hp + " hp", ConsoleColor.Red, 25);
            MainGame.Say(".\n", 25);

            MainGame.Say("Agility", ConsoleColor.Green, 25);
            MainGame.Say(": " + agility + ". You've got ", 25);
            MainGame.Say(armor + " armor", ConsoleColor.Green, 25);
            MainGame.Say(".\n", 25);

            MainGame.Say("Intellect", ConsoleColor.Blue, 25);
            MainGame.Say(": " + intellect + ". You've got ", 25);
            MainGame.Say(mana + " mana", ConsoleColor.Blue, 25);
            MainGame.Say(".\n", 25);

            MainGame.Say("Total damage:\n", 25);
            MainGame.Say("\tPhysical: ", ConsoleColor.Red, 25);
            MainGame.Say(dmg + "\n", 25);
            MainGame.Say("\tMagical: ", ConsoleColor.Blue, 25);
            MainGame.Say(magicDmg + "\n", 25);
        }
        public string MaxStat()//returns name of maximal stat, if there's more than 1 of them return the first one
        {
            if (Math.Max(strength, Math.Max(agility, intellect)) == strength)
                return "strength";
            else if (Math.Max(strength, Math.Max(agility, intellect)) == agility)
                return "agility";
            else if (Math.Max(strength, Math.Max(agility, intellect)) == intellect)
                return "intellect";
            return null;
        }
        //---------------------------INVENTORY-AND ITEM-----------------------------------
        public void OpenInventory()
        {
            int c;
            ConsoleKey pressedKey;
        st:
            if (inventory.Count == 0)
            {
                MainGame.Say("Your inventory is empty\n", 25);
                return;
            }
            c = 0;
            foreach (Item item in inventory)
            {
                c++;
                MainGame.Say(c + ". " + item.Name + "\n", MainGame.GetColor(item.Rareness), 25);
            }
            Condition();
            MainGame.Say("Which one you want to choose?\nInput num of item, Press 'C' to close inventory.\n", 25);
            do
            {
                c = InvControl(inventory.Count);
                if (c == 'c')
                    return;
                else if (c == 'b')
                    goto st;
            } while (c < 0 || c >= inventory.Count);
            inventory[c].ItemStats();
            MainGame.Say("Press 'E' to use/equip item, 'T' to throw item away,'B' to choose another item.", 25);
            do
            {
                pressedKey = Console.ReadKey().Key;
                Console.Write("\n");
                if (pressedKey == ConsoleKey.E)
                    TreatItem(c);
                else if (pressedKey == ConsoleKey.T)
                    ThrowItem(c);
                else if (pressedKey == ConsoleKey.Backspace)
                    goto st;
                else
                    continue;
                goto st;
            } while (true);
        }
        private int InvControl(int maxSlots)
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
                else if (pressedNum - 48 >= 1 && pressedNum - 48 < 10)
                {
                    if ((maxSlots - pressedNum + 48) / 10 >= num)
                    {
                        digitCount++;
                        Console.Write(pressedNum);
                        num = num * 10 + pressedNum - 48;
                    }
                }
                else if (pressedNum == 'c' || pressedNum == 'C')
                    return 'c';
                else if (pressedNum == 'b' || pressedNum == 'B')
                    return 'b';
            } while (pressedNum != 13);
            Console.Write("\n");
            return num - 1;
        }
        private void RandomInv()//random inv for debug issues
        {
            for (int i = 0; i < 15; i++)
                inventory.Add(MainGame.GetRandomItem("blue"));
        }
        private void TreatItem(int ind)//calls specified func for specified item
        {
            if (inventory[ind] is Weapon || inventory[ind] is Armor)
                Equip(ind);
            else if (inventory[ind] is Consumable)
                Use(ind);
            InventoryIsFull = false;
        }
        private void Equip(int ind)//equpping item
        {
            if (slots.ContainsKey(inventory[ind].Slot))//if slot already has item we ask should we replace it
            {
                MainGame.Say("This slot is occupied. Replace?(y/n)", 25);
                if (StrInput.YNInput())
                {//replacin
                    Console.Write("\n");
                    Item tmp = inventory[ind];
                    inventory[ind] = slots[inventory[ind].Slot];
                    slots[inventory[ind].Slot] = tmp;
                    MainGame.Say("You replaced ", 25);
                    MainGame.Say(inventory[ind].Name, MainGame.GetColor(inventory[ind].Rareness), 25); ;
                    MainGame.Say(" with ", 25);
                    MainGame.Say(slots[inventory[ind].Slot].Name + ".\n", MainGame.GetColor(slots[inventory[ind].Slot].Rareness), 25);
                }
                else//ok den
                {
                    Console.Write("\n");
                    return;
                }
            }
            else//or we can just equip it
            {
                slots[inventory[ind].Slot] = inventory[ind];
                MainGame.Say("You equipped ", 25);
                MainGame.Say(slots[inventory[ind].Slot].Name + ".\n", MainGame.GetColor(slots[inventory[ind].Slot].Rareness), 25);
                inventory.RemoveAt(ind);
            }
            StatCalc();//recalc stats
        }
        private void Unequip(string slot)//equpping item
        {
            if (InventoryIsFull)
            {
                MainGame.Say("You had no space in your inventory, so you threw ", 25);
                MainGame.Say(slots[slot].Name + "\n", MainGame.GetColor(slots[slot].Rareness), 25);
                currentRoom.Loot.Add(slots[slot]);
            }
            else
            {
                MainGame.Say("You unequipped ", 25);
                MainGame.Say(slots[slot].Name + "\n", MainGame.GetColor(slots[slot].Rareness), 25);
                inventory.Add(slots[slot]);
            }
            slots.Remove(slot);
            StatCalc();//recalc stats
        }
        private void Use(int ind)//using potions
        {
            Consumable item = inventory[ind] as Consumable;
            Hp += item.HpRest;
            Mana += item.ManaRest;
            MainGame.Say("You restored ", 25);
            if (item.HpRest != 0)
                MainGame.Say(item.HpRest + " hp.\n", ConsoleColor.Red, 25);
            else if (item.ManaRest != 0)
                MainGame.Say(item.ManaRest + " mana.\n", ConsoleColor.Blue, 25);
            inventory.RemoveAt(ind);
        }
        private void ThrowItem(int ind)//throw items away to room loot
        {
            MainGame.Say("You threw ", 25);
            MainGame.Say(inventory[ind].Name + ".\n", 25);
            if (currentRoom != null)//if we haven't stepped in dung
                currentRoom.Loot.Add(inventory[ind]);
            inventory.RemoveAt(ind);
            InventoryIsFull = false;
        }
        public void GetItem(Item item)
        {
            if (!InventoryIsFull)
            {
                inventory.Add(item);
                MainGame.Say("You obtined ", 25);
                MainGame.Say(item.Name + "\n", MainGame.GetColor(item.Rareness), 25);
                if (inventory.Count == 15)
                    InventoryIsFull = true;
            }
            else
                MainGame.Say("Your inventory is full\n", 25);
        }
        public void OpenEquipment()
        {
            string slot;
            int c;
        st:
            if (slots.ContainsKey("head"))
                slots["head"].ShortStats();
            if (slots.ContainsKey("chest"))
                slots["chest"].ShortStats();
            if (slots.ContainsKey("legs"))
                slots["legs"].ShortStats();
            if (slots.ContainsKey("weapon"))
                slots["weapon"].ShortStats();
            Condition();
            slot = StrInput.NoDigitSpace();
            slots[slot].ItemStats();
            MainGame.Say("Press 'E' to unequip slot, 'B' to choose another slot, 'C' to close equipment", 25);
            do
            {
                c = InvControl(0);
                if (c == 'e')
                    Unequip(slot);
                else if (c == 'b')
                    goto st;
                else if (c == 'c')
                    return;
            } while (c != 'e');
            goto st;
        }
        //---------------------------------BATTLE----------------------------------------
        public void Regen()
        {
            Hp += hpRegen;
            Mana += manaRegen;
            MainGame.Say("You restored ", 25);
            MainGame.Say(hpRegen + " hp ", ConsoleColor.Red, 25);
            MainGame.Say("and ", 25);
            MainGame.Say(manaRegen + " mana\n", ConsoleColor.Blue, 25);
        }
        public void TakeDamage(int dmg)
         {
            if (dmg < armor)
                dmg = 0;
            else
                dmg -= armor;
            Hp -= dmg;
            MainGame.Say("You recieved ", 25);
            MainGame.Say(dmg + " damage.\n", ConsoleColor.DarkRed, 25);
            if(hp == 0)
            {
                MainGame.Say("----------YOU DIED----------\n", ConsoleColor.DarkRed, 25);
                MainGame.Say("Restart?(y/n)\n", 25);
                if (StrInput.YNInput())
                    MainGame.Restart = true;
                else
                    MainGame.Restart = false;
            }
        }
        public int Attack(Entity enemy)//returning summary damage based on equipped weapon and current mana
                                       //inputting enemy object just to make a message
        {
            if (slots.ContainsKey("weapon"))
            {
                Weapon weapon = slots["weapon"] as Weapon;
                if (weapon.DmgType == "magical" && weapon.ManaCost <= mana)//if we have magic spell and enough mana
                {
                    MainGame.Say("You dealt ", 25);
                    MainGame.Say(magicDmg + " magical damage", ConsoleColor.Blue, 25);
                    MainGame.Say(" to ", 25);
                    MainGame.Say(enemy.Name + ".\n", ConsoleColor.DarkRed, 25);
                    return magicDmg;
                }
                else if (weapon.ManaCost > mana)//if we have magic spell but not enough mana
                {
                    MainGame.Say("You had not enough mana to cast a spell so strike ", 25);
                    MainGame.Say(enemy.Name, ConsoleColor.DarkRed, 25);
                    MainGame.Say(" with bare hands\n", 25);
                    MainGame.Say("You dealt ", 25);
                    MainGame.Say(dmg + " physical damage", ConsoleColor.Red, 25);
                    MainGame.Say(" to ", 25);
                    MainGame.Say(enemy.Name + ".\n", ConsoleColor.DarkRed, 25);
                    return dmg;
                }
                else if (weapon.DmgType == "physical")//if we ARE BARBARIAN
                {
                    MainGame.Say("You dealt ", 25);
                    MainGame.Say(dmg + " physical damage", ConsoleColor.Red, 25);
                    MainGame.Say(" to ", 25);
                    MainGame.Say(enemy.Name + ".\n", ConsoleColor.DarkRed, 25);
                    return dmg;
                }
                return 0;
            }
            else
            {
                MainGame.Say("You dealt ", 25);
                MainGame.Say(dmg + " physical damage", ConsoleColor.Red, 25);
                MainGame.Say(" to ", 25);
                MainGame.Say(enemy.Name + ".\n", ConsoleColor.DarkRed, 25);
                return dmg;
            }
                
        }
        public void Condition()
        {
            MainGame.Say("You have ", 25);
            MainGame.Say(hp + " hp", ConsoleColor.Red, 25);
            MainGame.Say(" and ", 25);
            MainGame.Say(mana + " mana.\n", ConsoleColor.Blue, 25);
        }
    }
}