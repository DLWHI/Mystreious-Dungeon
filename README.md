# Mystreious-Dungeon
RPG game about one adventurer who entered mystreious dungeon(sample project)
***
#### Dungeon
You are exploring dungeon with spooky enemies and a bunch of loot ou there. Your dungeon is completely random and it can have 16-32 various rooms in it. Each room is completely random too and can have 0-3 enemies and 0-4 loot in it. But if you're lucky enough, you may encounter loot room that may have more loot inside. So kiss your lucky charms ;)
#### Enemies and Battle
As almost every thing in this game, emenies are randomed. But to have some balance in this game, enemies are divided by levels from 1-5. Each level can be encoured from specific room, so you could find(or maybe not) better loot to fight them. Exception from this rule is level 5 enemies that can be encountered in any room with 1% chance. So if u find lvl 5 enemy in first room, maybe its not your lucky day.

Game will list you enemies in current room and you have choose one of them by inputtin num of this enemy in list to interract with it. After you have chosen the enemy, you choose what you will do. Pressing 'A'(then enter) will attack enemy, 'I' open your inventory, and if you want to choose another enemy press 'Backspace'. After you move all enemies in room will attack you and you will recieve damage. Opening inventory or attacking counts as move, so when you close your invenory you will recieve damage.

After you kill all enemies, you will be able to search room for loot and move forward. Entering next room regenerates some of your hitpoints and mana.
#### Attacking
Your damage is based on your stats and your currently equipped weapon. Each weapon(and you) has damage and 2 damage type: *physical* and *magical*. Physical damage is only affected by your strength(and agility a little bit) so its doesn't need mana to attack. But each magic attack needs some mana to make it, so if you won't have mana you will be forced to strike with your bare hands. Intellect affects your mana and magical damage, so if you're some stooped barbarian you won't be able to use almost all magical weapons(you simply won't have enough mana).
#### Stats
You can have 3 stats:
* Strength - affects your hitpoints, hitpoint regen and physical damage.
* Agility - affects your armor and physical damage, but strong
* Intellect - affects your mana pool, mana regen and magic damage
At start you will be given 15 points that you can spend in one of this stats. Each stat can have maximum of 9 points. After you finish, the game will show you what you got. Stats cannot be changed in game or increased, so choose wisely
#### Loot and moving to next room
So you killed all enemies and now you can see what you got! After battle, game will list you all loot in room. You can take what you want by simple inputting nums of items in list. You can first input 'A' to take all loot and after you press enter you will be given some time to see what you got. Note that if you input something wrong, game will just ignore that ~~SO MAYBE NEXT TIME MOTHERFUCKER~~.

After you've done with loot you can open your inventory(I button), equipment(E), move to next room(N) or watch back for loot if you missed something(L).
#### Inventory, slots and items
You don't have infinte bag, so your inventory can hold maximum 15 items. As you open inventory and see list of your items, you can input num of item as in battle process. Each item can be used or thrown to room loot list, so if you thought that you don't need something but suddenly you realized that you need it you may look back to room.

You have 4 slots:3 armor slots and 1 weapon. Armor is your main damage decreasing stat. It is based on your agility(base armor of you) and armor that you wear. Armor directly decreases damage you recieve, so, for example, room has 10 damage, and you have 4 armor, you will recieve 6 damage. Its not forbidden to have more amror than damage, you will just recive 0 damage.

If you run out mana or you need to heal yourself, you can use one of the potions you find in dungeon. But be aware, that after you close your inventory you will recieve damage, so it may cause no effect to you.

