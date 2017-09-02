﻿using System;

namespace InventoryManager.Models
{
    public class History
    {
        public int Id { get; set; }
        public DateTime Assigned { get; set; }
        public InventoryStatus Status { get; set; }
        public User AssignedBy { get; set; }
        public Owner AssignedTo { get; set; }
        public InventoryStatus StatusAssigned { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}