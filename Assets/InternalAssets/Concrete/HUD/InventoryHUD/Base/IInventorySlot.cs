﻿using SouthBasement.InventorySystem.ItemBase;
using SouthBasement.InventorySystem;

namespace SouthBasement.InventoryHUD.Base
{
    public interface IInventorySlot
    {
        Item CurrentItemNonGeneric { get; }
    }
}