using System;
using System.Collections.Generic;
using Mysterious_Dungeon.Rooms;
using Mysterious_Dungeon.Items;
using Mysterious_Dungeon.Entities;
using ExtendedConsoleInput;

namespace Mysterious_Dungeon
{
    class MainGame
    {
        private static List<Item> itemListWhite = new List<Item>();
        private static List<Item> itemListGreen = new List<Item>();
        private static List<Item> itemListBlue = new List<Item>();
        private static List<Item> itemListPurple = new List<Item>();
        private static List<Entity> entityList = new List<Entity>();
        private static List<string> rNameList;
        private static List<string> rSpecialNameList;
        public static bool Restart { get; set; }//restart flag

        static void Main(string[] args)
        {
            do
            {
                ItemInit();
                EntityInit();
                List<Room> dungeon = new List<Room>();
                DungeonFill(dungeon);
                Player player = new Player();
                player.GetName();
                player.GetStats();
                Say("\n\n-Alright, I have some stuff up here, I think you may need this.\n", ConsoleColor.Yellow, 25);
                //gives player item based on his stats
                if (player.MaxStat() == "strength")
                    player.GetItem(itemListWhite[0]);
                else if (player.MaxStat() == "agility")
                    player.GetItem(itemListWhite[2]);
                else if (player.MaxStat() == "intellect")
                    player.GetItem(itemListWhite[1]);
                //some text+askin if player wants to pen inv
                Say("You come closer to dungeon enteracne. You some mysterious feeling goes through your body.\n", ConsoleColor.White, 25);
                Say("You can open your inventory before entering the dungeon and see what things overseer gave you.\n", ConsoleColor.White, 25);
                MainGame.Say("Would you like to open your inventory?(y/n)", 25);
                if (StrInput.YNInput())
                {
                    Console.Write("\n");
                    player.OpenInventory();
                }
                Say("\nYou enter the dungeon...\n\n\n", 25);
                Say("You encounter your first room!\n", 25);
                //proccecing battle+lotting procces for every room
                foreach (Room room in dungeon)
                {
                    player.currentRoom = room;
                    Say("You enter " + room.Name + "\n", 25);
                    //Entity interaction
                    Battle(player);
                    //death and restart check
                    if (Restart && player.Hp == 0)
                        continue;
                    else if (!Restart && player.Hp == 0)
                        break;
                    //Loot interaction
                    LootRoom(player);
                    //Operations before going to next room 
                    Say("You picked everything you could take and you're ready to go.\n", 25);
                    Say("You can open your inventory(I), equipment(E), look back to room(L) or move to the next room(N).\n", 25);
                    do
                    {
                        char ind;
                        ind = Console.ReadKey(true).KeyChar;
                        if (ind == 'i' || ind == 'I')//open inventory
                            player.OpenInventory();
                        else if (ind == 'e' || ind == 'E')//open equipment
                            player.OpenEquipment();
                        else if (ind == 'l' || ind == 'L')//looking back if player forgot smt/threw thig that he needed
                            LootRoom(player);
                        else if (ind == 'n' || ind == 'N')//move forward
                            break;
                        else//no reaction
                            continue;
                        Say("You picked everything you could take and you're ready to go\n", 25);
                        Say("You can open your inventory(I), look back to room(L) or move to the next room(N)\n", 25);
                    } while (true);
                    player.Regen();//hp+mana regen
                }
                //ending
                Say("\n\nYou walk through a dungeon. Suddenly you feel wind blowing.\n", 25);
                Say("Exit should be nearby...\n", 25);
                Say("You're looking for exit\n", 25);
                Say("You see the light\n", 25);
                Say("Dungeon ends...\n", 25);
                Say("\n\nYou made it!\n", 25);
                if (StrInput.YNInput())
                    Restart = true;
                else
                    Restart = false;
            } while (Restart);
            Console.ReadKey();
        }
        //System.Threading.Thread.Sleep(speed); makes sys sleep for future purposes, time in ms
        //Random.Next(i1, i2) diapasone [i1, i2);
        public static void Battle(Player player)
        {
            Room room = player.currentRoom;//setting players current room, usefull
            if (room.Entities.Count != 0)
            {
                Say("You encounter enemy!\n", 25);
                room.ListEntities();
                do
                {
                    char ind;
                    Say("Which one to choose?", 25);
                    ind = room.Control(room.Entities.Count);
                    Console.Write("\n");
                    if (ind > 0 && ind <= room.Entities.Count)
                        room.Entities[ind - 1].EntityStat();
                    else
                        continue;
                    player.Condition();//condition of player: current mana+hp
                    Say("Press 'A' to attack him, 'I' to open inventory, 'Backspace' to choose another one.", 25);
                    char move;
                    do
                    {//i think everything is pretty obvious but anyway
                        move = Console.ReadKey(true).KeyChar;
                        Console.Write("\n");
                        if (move == 'a' || move == 'A')//proccesing attack
                        {
                            int dmg = player.Attack(room.Entities[ind - 1]);
                            room.Entities[ind - 1].TakeDamage(dmg);
                            if (room.Entities[ind - 1].Hp == 0)//die procces
                            {
                                Say(room.Entities[ind - 1].Name + " dies\n", ConsoleColor.DarkRed, 25);
                                room.Entities.RemoveAt(ind - 1);
                            }
                        }
                        else if (move == 'i' || move == 'I')//opening inventory counts as move so after closing player will recieve damage
                            player.OpenInventory();
                    } while (move != 8 && move != 'i' && move != 'I' && move != 'a' && move != 'A');
                    if (move == 8)//choosing another one
                    {
                        room.ListEntities();
                        continue;
                    }
                    player.TakeDamage(room.TotalDamage());
                    if (player.Hp == 0)
                        return;
                    room.ListEntities();
                } while (room.Entities.Count != 0);
            }
        }
        public static void LootRoom(Player player)
        {
            Room room = player.currentRoom;
            Say("You search room for loot\n", 25);
            room.ListItems();
            if (player.InventoryIsFull)
                MainGame.Say("Your inventory is full\n", 25);
            else if (room.Loot.Count != 0 && !player.InventoryIsFull)//obvious cheking if room has looot and player's pockets aren't full(just because WE CAN check)
            {
                string choice;
                char[] items;//we will need them
                MainGame.Say("Which one you want to pick?(Write nums of items through space, 'A' to pick all, 'Enter' to move on)\n", 25);
                choice = StrInput.CleanInput();//player writes item nums in string through space(but honestly we don't beacuse we wiil read as single num them anyway)
                items = ParseStringDigit(choice);//taking only digits from string that was submitted and putting them in char array
                Console.Write("\n");
                if (choice == "" || items.Length == 0) { }//if player choses nothing he inputs emty string so we just move forward
                else if (items[0] == 'A' || items[0] == 'a')//if player wants to take all he submits a letter A in front of string and we procced
                {
                    foreach (Item item in room.Loot)
                    {
                        player.GetItem(item);
                        if (player.InventoryIsFull)
                            break;
                    }
                    room.Loot.RemoveRange(0, room.Loot.Count);
                }
                else//else we just watchin what player submitted
                {
                    foreach (char ch in items)
                    {
                        if (ch <= 48 || ch - 48 > room.Loot.Count || ch == 'a' || ch == 'A')
                            continue;
                        if (room.Loot[ch - 49] != null)
                        {
                            player.GetItem(room.Loot[ch - 49]);
                            room.Loot[ch - 49] = null;//we make item place in list null so order in list doesn't change
                        }
                    }
                }
                room.EraseNulls();//erasing nulls because it may disrupt us
            }
        }
        public static ConsoleColor GetColor(string color)//getting console color by string
        {
            if (color == "white")
                return ConsoleColor.White;
            else if (color == "green")
                return ConsoleColor.Green;
            else if (color == "blue")
                return ConsoleColor.Blue;
            else if (color == "purple")
                return ConsoleColor.Magenta;
            return ConsoleColor.White;
        }
        public static void Say(string msg, int speed)//special for our game output char by char
        {
            char[] msgAr = msg.ToCharArray();
            ConsoleKey pressedKey;
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < msgAr.Length; i++)
            {
                Console.Write(msgAr[i]);
                if (Console.KeyAvailable)
                {
                    pressedKey = Console.ReadKey(true).Key;
                    if (pressedKey == ConsoleKey.Spacebar || pressedKey == ConsoleKey.Enter)
                    {
                        i++;
                        for (; i < msgAr.Length; i++)
                            Console.Write(msgAr[i]);
                        break;
                    }
                }
                System.Threading.Thread.Sleep(speed);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static void Say(string msg, ConsoleColor color, int speed)//same thing with color
        {
            char[] msgAr = msg.ToCharArray();
            ConsoleKey pressedKey;
            Console.ForegroundColor = color;
            for (int i = 0; i < msgAr.Length; i++)
            {
                Console.Write(msgAr[i]);
                if (Console.KeyAvailable)
                {
                    pressedKey = Console.ReadKey(true).Key;
                    if (pressedKey == ConsoleKey.Spacebar || pressedKey == ConsoleKey.Enter)
                    {
                        i++;
                        for (; i < msgAr.Length; i++)
                            Console.Write(msgAr[i]);
                        break;
                    }
                }
                System.Threading.Thread.Sleep(speed);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        private static void DungeonFill(List<Room> dung)//filling our dungeon with rooms
        {
            rNameList = new List<string>() { "Mysterious Room", "Prison", "Destroyed Room", "Kitchen", "Laboratory", "Armory", "Living Room", "room with sign \"DANGER|DO NOT ENTER\"" };
            rSpecialNameList = new List<string>() { "Loot Room" };
            Random rnd = new Random();
            int roomCount = rnd.Next(16, 33);
            int chance;
            for (int i = 0; i < roomCount; i++)
            {
                chance = rnd.Next(0, 100);
                if (chance > 24 && chance < 29)//4% chance of loot room appearing
                    dung.Add(new Room(rSpecialNameList[0], rnd.Next(4, 6), rnd.Next(0, 4), Convert.ToBoolean(rnd.Next(0, 2))));
                else
                    dung.Add(new Room(rNameList[rnd.Next(0, rNameList.Count)], rnd.Next(0, 4), rnd.Next(0, 4), Convert.ToBoolean(rnd.Next(0, 2))));
                dung[i].GenerateLoot(i);
                dung[i].GenerateEntity(i);
            }
        }
        public static Item GetRandomItem(string rare)//to take random item with specified rareness
        {
            Random rnd = new Random();
            if (rare == "white")
                return itemListWhite[rnd.Next(0, itemListWhite.Count)];
            else if (rare == "green")
                return itemListGreen[rnd.Next(0, itemListGreen.Count)];
            else if (rare == "blue")
                return itemListBlue[rnd.Next(0, itemListBlue.Count)];
            else if (rare == "purple")
                return itemListPurple[rnd.Next(0, itemListPurple.Count)];
            return null;
        }
        public static Entity GetRandomEntity(int lvl)//to get random entity with specified level
        {
            Random rnd = new Random();
            int chance;
            do
            {
                chance = rnd.Next(0, entityList.Count);
            } while (entityList[chance].Lvl != lvl);
            return entityList[chance];
        }
        public static string FirstUpper(string str)//making first letter in string uppercase
        {
            char[] strAr = str.ToCharArray();//making char array of string and adding all changed chars to string again(like mergesort but without sort)
            for (int i = 0; i < strAr.Length; i++)
                if (strAr[i] >= 97 && strAr[i] <= 122)
                {
                    strAr[i] -= ' ';
                    break;
                }
            str = "";
            foreach (char ch in strAr)
                str += ch;
            return str;
        }
        public static char[] ParseStringDigit(string str)//parsing thing to digits
        {
            char[] chAr = str.ToCharArray();
            string resStr = "";
            foreach (char ch in chAr)
            {
                if ((ch == 32 || ch < 48 || ch > 57) && ch != 'a' && ch != 'A')
                    continue;
                resStr += ch;
            }
            return resStr.ToCharArray();
        }
        private static void ItemInit()//initializing items so we could use them
        {
            //new Weapon(string name, int damage, int manaCost, string rareness, string desc) - magical spell constructor
            // new Weapon(string name, int damage, string rareness, string desc) - physical damage weapon constructor
            //new Consumable(string name, int hpRest, int manaRest, string rareness)
            //new Armor(string name, int protection, string slot, string rareness, string desc)

            //white item list
            //0. Rusty Blade
            //1. Tome of Fireball
            //2. Long Stick
            //3. Rusty Axe
            //4. Tome of ice Spike
            //5. Health Potion
            //6. Mana Potion
            //7. Bucket
            //8. Rusted Armorplate
            //9. Fireproof Leggings
            itemListWhite.Add(new Weapon("Rusty Blade", 1, "white", "Old rusty blade. Swift nuff"));
            itemListWhite.Add(new Weapon("Tome of Fireball", 2, 8, "white", "Tome of fireball spell"));
            itemListWhite.Add(new Weapon("Long stick", 1, "white", "Old long stick probably belonged to some monach"));
            itemListWhite.Add(new Weapon("Rusty Axe", 2, "white", "This axe has seen more than one user in his lifetime"));
            itemListWhite.Add(new Weapon("Tome of Ice Spike", 3, 8, "white", "Tome of ice spike spell"));
            itemListWhite.Add(new Consumable("Health Potion", 25, 0, "white", "Restores 25 hp"));
            itemListWhite.Add(new Consumable("Mana Potion", 0, 30, "white", "Restores 30 mana"));
            itemListWhite.Add(new Armor("Bucket", 1, "head", "white", "Good ol' bucket"));
            itemListWhite.Add(new Armor("Rusted Armorplate", 2, "chest", "white", "This armor has seen a lot of adventures"));
            itemListWhite.Add(new Armor("Fireproof Leggings", 1, "legs", "white", "Your grandpa's old leggings"));
            //green item list
            //0. Tome of Electroball
            //1. Armature
            //2. Blade
            //3. Iron Mace
            //4. Greater Health Potion
            //5. Greater Mana Potion
            //6. Iron Helmet
            //7. Iron Armor
            //8. Iron Leg Protection
            itemListGreen.Add(new Weapon("Tome of Electroball", 5, 17, "green", "Tome of electroball spell"));
            itemListGreen.Add(new Weapon("Armature", 3, "green", "Hard iron armature that is gretaly suited to smash someone's head"));
            itemListGreen.Add(new Weapon("Blade", 4, "green", "Iron blade that was well-worn"));
            itemListGreen.Add(new Weapon("Iron Mace", 4, "green", "Mace that is prefectly suited to break bones"));
            itemListGreen.Add(new Consumable("Greater Health Potion", 45, 0, "green", "Restores 45 hp"));
            itemListGreen.Add(new Consumable("Greater Mana Potion", 0, 50, "green", "Restores 50 mana"));
            itemListGreen.Add(new Armor("Iron Helmet", 2, "head", "green", "Basic head protection"));
            itemListGreen.Add(new Armor("Iron Armor", 3, "chest", "green", "Heavy armor will protect you from damage"));
            itemListGreen.Add(new Armor("Iron Leg Protection", 2, "legs", "green", "Basic leg protection"));
            //blue item list
            //0. Battle Axe
            //1. Small Hammer
            //2. Staff of Fire
            //3. Force Staff
            //4. Knight Sword
            //5. Katana
            //6. Electric Field Scroll
            //7. Max Health Potion
            //8. Max Mana Potion
            //9. Horned Viking Helmet
            //10. Reinforced Leather Armor
            //11. Reinforced Leather Leggings
            itemListBlue.Add(new Weapon("Battle Axe", 5, "blue", "Big battle axe will cut your enemies in half"));
            itemListBlue.Add(new Weapon("Small Hammer", 5, "blue", "Quite balanced, heavy and good weapon for smashin'"));
            itemListBlue.Add(new Weapon("Staff of Fire", 8, 25, "blue", "Burns your enemies down like flamethrower"));
            itemListBlue.Add(new Weapon("Force Staff", 14, 30, "blue", "Creates forse that pulls your enemies away"));
            itemListBlue.Add(new Weapon("Knight Sword", 10, "blue", "Long sword that usually used by knights"));
            itemListBlue.Add(new Weapon("Katana", 9, "blue", "Swift sword of Japan samurai's"));
            itemListBlue.Add(new Weapon("Electic Field Scroll", 12, 20, "blue", "Creates electric field that doesn't let your enemies come close"));
            itemListBlue.Add(new Consumable("Max Health Potion", 150, 0, "blue", "Restores 150 hp"));
            itemListBlue.Add(new Consumable("Max Mana Potion", 0, 100, "blue", "Restores 100 mana"));
            itemListBlue.Add(new Armor("Horned Viking Helmet", 2, "head", "blue", "Arrrrrrrr!"));
            itemListBlue.Add(new Armor("Reinforced Leather Armor", 4, "chest", "blue", "Reinforced jacket made from leather of unknown creature"));
            itemListBlue.Add(new Armor("Reinforced Leather Leggings", 3, "legs", "blue", "Leggings made from leather of unknown creature"));
            //purple item list
            //0. SLEGEHAMMER
            //1. ShiftBlade
            //2. Paladin Sword
            //3. Time Edge
            //4. Staff of Light
            //5. Scroll of Electric Storm
            //6. Necronomicon
            //7. Igni Book
            //8. Staff of Mindus
            //9. Helm of The Enlightened
            //10. Armor Of God
            //11. Legs of Haste
            itemListPurple.Add(new Weapon("SLEGDEHAMMER", 36, "purple", "A'rigt, let's crush dis place up"));
            itemListPurple.Add(new Weapon("ShiftBlade", 30, "purple", "Elegance means proffesional"));
            itemListPurple.Add(new Weapon("Paladin Sword", 33, "purple", "May God have mercy upon you"));
            itemListPurple.Add(new Weapon("Time Egde", 96, "purple", "This is not yours.\nYou have no might to use that"));
            itemListPurple.Add(new Weapon("Staff of Light", 34, "purple", "May the light be with you!"));
            itemListPurple.Add(new Weapon("Scroll of Electric Storm", 35, 35, "purple", "Zip Zap"));
            itemListPurple.Add(new Weapon("Necronomicon", 44, 50, "purple", "Death shall fall upon this place"));
            itemListPurple.Add(new Weapon("Igni Book", 30, 35, "purple", "I'm on fire!"));
            itemListPurple.Add(new Weapon("Staff of Mindus", 100, 100, "purple", "This is not yours.\n You have no right to enter"));
            itemListPurple.Add(new Armor("Helm of The Enlightened", 4, "head", "purple", "You, you have been chosen"));
            itemListPurple.Add(new Armor("Armor Of God", 5, "chest", "purple", "Lord himself was wearing this"));
            itemListPurple.Add(new Armor("Legs of Haste", 4, "legs", "purple", "Mechanical leggings that gives more power to your legs"));
        }
        private static void EntityInit()//initializing entities so we could use them
        {
            //new Entity(string name, int dmg, int hp, string desc) - constructor
            //lvl 1
            //0. Big Rat
            //1. Skeleton
            //2. Kobold Worker
            entityList.Add(new Entity("Big Rat", 3, 6, 1, "Big distgusting rat"));
            entityList.Add(new Entity("Skeleton", 4, 4, 1, "Click-clack"));
            entityList.Add(new Entity("Kobold Worker", 4, 8, 1, "Dungeon kobold that is not pleased to see you!"));
            //lvl 2
            //3. Kobold Foreman
            //4. Newbie Bandit
            //5. Zombie
            //6. Slime
            entityList.Add(new Entity("Kobold Foreman", 6, 11, 2, "Foreman of kobold workers"));
            entityList.Add(new Entity("Newbie Bandit", 5, 15, 2, "Wrong life path"));
            entityList.Add(new Entity("Zombie", 6, 13, 2, "You know how to kill a zombie, c'mon"));
            entityList.Add(new Entity("Slime", 5, 4, 2, "Yikes"));
            //lvl 3
            //7. Angry Digger
            //8. Clay Golem
            //9. Zombie
            //10. Bandit
            //11. Skilled Bandit
            entityList.Add(new Entity("Angry Digger", 20, 25, 3, "He's not pleased to see you. Definetly"));
            entityList.Add(new Entity("Clay Golem", 40, 10, 3, "Damn he's big. Should be easy to dodge..."));
            entityList.Add(new Entity("Zombie", 10, 22, 3, "You know how to kill a zombie, c'mon"));
            entityList.Add(new Entity("Bandit", 23, 22, 3, "Stop rigth there, you criminal scum!"));
            entityList.Add(new Entity("Skilled Bandit", 26, 28, 3, "Tough one"));
            //lvl 4
            //12. Giant Spider
            //13. Bandit Marauder
            //14. Draugr
            //15. Mimic
            //16. Tripper
            //17. Stone Golem
            entityList.Add(new Entity("Giant Spider", 28, 32, 4, "Holy shit, now i fear spiders"));
            entityList.Add(new Entity("Bandit Marauder", 27, 30, 4, "This is getting harder..."));
            entityList.Add(new Entity("Draugr", 30, 30, 4, "Holy shit this thing's alive"));
            entityList.Add(new Entity("Mimic", 30, 50, 4, "IT'S A TRAP"));
            entityList.Add(new Entity("Tripper", 25, 32, 4, "GOTCHA ASS!"));
            entityList.Add(new Entity("Stone Golem", 25, 60, 4, "GO-LEM CRU-USH"));
            //lvl 5
            //18. Bandit Captain
            //19.Evil Spirit
            //20. Bad Connection
            //21. Cozy Bed
            //22. Elemental
            entityList.Add(new Entity("Bandit Captain", 30, 50, 5, "Cut the head"));
            entityList.Add(new Entity("Evil Spirit", 40, 40, 5, "Stando..."));
            entityList.Add(new Entity("Bad Connection", 43, 70, 5, "Your main enemy"));
            entityList.Add(new Entity("Cozy Bed", 0, 10, 5, "Have a rest"));
            entityList.Add(new Entity("Elemental", 50, 50, 5, "Oh shit..."));

        }
    }
}