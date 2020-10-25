using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Items
{
    public interface IInventory
    {
        Inventory inventory { get; set; }
        GameObject gameObject { get; }
    }
}

