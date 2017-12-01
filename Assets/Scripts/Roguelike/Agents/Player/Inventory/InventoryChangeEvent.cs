using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    [Serializable]
    public sealed class InventoryChangeEvent : UnityEvent<IEnumerable<Item>>
    {

    } 
}