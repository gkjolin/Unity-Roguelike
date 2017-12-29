using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    public sealed class AttackResolver : MonoBehaviour
    {
        const double ARMOR_COEFFICIENT = 3;

        int RandomRoll { get { return UnityEngine.Random.Range(0, 100); } }

        public AttackResult ResolvePhysicalAttack(Attack atk, Defense def)
        {
            var result = new AttackResult();

            result.OriginalHealth = def.CurrentHealth;
            result.NameOfAttacker = atk.NameOfAttacker;
            result.NameOfDefender = def.NameOfDefender;

            if (RandomRoll > atk.Accuracy) // miss 
            {
                result.IsMiss = true;
                Logger.Log(result.ToString());
                return result;
            }
            if (RandomRoll < def.PhysicalEvasion) // evaded
            {
                result.IsEvaded = true;
                Logger.Log(result.ToString());
                return result;
            }
            if (RandomRoll < atk.CritChance)
            {
                result.IsCritical = true;
            }

            int physicalReduction = def.PhysicalDamageSubtraction;
            // magical reduction gets distributed across the nonphysical types until it's depleted
            int magicalReduction = def.NonPhysicalDamageSubstraction;

            result.MagicDamage = ComputeDamage(atk.MagicDamage, 0, 0, magicalReduction);
            magicalReduction -= result.MagicDamage.Reduced;

            result.LightningDamage = ComputeDamage(atk.LightningDamage, def.LightningResistance, def.LightningAbsorption, magicalReduction);
            magicalReduction -= result.LightningDamage.Reduced;

            result.FireDamage = ComputeDamage(atk.FireDamage, def.FireResistance, def.FireAbsorption, magicalReduction);
            magicalReduction -= result.FireDamage.Reduced;

            result.ColdDamage = ComputeDamage(atk.ColdDamage, def.ColdResistance, def.ColdAbsorption, magicalReduction);
            magicalReduction -= result.ColdDamage.Reduced;

            result.PoisonDamage = ComputeDamage(atk.PoisonDamage, def.PoisonResistance, def.PoisonAbsoprtion, magicalReduction);
            magicalReduction -= result.PoisonDamage.Reduced;

            int critFactor = result.IsCritical ? atk.CritMultiplier : 1;
            result.PhysicalDamage = ComputePhysicalDamage(atk.PhysicalDamage, def.Armor, critFactor, physicalReduction);
            Logger.Log(result.ToString());
            return result;
        }

        DamageInfo ComputeDamage(MagnitudeRange damageRange, int mitigation, int absorption, int reduction)
        {
            int rawDamage = damageRange.Interpolate(QualityRoll.GetRandom());
            int afterResistance = ApplyMitigation(rawDamage, mitigation);
            int absorbed = rawDamage - ApplyMitigation(rawDamage, absorption);
            DamageInfo damageResult = new DamageInfo(rawDamage, rawDamage - afterResistance, absorbed, reduction);
            return damageResult;
        }

        DamageInfo ComputePhysicalDamage(MagnitudeRange damageRange, int armor, int critFactor, int reduction)
        {
            int mitigation = ArmorToMitigation(armor);
            int rawDamage = critFactor * damageRange.Interpolate(QualityRoll.GetRandom());
            int afterArmor = ApplyMitigation(rawDamage, mitigation);
            DamageInfo damageResult = new DamageInfo(rawDamage, rawDamage - afterArmor, 0, reduction);
            return damageResult;
        }

        int ArmorToMitigation(int armor)
        {
            if (armor > 0)
            {
                // This is the critical formula for armor mitigation. It's the same formula used by Diablo 3 for both
                // armor and resistance. As armor increases, the value will approach 100 (percent), and for small values,
                // it will be approximately 0. The coefficient slows down the growth. 
                return (int)(1 / (0.01 + ARMOR_COEFFICIENT / armor));
            }
            else
            {
                return 0;
            }
        }

        static int ApplyMitigation(int baseValue, int mitigation)
        {
            return (baseValue * (100 - mitigation)) / 100;
        }
    } 
}