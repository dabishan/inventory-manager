﻿using System;
using System.Data.Entity.ModelConfiguration;

namespace InventoryManager.Models
{
    public class History
    {
        public int Id { get; set; }

        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }

        public DateTime AssignedOn { get; set; }

        public string AssignedById { get; set; }
        public ApplicationUser AssignedBy { get; set; }

        public int AssignedToId { get; set; }
        public Owner AssignedTo { get; set; }

        public InventoryStatus StatusAssigned { get; set; }

    }
}