using System;
using System.Collections.Generic;
using System.Text;

namespace AzureRedis
{
    class ToDoItem
    {

        private string itemid;
        private string itemName;
        private DateTime itemDateTime;

        public string Itemid
        {
            get { return itemid; }
            set { value = itemid; }
        }

        public string ItemName
        {
            get { return itemName; }
            set { value = itemName; }
        }

        public DateTime ItemDateTime
        {
            get { return itemDateTime; }
            set { value = itemDateTime; }
        }


        public ToDoItem(string id, string name, DateTime datetime)
        {
            itemid = id; itemName = name; itemDateTime = datetime;

        }

    }
}
