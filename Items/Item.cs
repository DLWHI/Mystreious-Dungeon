using System;

namespace Mysterious_Dungeon.Items
{
    abstract class Item//abstract class of items
    {
        public Item()
        {
            return;
        }
        public Item(string name, string rareness, string desc)
        {
            Name = name;
            Rareness = rareness;
            Desc = desc;
        }

        protected string name;//name of item
        protected string desc;//short description
        protected string rareness;//how rare this item is
        protected string slot;

        public string Name
        {
            get
            {
                return name;
            }
            protected set
            {
                if (value == "\0" || value == "" || value == null || String.IsNullOrWhiteSpace(value))
                    return;
                else
                    name = value;
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
        public string Rareness
        {
            get
            {
                return rareness;
            }
            protected set
            {
                if (value == null || String.IsNullOrWhiteSpace(value) || value == "\0" || value == "")
                    return;
                else if (value == "white" || value == "green" || value == "blue" || value == "purple")
                    rareness = value;
            }
        }
        public virtual string Slot
        {
            get
            {
                return slot;
            }
            protected set
            {
                if (value == null || String.IsNullOrWhiteSpace(value) || value == "\0" || value == "")
                    return;
            }
        }
        public abstract void ItemStats();//each item has its stats so we need to override them
        public abstract void ShortStats();
    }
}
