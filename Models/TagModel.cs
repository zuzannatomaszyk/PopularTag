using System;
using System.Collections.Generic;

namespace PopularTag.Models
{
    public class TagModel
    {
        public int count { get; set; }
        public string name { get; set; }
        public float percentage { get; set; }
        
    }
    public class ItemModel 
    {
        public List<TagModel> items;
    }
}