using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    public interface IInventory
    {
        Weapon Weapon { get; set; }
        BodyArmor BodyArmor { get; set; }
        Shield Shield { get; set; }
    } 
}